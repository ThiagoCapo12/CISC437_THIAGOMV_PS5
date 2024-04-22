using System;
using System.Collections.Generic;
using Eagles.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Eagles.EF.Data;

public partial class EaglesOracleContext : DbContext
{
    public EaglesOracleContext(DbContextOptions<EaglesOracleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AddressType> AddressTypes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryState> InventoryStates { get; set; }

    public virtual DbSet<InventoryStatus> InventoryStatuses { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderState> OrderStates { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<OrdersLine> OrdersLines { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPrice> ProductPrices { get; set; }

    public virtual DbSet<ProductStatus> ProductStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("UD_THIAGOMV")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("ADDRESS_PK");

            entity.Property(e => e.AddressState).IsFixedLength();
        });

        modelBuilder.Entity<AddressType>(entity =>
        {
            entity.HasKey(e => e.AddressTypeId).HasName("ADDRESS_TYPE_PK");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("CUSTOMER_PK");

            entity.HasOne(d => d.CustomerGender).WithMany(p => p.Customers).HasConstraintName("CUSTOMER_FK1");
        });

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.HasKey(e => e.CustomerAddressId).HasName("CUSTOMER_ADDRESS_PK");

            entity.HasOne(d => d.CustomerAddressAddress).WithMany(p => p.CustomerAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CUSTOMER_ADDRESS_FK2");

            entity.HasOne(d => d.CustomerAddressAddressType).WithMany(p => p.CustomerAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CUSTOMER_ADDRESS_FK3");

            entity.HasOne(d => d.CustomerAddressCustomer).WithMany(p => p.CustomerAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CUSTOMER_ADDRESS_FK1");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("GENDER_PK");

            entity.Property(e => e.GenderId).HasDefaultValueSql("sys_guid() ");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("INVENTORY_PK");

            entity.HasOne(d => d.InventoryProduct).WithMany(p => p.Inventories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("INVENTORY_FK1");
        });

        modelBuilder.Entity<InventoryState>(entity =>
        {
            entity.HasKey(e => e.InventoryStateId).HasName("INVENTORY_STATE_PK");

            entity.Property(e => e.InventoryStateId).HasDefaultValueSql("sys_guid() ");
            entity.Property(e => e.InventoryStateTs).HasDefaultValueSql("current_timestamp ");

            entity.HasOne(d => d.InventoryStateInventory).WithMany(p => p.InventoryStates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("INVENTORY_STATE_FK2");

            entity.HasOne(d => d.InventoryStateInventoryStatus).WithMany(p => p.InventoryStates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("INVENTORY_STATE_FK1");
        });

        modelBuilder.Entity<InventoryStatus>(entity =>
        {
            entity.HasKey(e => e.InventoryStatusId).HasName("INVENTORY_STATUS_PK");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrdersId).HasName("ORDERS_PK");

            entity.HasOne(d => d.OrdersCustomer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ORDERS_FK1");
        });

        modelBuilder.Entity<OrderState>(entity =>
        {
            entity.HasKey(e => e.OrderStateId).HasName("ORDER_STATE_PK");

            entity.HasOne(d => d.OrderStateOrderStatus).WithMany(p => p.OrderStates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ORDER_STATE_FK2");

            entity.HasOne(d => d.OrderStateOrders).WithMany(p => p.OrderStates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ORDER_STATE_FK1");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("ORDER_STATUS_PK");

            entity.HasOne(d => d.OrderStatusNextOrderStatus).WithMany(p => p.InverseOrderStatusNextOrderStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ORDER_STATUS_FK1");
        });

        modelBuilder.Entity<OrdersLine>(entity =>
        {
            entity.HasKey(e => e.OrdersLineId).HasName("ORDERS_LINE_PK");

            entity.HasOne(d => d.OrdersLineInventory).WithMany(p => p.OrdersLines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ORDERS_LINE_FK3");

            entity.HasOne(d => d.OrdersLineOrders).WithMany(p => p.OrdersLines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ORDERS_LINE_FK1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRODUCT_PK");

            entity.Property(e => e.ProductId).HasDefaultValueSql("SYS_GUID()");

            entity.HasOne(d => d.ProductProductStatus).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PRODUCT_FK1");
        });

        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.HasKey(e => e.ProductPriceId).HasName("PRODUCT_PRICE_PK");

            entity.HasOne(d => d.ProductPriceProduct).WithMany(p => p.ProductPrices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PRODUCT_PRICE_FK1");
        });

        modelBuilder.Entity<ProductStatus>(entity =>
        {
            entity.HasKey(e => e.ProductStatusId).HasName("PRODUCT_STATUS_PK");

            entity.Property(e => e.ProductStatusId).HasDefaultValueSql("SYS_GUID()");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
