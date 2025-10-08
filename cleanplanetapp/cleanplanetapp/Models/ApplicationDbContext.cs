using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace cleanplanetapp
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=PostgresContext")
        {
        }

        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ServiceMaterial> ServiceMaterials { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<ShiftEmployee> ShiftEmployees { get; set; }
        public DbSet<PartnerRatingHistory> PartnersRatingHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

   
            modelBuilder.Entity<ServiceMaterial>()
                .HasKey(sm => new { sm.ServiceId, sm.MaterialId });

            modelBuilder.Entity<ShiftEmployee>()
                .HasKey(se => new { se.ShiftId, se.EmployeeId });

          
            modelBuilder.Entity<Position>().ToTable("Positions");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Shift>().ToTable("Shifts");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Service>().ToTable("Services");
            modelBuilder.Entity<Supplier>().ToTable("Suppliers");
            modelBuilder.Entity<Material>().ToTable("Materials");
            modelBuilder.Entity<ServiceMaterial>().ToTable("ServiceMaterials");
            modelBuilder.Entity<Partner>().ToTable("Partners");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Delivery>().ToTable("Deliveries");
            modelBuilder.Entity<ShiftEmployee>().ToTable("ShiftEmployees");
            modelBuilder.Entity<PartnerRatingHistory>().ToTable("PartnersRatingHistory");

      
            modelBuilder.Entity<Position>()
                .Property(p => p.PositionId).HasColumnName("position_id");
            modelBuilder.Entity<Position>()
                .Property(p => p.PositionName).HasColumnName("position_name");
            modelBuilder.Entity<Position>()
                .Property(p => p.HourlyRate).HasColumnName("hourly_rate");

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeId).HasColumnName("employee_id");
            modelBuilder.Entity<Employee>()
                .Property(e => e.FullName).HasColumnName("full_name");
            modelBuilder.Entity<Employee>()
                .Property(e => e.PositionId).HasColumnName("position_id");
            modelBuilder.Entity<Employee>()
                .Property(e => e.Status).HasColumnName("status");
            modelBuilder.Entity<Employee>()
                .Property(e => e.Username).HasColumnName("username");
            modelBuilder.Entity<Employee>()
                .Property(e => e.PasswordHash).HasColumnName("password_hash");

            modelBuilder.Entity<Shift>()
                .Property(s => s.ShiftId).HasColumnName("shift_id");
            modelBuilder.Entity<Shift>()
                .Property(s => s.EmployeeId).HasColumnName("employee_id");
            modelBuilder.Entity<Shift>()
                .Property(s => s.ShiftDate).HasColumnName("shift_data");
            modelBuilder.Entity<Shift>()
                .Property(s => s.StartTime).HasColumnName("start_time");
            modelBuilder.Entity<Shift>()
                .Property(s => s.EndTime).HasColumnName("end_time");
            modelBuilder.Entity<Shift>()
                .Property(s => s.Status).HasColumnName("status");
            modelBuilder.Entity<Shift>()
                .Property(s => s.HoursWorked).HasColumnName("hours_worked");

            modelBuilder.Entity<Client>()
                .Property(c => c.ClientId).HasColumnName("client_id");
            modelBuilder.Entity<Client>()
                .Property(c => c.FullName).HasColumnName("full_name");
            modelBuilder.Entity<Client>()
                .Property(c => c.Contact).HasColumnName("contact");
            modelBuilder.Entity<Client>()
                .Property(c => c.RegistrationDate).HasColumnName("registration_date");

            modelBuilder.Entity<Service>()
                .Property(s => s.ServiceId).HasColumnName("service_id");
            modelBuilder.Entity<Service>()
                .Property(s => s.ServiceName).HasColumnName("service_name");
            modelBuilder.Entity<Service>()
                .Property(s => s.Description).HasColumnName("description");
            modelBuilder.Entity<Service>()
                .Property(s => s.Price).HasColumnName("price");
            modelBuilder.Entity<Service>()
                .Property(s => s.TimeNormHours).HasColumnName("time_norm_hours");
            modelBuilder.Entity<Service>()
                .Property(s => s.RequiredPositionId).HasColumnName("required_position_id");
            modelBuilder.Entity<Service>()
                .HasRequired(s => s.RequiredPosition)
                .WithMany(p => p.Services)
                .HasForeignKey(s => s.RequiredPositionId);


            modelBuilder.Entity<Supplier>()
                .Property(s => s.SupplierId).HasColumnName("supplier_id");
            modelBuilder.Entity<Supplier>()
                .Property(s => s.Name).HasColumnName("name");
            modelBuilder.Entity<Supplier>()
                .Property(s => s.Contact).HasColumnName("contact");

            modelBuilder.Entity<Material>()
                .Property(m => m.MaterialId).HasColumnName("material_id");
            modelBuilder.Entity<Material>()
                .Property(m => m.Name).HasColumnName("name");
            modelBuilder.Entity<Material>()
                .Property(m => m.CurrentPrice).HasColumnName("current_price");
            modelBuilder.Entity<Material>()
                .Property(m => m.Quantity).HasColumnName("quantity");
            modelBuilder.Entity<Material>()
                .Property(m => m.Unit).HasColumnName("unit");
            modelBuilder.Entity<Material>()
                .Property(m => m.SupplierId).HasColumnName("supplier_id");

            modelBuilder.Entity<ServiceMaterial>()
                .Property(sm => sm.ServiceId).HasColumnName("service_id");
            modelBuilder.Entity<ServiceMaterial>()
                .Property(sm => sm.MaterialId).HasColumnName("material_id");
            modelBuilder.Entity<ServiceMaterial>()
                .Property(sm => sm.ConsumptionNorm).HasColumnName("consumption_norm");
            modelBuilder.Entity<ServiceMaterial>()
                .Property(sm => sm.OverusePercent).HasColumnName("overuse_percent");
            modelBuilder.Entity<ServiceMaterial>()
                .Property(sm => sm.ServiceCoefficient).HasColumnName("service_coefficient");
            modelBuilder.Entity<Partner>()
                .Property(p => p.PartnerId).HasColumnName("partner_id");

            modelBuilder.Entity<Partner>()
                .Property(p => p.PartnerType).HasColumnName("partner_type");
            modelBuilder.Entity<Partner>()
                .Property(p => p.Name).HasColumnName("name");
            modelBuilder.Entity<Partner>()
                .Property(p => p.Director).HasColumnName("partner_director");
            modelBuilder.Entity<Partner>()
                .Property(p => p.Address).HasColumnName("address");
            modelBuilder.Entity<Partner>()
                .Property(p => p.Email).HasColumnName("email");
            modelBuilder.Entity<Partner>()
               .Property(p => p.Phone).HasColumnName("phone");
            modelBuilder.Entity<Partner>()
                .Property(p => p.Commission).HasColumnName("commission");
            modelBuilder.Entity<Partner>()
                .Property(p => p.Rating).HasColumnName("rating");

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderId).HasColumnName("order_id");
            modelBuilder.Entity<Order>()
                .Property(o => o.ClientId).HasColumnName("client_id");
            modelBuilder.Entity<Order>()
                .Property(o => o.ServiceId).HasColumnName("service_id");
            modelBuilder.Entity<Order>()
                .Property(o => o.Quantity).HasColumnName("quantity");
            modelBuilder.Entity<Order>()
                .Property(o => o.ShiftId).HasColumnName("shift_id");
            modelBuilder.Entity<Order>()
                .Property(o => o.PartnerId).HasColumnName("partner_id");
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderDate).HasColumnName("order_date");
            modelBuilder.Entity<Order>()
                .Property(o => o.CompletionDate).HasColumnName("completion_date");
            modelBuilder.Entity<Order>()
                .Property(o => o.Status).HasColumnName("status");
            modelBuilder.Entity<Order>()
                .Property(o => o.CostPrice).HasColumnName("cost_price");
            modelBuilder.Entity<Order>()
                .Property(o => o.FinalPrice).HasColumnName("final_price");

            modelBuilder.Entity<Delivery>()
                .Property(d => d.DeliveryId).HasColumnName("delivery_id");
            modelBuilder.Entity<Delivery>()
                .Property(d => d.SupplierId).HasColumnName("supplier_id");
            modelBuilder.Entity<Delivery>()
                .Property(d => d.MaterialId).HasColumnName("material_id");
            modelBuilder.Entity<Delivery>()
                .Property(d => d.QuantityDelivery).HasColumnName("quantity_delivery");
            modelBuilder.Entity<Delivery>()
                .Property(d => d.DeliveryDate).HasColumnName("delivery_date");
            modelBuilder.Entity<Delivery>()
                .Property(d => d.EmployeeId).HasColumnName("employee_id");

            modelBuilder.Entity<ShiftEmployee>()
                .Property(se => se.ShiftId).HasColumnName("shift_id");
            modelBuilder.Entity<ShiftEmployee>()
                .Property(se => se.EmployeeId).HasColumnName("employee_id");

            modelBuilder.Entity<PartnerRatingHistory>()
                .Property(prh => prh.HistoryId).HasColumnName("history_id");
            modelBuilder.Entity<PartnerRatingHistory>()
                .Property(prh => prh.PartnerId).HasColumnName("partner_id");
            modelBuilder.Entity<PartnerRatingHistory>()
                .Property(prh => prh.ChangedAt).HasColumnName("changed_at");
            modelBuilder.Entity<PartnerRatingHistory>()
                .Property(prh => prh.OldRating).HasColumnName("old_rating");
            modelBuilder.Entity<PartnerRatingHistory>()
                .Property(prh => prh.NewRating).HasColumnName("new_rating");
            modelBuilder.Entity<PartnerRatingHistory>()
                .Property(prh => prh.ChangedBy).HasColumnName("changed_by");
            modelBuilder.Entity<PartnerRatingHistory>()
                .Property(prh => prh.Reason).HasColumnName("reason");

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Position
    {
        [Key]
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public decimal HourlyRate { get; set; }

      
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }

    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public int PositionId { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }


        public virtual Position Position { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<PartnerRatingHistory> PartnerRatingHistories { get; set; }
        public virtual ICollection<ShiftEmployee> ShiftEmployees { get; set; }
    }


    public class Shift
    {
        [Key]
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public decimal? HoursWorked { get; set; }


        public virtual Employee Employee { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ShiftEmployee> ShiftEmployees { get; set; }
    }


    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        public string FullName { get; set; }
        public string Contact { get; set; }
        public DateTime RegistrationDate { get; set; }


        public virtual ICollection<Order> Orders { get; set; }
    }


    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal TimeNormHours { get; set; }
        public int RequiredPositionId { get; set; }


        public virtual Position RequiredPosition { get; set; }
        public virtual ICollection<ServiceMaterial> ServiceMaterials { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }


    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }


        public virtual ICollection<Material> Materials { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
    }

    public class Material
    {
        [Key]
        public int MaterialId { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public int SupplierId { get; set; }


        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ServiceMaterial> ServiceMaterials { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
    }


    public class ServiceMaterial
    {
        public int ServiceId { get; set; }
        public int MaterialId { get; set; }
        public decimal ConsumptionNorm { get; set; }

        public decimal OverusePercent { get; set; }

     
        public decimal ServiceCoefficient { get; set; }

  
        public virtual Service Service { get; set; }
        public virtual Material Material { get; set; }
    }


    public class Partner
    {
        [Key]
        public int PartnerId { get; set; }
        public string Name { get; set; }

        public string Director { get; set; }
        public string PartnerType { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }
        public decimal Commission { get; set; }
        public decimal Rating { get; set; }

     
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PartnerRatingHistory> PartnerRatingHistories { get; set; }
    }


    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public int ShiftId { get; set; }
        public int PartnerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Status { get; set; }
        public decimal CostPrice { get; set; }
        public decimal FinalPrice { get; set; }

        public int Quantity { get; set; }

    
        public virtual Client Client { get; set; }
        public virtual Service Service { get; set; }
        public virtual Shift Shift { get; set; }
        public virtual Partner Partner { get; set; }
    }


    public class Delivery
    {
        [Key]
        public int DeliveryId { get; set; }
        public int SupplierId { get; set; }
        public int MaterialId { get; set; }
        public decimal QuantityDelivery { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int EmployeeId { get; set; }


        public virtual Supplier Supplier { get; set; }
        public virtual Material Material { get; set; }
        public virtual Employee Employee { get; set; }
    }


    public class ShiftEmployee
    {
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }

     
        public virtual Shift Shift { get; set; }
        public virtual Employee Employee { get; set; }
    }


    public class PartnerRatingHistory
    {
        [Key]
        public int HistoryId { get; set; }
        public int PartnerId { get; set; }
        public DateTime ChangedAt { get; set; }
        public decimal OldRating { get; set; }
        public decimal NewRating { get; set; }


        public int ChangedBy { get; set; }
        public string Reason { get; set; }


      
        public virtual Partner Partner { get; set; }

        [ForeignKey(nameof(ChangedBy))]
        public virtual Employee Employee { get; set; }
    }
}
