using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cleanplanetapp;
using System.Linq;
using cleanplanetapp.Svc;

namespace cleanplanetapp.Tests
{
    [TestClass]
    public class CalculationTests
    {
        private ApplicationDbContext ctx;

        [TestInitialize]
        public void Setup()
        {
            ctx = new ApplicationDbContext();
        }


        // ПОЛОЖИТЕЛЬНЫЕ ТЕСТЫ

        [TestMethod]
        public void CalculateServiceCost_ValidService_ReturnsPositiveCost()
        {
            
            var existingService = ctx.Services.FirstOrDefault();
            Assert.IsNotNull(existingService, "Нет доступных услуг в БД.");// Проверка что себестоимость услуги корректно рассчитывается

            decimal result = ServiceFunc.CalculateServiceCost(existingService.ServiceId);

            Assert.IsTrue(result > 0, "Себестоимость должна быть больше нуля.");
        }

        [TestMethod]
        public void CalculateRequiredMaterial_ValidParams_ReturnsPositiveInt()
        {
            
            var sm = ctx.ServiceMaterials.FirstOrDefault();
            Assert.IsNotNull(sm, "Нет данных в ServiceMaterials."); //расчёт количества материала при корректных данных

            int result = ServiceFunc.CalculateRequiredMaterial(
                sm.ServiceId, sm.MaterialId, 3, 2.5, 1.2);

            Assert.IsTrue(result > 0, "Количество материала должно быть > 0.");
        }

        [TestMethod]
        public void CalculateRequiredMaterial_MultipleParams_ReturnsExpectedValue()
        {
           
            var sm = ctx.ServiceMaterials.FirstOrDefault();
            Assert.IsNotNull(sm, "Нет данных в ServiceMaterials.");  // Проверка что функция корректно учитывает все параметры

            int result = ServiceFunc.CalculateRequiredMaterial(
                sm.ServiceId, sm.MaterialId, 2, 1.0, 1.0, 1.0);

            Assert.IsTrue(result > 0, "Ожидается положительный результат.");
        }

        // НЕГАТИВНЫЕ ТЕСТЫ 

        [TestMethod]
        public void CalculateServiceCost_InvalidService_ReturnsMinusOne()
        {
            
            decimal result = ServiceFunc.CalculateServiceCost(-1);
            Assert.AreEqual(-1, result, "Ожидалось значение -1 при неверном ID."); // Проверка что при несуществующем serviceId возвращается -1
        }

        [TestMethod]
        public void CalculateRequiredMaterial_InvalidService_ReturnsMinusOne()
        {
            
            int result = ServiceFunc.CalculateRequiredMaterial(-1, 1, 1, 1.0); // Проверка на расчет количества материалов по несуществующей услуге
            Assert.AreEqual(-1, result, "Ожидалось -1 при неверном serviceId.");
        }

        [TestMethod]
        public void CalculateRequiredMaterial_InvalidParams_ReturnsMinusOne()
        {
            
            var sm = ctx.ServiceMaterials.FirstOrDefault();
            Assert.IsNotNull(sm, "Нет данных в ServiceMaterials.");

            int result = ServiceFunc.CalculateRequiredMaterial(
                sm.ServiceId, sm.MaterialId, 2, -1.5);

            Assert.AreEqual(-1, result, "Ожидалось -1 при отрицательном параметре.");// Проверка что при отрицательных параметрах возвращается -1
        }
    }
}
