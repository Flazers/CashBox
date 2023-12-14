using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Cashbox.Data
{
    public partial class AppContext : DbContext
    {
        public AppContext()
            : base("name=Model15")
        {
        }

        public virtual DbSet<AuthHistory> AuthHistory { get; set; }
        public virtual DbSet<AutoDReport> AutoDReport { get; set; }
        public virtual DbSet<DailyReport> DailyReport { get; set; }
        public virtual DbSet<MoneyBox> MoneyBox { get; set; }
        public virtual DbSet<OrderProduct> OrderProduct { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<Refund> Refund { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<TFAData> TFAData { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyReport>()
                .HasOptional(e => e.AutoDReport)
                .WithRequired(e => e.DailyReport);

            modelBuilder.Entity<Orders>()
                .HasMany(e => e.OrderProduct)
                .WithRequired(e => e.Orders)
                .HasForeignKey(e => e.order_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PaymentMethod>()
                .Property(e => e.method)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMethod>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.PaymentMethod)
                .HasForeignKey(e => e.payment_method_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.articul_code)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.brand)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderProduct)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Refund)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasOptional(e => e.Stock)
                .WithRequired(e => e.Product);

            modelBuilder.Entity<ProductCategory>()
                .Property(e => e.category)
                .IsUnicode(false);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(e => e.Product)
                .WithRequired(e => e.ProductCategory)
                .HasForeignKey(e => e.category_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Refund>()
                .Property(e => e.reason)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.role)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.UserInfo)
                .WithRequired(e => e.Roles)
                .HasForeignKey(e => e.role_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TFAData>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.surname)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.patronymic)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.location)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.login)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.AuthHistory)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.DailyReport)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasOptional(e => e.TFAData)
                .WithRequired(e => e.Users);

            modelBuilder.Entity<Users>()
                .HasOptional(e => e.UserInfo)
                .WithRequired(e => e.Users);
        }
    }
}
