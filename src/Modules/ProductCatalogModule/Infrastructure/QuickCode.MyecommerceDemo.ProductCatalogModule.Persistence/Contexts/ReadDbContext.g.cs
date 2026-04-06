using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Entities;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Contexts;

public partial class ReadDbContext : DbContext
{
	public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }


	public virtual DbSet<Product> Product { get; set; }

	public virtual DbSet<Category> Category { get; set; }

	public virtual DbSet<Brand> Brand { get; set; }

	public virtual DbSet<ProductAttribute> ProductAttribute { get; set; }

	public virtual DbSet<ProductAttributeValue> ProductAttributeValue { get; set; }

	public virtual DbSet<ProductReview> ProductReview { get; set; }

	public virtual DbSet<AuditLog> AuditLog { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Product>()
		.Property(b => b.Price)
		.IsRequired()
		.HasDefaultValueSql("0");

		modelBuilder.Entity<Product>()
		.Property(b => b.StockQuantity)
		.IsRequired()
		.HasDefaultValueSql("0");

		modelBuilder.Entity<Product>()
		.Property(b => b.Status)
		.IsRequired()
		.HasDefaultValueSql("'DRAFT'");

		modelBuilder.Entity<Product>()
		.Property(b => b.IsFeatured)
		.IsRequired()
		.HasDefaultValue(false);


		var converterProductStatus = new ValueConverter<ProductStatus, string>(
		v => v.ToString(),
		v => (ProductStatus)Enum.Parse(typeof(ProductStatus), v));

		modelBuilder.Entity<Product>()
		.Property(b => b.Status)
		.HasConversion(converterProductStatus);

		modelBuilder.Entity<Category>()
		.Property(b => b.IsActive)
		.IsRequired()
		.HasDefaultValue(true);

		modelBuilder.Entity<Brand>()
		.Property(b => b.IsActive)
		.IsRequired()
		.HasDefaultValue(true);

		modelBuilder.Entity<ProductReview>()
		.Property(b => b.IsApproved)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<AuditLog>()
		.Property(b => b.IsChanged)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<AuditLog>()
		.Property(b => b.IsSuccess)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<Product>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<Product>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<Category>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<Category>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<Brand>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<Brand>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<ProductAttribute>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<ProductAttribute>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<ProductAttributeValue>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<ProductAttributeValue>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<ProductReview>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<ProductReview>().HasQueryFilter(r => !r.IsDeleted);


		modelBuilder.Entity<Product>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<Category>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<Brand>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<ProductAttribute>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<ProductAttributeValue>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<ProductReview>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");


		modelBuilder.Entity<Product>()
			.HasOne(e => e.Category)
			.WithMany(p => p.Products)
			.HasForeignKey(e => e.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Product>()
			.HasOne(e => e.Brand)
			.WithMany(p => p.Products)
			.HasForeignKey(e => e.BrandId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Category>()
			.HasOne(e => e.ParentCategory)
			.WithMany(p => p.CategoryParentCategory)
			.HasForeignKey(e => e.ParentCategoryId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<ProductAttributeValue>()
			.HasOne(e => e.Product)
			.WithMany(p => p.ProductAttributeValues)
			.HasForeignKey(e => e.ProductId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<ProductAttributeValue>()
			.HasOne(e => e.ProductAttribute)
			.WithMany(p => p.ProductAttributeValues)
			.HasForeignKey(e => e.AttributeId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<ProductReview>()
			.HasOne(e => e.Product)
			.WithMany(p => p.ProductReviews)
			.HasForeignKey(e => e.ProductId)
			.OnDelete(DeleteBehavior.Restrict);

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    public override int SaveChanges()
    {
        throw new InvalidOperationException("ReadDbContext is read-only.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("ReadDbContext is read-only.");
    }

}
