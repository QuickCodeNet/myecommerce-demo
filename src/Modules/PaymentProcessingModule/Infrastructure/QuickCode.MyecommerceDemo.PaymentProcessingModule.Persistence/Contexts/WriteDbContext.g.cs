using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Contexts;

public partial class WriteDbContext : DbContext
{
	public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options) { }


	public virtual DbSet<Payment> Payment { get; set; }

	public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }

	public virtual DbSet<PaymentGateway> PaymentGateway { get; set; }

	public virtual DbSet<GatewayConfig> GatewayConfig { get; set; }

	public virtual DbSet<Refund> Refund { get; set; }

	public virtual DbSet<TransactionLog> TransactionLog { get; set; }

	public virtual DbSet<AuditLog> AuditLog { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Payment>()
		.Property(b => b.CurrencyCode)
		.IsRequired()
		.HasDefaultValueSql("'USD'");

		modelBuilder.Entity<Payment>()
		.Property(b => b.Status)
		.IsRequired()
		.HasDefaultValueSql("'PENDING'");


		var converterPaymentStatus = new ValueConverter<PaymentStatus, string>(
		v => v.ToString(),
		v => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), v));

		modelBuilder.Entity<Payment>()
		.Property(b => b.Status)
		.HasConversion(converterPaymentStatus);

		modelBuilder.Entity<PaymentMethod>()
		.Property(b => b.IsDefault)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<PaymentMethod>()
		.Property(b => b.IsActive)
		.IsRequired()
		.HasDefaultValue(true);


		var converterPaymentMethodMethodType = new ValueConverter<PaymentMethodType, string>(
		v => v.ToString(),
		v => (PaymentMethodType)Enum.Parse(typeof(PaymentMethodType), v));

		modelBuilder.Entity<PaymentMethod>()
		.Property(b => b.MethodType)
		.HasConversion(converterPaymentMethodMethodType);

		modelBuilder.Entity<PaymentGateway>()
		.Property(b => b.IsActive)
		.IsRequired()
		.HasDefaultValue(true);

		modelBuilder.Entity<GatewayConfig>()
		.Property(b => b.IsSecret)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<Refund>()
		.Property(b => b.Status)
		.IsRequired()
		.HasDefaultValueSql("'REQUESTED'");


		var converterRefundStatus = new ValueConverter<RefundStatus, string>(
		v => v.ToString(),
		v => (RefundStatus)Enum.Parse(typeof(RefundStatus), v));

		modelBuilder.Entity<Refund>()
		.Property(b => b.Status)
		.HasConversion(converterRefundStatus);

		modelBuilder.Entity<AuditLog>()
		.Property(b => b.IsChanged)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<AuditLog>()
		.Property(b => b.IsSuccess)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<Payment>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<Payment>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<PaymentMethod>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<PaymentMethod>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<PaymentGateway>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<PaymentGateway>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<GatewayConfig>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<GatewayConfig>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<Refund>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<Refund>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<TransactionLog>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<TransactionLog>().HasQueryFilter(r => !r.IsDeleted);


		modelBuilder.Entity<Payment>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<PaymentMethod>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<PaymentGateway>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<GatewayConfig>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<Refund>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<TransactionLog>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");


		modelBuilder.Entity<Payment>()
			.HasOne(e => e.PaymentGateway)
			.WithMany(p => p.Payments)
			.HasForeignKey(e => e.PaymentGatewayId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<GatewayConfig>()
			.HasOne(e => e.PaymentGateway)
			.WithMany(p => p.GatewayConfigs)
			.HasForeignKey(e => e.GatewayId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Refund>()
			.HasOne(e => e.Payment)
			.WithMany(p => p.Refunds)
			.HasForeignKey(e => e.PaymentId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<TransactionLog>()
			.HasOne(e => e.Payment)
			.WithMany(p => p.TransactionLogs)
			.HasForeignKey(e => e.PaymentId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<TransactionLog>()
			.HasOne(e => e.PaymentGateway)
			.WithMany(p => p.TransactionLogs)
			.HasForeignKey(e => e.GatewayId)
			.OnDelete(DeleteBehavior.Restrict);

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
