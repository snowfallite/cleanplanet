using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cleanplanetapp.Svc
{
    public static class ServiceFunc
    {
        public static decimal CalculateServiceCost(int serviceId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                
                var service = ctx.Services.FirstOrDefault(s => s.ServiceId == serviceId);
                if (service == null)
                    return -1;

           
                var serviceMaterials = ctx.ServiceMaterials
                                          .Where(sm => sm.ServiceId == serviceId)
                                          .Join(ctx.Materials,
                                                sm => sm.MaterialId,
                                                m => m.MaterialId,
                                                (sm, m) => new { sm.ConsumptionNorm, m.CurrentPrice })
                                          .ToList();

              
                var position = ctx.Positions.FirstOrDefault(p => p.PositionId == service.RequiredPositionId);
                if (position == null)
                    return -1;

                
                decimal materialCost = serviceMaterials.Sum(x => x.ConsumptionNorm * x.CurrentPrice);

                decimal laborCost = service.TimeNormHours * position.HourlyRate;

                return materialCost + laborCost;
            }
        }

        public static int CalculateRequiredMaterial(int serviceId, int materialId, int serviceCount, params double[] serviceParams)
        {
            if (serviceCount <= 0 || serviceParams == null || serviceParams.Length == 0)
                return -1;

            using (var ctx = new ApplicationDbContext())
            {
          
                var sm = ctx.ServiceMaterials.FirstOrDefault(x => x.ServiceId == serviceId && x.MaterialId == materialId);
                if (sm == null)
                    return -1;

                foreach (var p in serviceParams)
                    if (p <= 0) return -1;

            
                double paramProduct = 1.0;
                foreach (var p in serviceParams)
                    paramProduct *= p;

             
                double baseAmount = paramProduct * (double)sm.ServiceCoefficient;
                baseAmount *= 1 + (double)sm.OverusePercent;

             
                double totalAmount = baseAmount * serviceCount;

                return (int)Math.Ceiling(totalAmount);
            }
        }


    }
}
