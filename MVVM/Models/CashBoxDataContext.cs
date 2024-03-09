using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cashbox.MVVM.Models;

public partial class CashBoxDataContext : DbContext
{
    public CashBoxDataContext()
    {
    }

    private static CashBoxDataContext? _context;
    public static CashBoxDataContext Context => _context ??= new CashBoxDataContext();

    public CashBoxDataContext(DbContextOptions<CashBoxDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuthHistory> AuthHistories { get; set; }
    public virtual DbSet<AppSettings> AppSettings { get; set; }

    public virtual DbSet<AutoDreport> AutoDreports { get; set; }

    public virtual DbSet<DailyReport> DailyReports { get; set; }

    public virtual DbSet<MoneyBox> MoneyBoxes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:\\Users\\StateUser\\source\\repos\\CashBox\\MVVM\\Models\\Data.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppSettings>(entity =>
        {
            entity.ToTable("AppSettings");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MoneyBox).HasColumnName("moneybox");
            entity.Property(e => e.Salary).HasColumnName("salary");
            entity.Property(e => e.AwardProcent).HasColumnName("awardprocent");
            entity.Property(e => e.MainEmail).HasColumnName("mainemail");
        });

        modelBuilder.Entity<AuthHistory>(entity =>
        {
            entity.ToTable("AuthHistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Datetime)
                .HasColumnType("datetime")
                .HasColumnName("datetime");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.AuthHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthHistory_Users");
        });

        modelBuilder.Entity<AutoDreport>(entity =>
        {
            entity.HasKey(e => e.DailyReportId);

            entity.ToTable("AutoDReport");

            entity.Property(e => e.DailyReportId)
                .ValueGeneratedNever()
                .HasColumnName("daily_report_id");
            entity.Property(e => e.AutoProceeds).HasColumnName("auto_proceeds");
            entity.Property(e => e.Award).HasColumnName("award");
            entity.Property(e => e.Forfeit).HasColumnName("forfeit");
            entity.Property(e => e.Salary).HasColumnName("salary");

            entity.HasOne(d => d.DailyReport).WithOne(p => p.AutoDreport)
                .HasForeignKey<AutoDreport>(d => d.DailyReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AutoDReport_DailyReport");
        });

        modelBuilder.Entity<DailyReport>(entity =>
        {
            entity.ToTable("DailyReport");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CloseTime).HasColumnName("close_time");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.OpenTime).HasColumnName("open_time");
            entity.Property(e => e.Proceeds).HasColumnName("proceeds");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.DailyReports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyReport_Users");
        });

        modelBuilder.Entity<MoneyBox>(entity =>
        {
            entity.ToTable("MoneyBox");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Money).HasColumnName("money");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.SellCost).HasColumnName("sell_cost");
            entity.Property(e => e.SellDatetime)
                .HasColumnType("datetime")
                .HasColumnName("sell_datetime");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_PaymentMethod");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.ToTable("OrderProduct");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.PurchaseСost).HasColumnName("purchase_сost");
            entity.Property(e => e.SellCost).HasColumnName("sell_cost");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderProduct_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderProduct_Product");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("PaymentMethod");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Method)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("method");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArticulCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("articul_code");
            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("brand");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasColumnType("image")
                .HasColumnName("image");
            entity.Property(e => e.PurchaseСost).HasColumnName("purchase_сost");
            entity.Property(e => e.SellCost).HasColumnName("sell_cost");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategory");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("ProductCategory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.ToTable("Refund");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BuyDate).HasColumnName("buy_date");
            entity.Property(e => e.IsPurchased).HasColumnName("isPurchased");
            entity.Property(e => e.IsSuccessRefund).HasColumnName("isSuccessRefund");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Reason)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("reason");

            entity.HasOne(d => d.Product).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Refund_Product");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Role1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("Stock");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_id");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.Product).WithOne(p => p.Stock)
                .HasForeignKey<Stock>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Product");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Pin).HasColumnName("pin");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserInfo");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("surname");

            entity.HasOne(d => d.Role).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserInfo_Roles");

            entity.HasOne(d => d.User).WithOne(p => p.UserInfo)
                .HasForeignKey<UserInfo>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserInfo_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
