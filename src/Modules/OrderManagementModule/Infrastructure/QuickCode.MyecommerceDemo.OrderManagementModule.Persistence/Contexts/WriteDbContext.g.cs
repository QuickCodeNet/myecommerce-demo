using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Persistence.Contexts;

public partial class WriteDbContext : DbContext
{
	public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options) { }


	public virtual DbSet<Order> Order { get; set; }

	public virtual DbSet<OrderItem> OrderItem { get; set; }

	public virtual DbSet<Address> Address { get; set; }

	public virtual DbSet<ShippingMethod> ShippingMethod { get; set; }

	public virtual DbSet<OrderNote> OrderNote { get; set; }

	public virtual DbSet<OrderStatusHistory> OrderStatusHistory { get; set; }

	public virtual DbSet<AuditLog> AuditLog { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Order>()
		.Property(b => b.Status)
		.IsRequired()
		.HasDefaultValueSql("'PENDING'");


		var converterOrderStatus = new ValueConverter<OrderStatus, string>(
		v => v.ToString(),
		v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

		modelBuilder.Entity<Order>()
		.Property(b => b.Status)
		.HasConversion(converterOrderStatus);

		modelBuilder.Entity<Address>()
		.Property(b => b.IsDefaultShipping)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<Address>()
		.Property(b => b.IsDefaultBilling)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<ShippingMethod>()
		.Property(b => b.IsActive)
		.IsRequired()
		.HasDefaultValue(true);

		modelBuilder.Entity<OrderNote>()
		.Property(b => b.IsCustomerVisible)
		.IsRequired()
		.HasDefaultValue(false);


		var converterOrderStatusHistoryPreviousStatus = new ValueConverter<OrderStatus, string>(
		v => v.ToString(),
		v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

		modelBuilder.Entity<OrderStatusHistory>()
		.Property(b => b.PreviousStatus)
		.HasConversion(converterOrderStatusHistoryPreviousStatus);


		var converterOrderStatusHistoryNewStatus = new ValueConverter<OrderStatus, string>(
		v => v.ToString(),
		v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

		modelBuilder.Entity<OrderStatusHistory>()
		.Property(b => b.NewStatus)
		.HasConversion(converterOrderStatusHistoryNewStatus);

		modelBuilder.Entity<AuditLog>()
		.Property(b => b.IsChanged)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<AuditLog>()
		.Property(b => b.IsSuccess)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<Order>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<Order>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<OrderItem>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<OrderItem>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<Address>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<Address>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<ShippingMethod>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<ShippingMethod>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<OrderNote>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<OrderNote>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<OrderStatusHistory>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<OrderStatusHistory>().HasQueryFilter(r => !r.IsDeleted);


		modelBuilder.Entity<Order>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<OrderItem>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<Address>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<ShippingMethod>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<OrderNote>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<OrderStatusHistory>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");


		modelBuilder.Entity<Order>()
			.HasOne(e => e.BillingAddress)
			.WithMany(p => p.OrderBillingAddress)
			.HasForeignKey(e => e.BillingAddressId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Order>()
			.HasOne(e => e.ShippingAddress)
			.WithMany(p => p.OrderShippingAddress)
			.HasForeignKey(e => e.ShippingAddressId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Order>()
			.HasOne(e => e.ShippingMethod)
			.WithMany(p => p.Orders)
			.HasForeignKey(e => e.ShippingMethodId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<OrderItem>()
			.HasOne(e => e.Order)
			.WithMany(p => p.OrderItems)
			.HasForeignKey(e => e.OrderId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<OrderNote>()
			.HasOne(e => e.Order)
			.WithMany(p => p.OrderNotes)
			.HasForeignKey(e => e.OrderId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<OrderStatusHistory>()
			.HasOne(e => e.Order)
			.WithMany(p => p.OrderStatusHistories)
			.HasForeignKey(e => e.OrderId)
			.OnDelete(DeleteBehavior.Restrict);

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
