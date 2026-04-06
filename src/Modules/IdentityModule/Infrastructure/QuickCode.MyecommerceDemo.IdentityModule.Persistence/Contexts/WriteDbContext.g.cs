using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Entities;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Contexts;

public partial class WriteDbContext : DbContext
{
	public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options) { }


	public virtual DbSet<Module> Module { get; set; }

	public virtual DbSet<Model> Model { get; set; }

	public virtual DbSet<PermissionGroup> PermissionGroup { get; set; }

	public virtual DbSet<ApiMethodDefinition> ApiMethodDefinition { get; set; }

	public virtual DbSet<PortalPageDefinition> PortalPageDefinition { get; set; }

	public virtual DbSet<ApiMethodAccessGrant> ApiMethodAccessGrant { get; set; }

	public virtual DbSet<PortalPageAccessGrant> PortalPageAccessGrant { get; set; }

	public virtual DbSet<PortalMenu> PortalMenu { get; set; }

	public virtual DbSet<ColumnType> ColumnType { get; set; }

	public virtual DbSet<TableComboboxSetting> TableComboboxSetting { get; set; }

	public virtual DbSet<KafkaEvent> KafkaEvent { get; set; }

	public virtual DbSet<TopicWorkflow> TopicWorkflow { get; set; }

	public virtual DbSet<AspNetRole> AspNetRole { get; set; }

	public virtual DbSet<AspNetUser> AspNetUser { get; set; }

	public virtual DbSet<RefreshToken> RefreshToken { get; set; }

	public virtual DbSet<AspNetRoleClaim> AspNetRoleClaim { get; set; }

	public virtual DbSet<AspNetUserClaim> AspNetUserClaim { get; set; }

	public virtual DbSet<AspNetUserLogin> AspNetUserLogin { get; set; }

	public virtual DbSet<AspNetUserRole> AspNetUserRole { get; set; }

	public virtual DbSet<AspNetUserToken> AspNetUserToken { get; set; }

	public virtual DbSet<AuditLog> AuditLog { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

		var converterApiMethodDefinitionHttpMethod = new ValueConverter<HttpMethodType, string>(
		v => v.ToString(),
		v => (HttpMethodType)Enum.Parse(typeof(HttpMethodType), v));

		modelBuilder.Entity<ApiMethodDefinition>()
		.Property(b => b.HttpMethod)
		.HasConversion(converterApiMethodDefinitionHttpMethod);


		var converterPortalPageDefinitionPageAction = new ValueConverter<PageActionType, string>(
		v => v.ToString(),
		v => (PageActionType)Enum.Parse(typeof(PageActionType), v));

		modelBuilder.Entity<PortalPageDefinition>()
		.Property(b => b.PageAction)
		.HasConversion(converterPortalPageDefinitionPageAction);

		modelBuilder.Entity<ApiMethodAccessGrant>()
		.Property(b => b.ModifiedBy)
		.IsRequired()
		.HasDefaultValueSql("'System'");

		modelBuilder.Entity<ApiMethodAccessGrant>()
		.Property(b => b.IsActive)
		.IsRequired()
		.HasDefaultValue(false);


		var converterApiMethodAccessGrantModifiedBy = new ValueConverter<ModificationType, string>(
		v => v.ToString(),
		v => (ModificationType)Enum.Parse(typeof(ModificationType), v));

		modelBuilder.Entity<ApiMethodAccessGrant>()
		.Property(b => b.ModifiedBy)
		.HasConversion(converterApiMethodAccessGrantModifiedBy);

		modelBuilder.Entity<PortalPageAccessGrant>()
		.Property(b => b.ModifiedBy)
		.IsRequired()
		.HasDefaultValueSql("'System'");

		modelBuilder.Entity<PortalPageAccessGrant>()
		.Property(b => b.IsActive)
		.IsRequired()
		.HasDefaultValue(false);


		var converterPortalPageAccessGrantPageAction = new ValueConverter<PageActionType, string>(
		v => v.ToString(),
		v => (PageActionType)Enum.Parse(typeof(PageActionType), v));

		modelBuilder.Entity<PortalPageAccessGrant>()
		.Property(b => b.PageAction)
		.HasConversion(converterPortalPageAccessGrantPageAction);


		var converterPortalPageAccessGrantModifiedBy = new ValueConverter<ModificationType, string>(
		v => v.ToString(),
		v => (ModificationType)Enum.Parse(typeof(ModificationType), v));

		modelBuilder.Entity<PortalPageAccessGrant>()
		.Property(b => b.ModifiedBy)
		.HasConversion(converterPortalPageAccessGrantModifiedBy);

		modelBuilder.Entity<KafkaEvent>()
		.Property(b => b.IsActive)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<RefreshToken>()
		.Property(b => b.CreatedDate)
		.IsRequired()
		.HasColumnType("datetime")
		.HasDefaultValueSql("getdate()");

		modelBuilder.Entity<RefreshToken>()
		.Property(b => b.IsRevoked)
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

		modelBuilder.Entity<ColumnType>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<ColumnType>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<TopicWorkflow>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<TopicWorkflow>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<RefreshToken>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<RefreshToken>().HasQueryFilter(r => !r.IsDeleted);


		modelBuilder.Entity<ColumnType>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<TopicWorkflow>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<RefreshToken>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");


		modelBuilder.Entity<Model>()
			.HasOne(e => e.Module)
			.WithMany(p => p.Models)
			.HasForeignKey(e => e.ModuleName)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<ApiMethodDefinition>()
			.HasOne(e => e.Module)
			.WithMany(p => p.ApiMethodDefinitions)
			.HasForeignKey(e => e.ModuleName)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<ApiMethodDefinition>()
			.HasOne(e => e.Model)
			.WithMany(p => p.ApiMethodDefinitions)
			.HasForeignKey(e => new { e.ModelName, e.ModuleName })
			.HasPrincipalKey(p => new { p.Name, p.ModuleName })
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<PortalPageDefinition>()
			.HasOne(e => e.Module)
			.WithMany(p => p.PortalPageDefinitions)
			.HasForeignKey(e => e.ModuleName)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<PortalPageDefinition>()
			.HasOne(e => e.Model)
			.WithMany(p => p.PortalPageDefinitions)
			.HasForeignKey(e => new { e.ModelName, e.ModuleName })
			.HasPrincipalKey(p => new { p.Name, p.ModuleName })
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<ApiMethodAccessGrant>()
			.HasOne(e => e.ApiMethodDefinition)
			.WithMany(p => p.ApiMethodAccessGrants)
			.HasForeignKey(e => e.ApiMethodDefinitionKey)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<ApiMethodAccessGrant>()
			.HasOne(e => e.PermissionGroup)
			.WithMany(p => p.ApiMethodAccessGrants)
			.HasForeignKey(e => e.PermissionGroupName)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<PortalPageAccessGrant>()
			.HasOne(e => e.PortalPageDefinition)
			.WithMany(p => p.PortalPageAccessGrants)
			.HasForeignKey(e => e.PortalPageDefinitionKey)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<PortalPageAccessGrant>()
			.HasOne(e => e.PermissionGroup)
			.WithMany(p => p.PortalPageAccessGrants)
			.HasForeignKey(e => e.PermissionGroupName)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<KafkaEvent>()
			.HasOne(e => e.ApiMethodDefinition)
			.WithMany(p => p.KafkaEvents)
			.HasForeignKey(e => e.ApiMethodDefinitionKey)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<TopicWorkflow>()
			.HasOne(e => e.KafkaEvent)
			.WithMany(p => p.TopicWorkflows)
			.HasForeignKey(e => e.KafkaEventsTopicName)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<AspNetUser>()
			.HasOne(e => e.PermissionGroup)
			.WithMany(p => p.AspNetUsers)
			.HasForeignKey(e => e.PermissionGroupName)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<RefreshToken>()
			.HasOne(e => e.AspNetUser)
			.WithMany(p => p.RefreshTokens)
			.HasForeignKey(e => e.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<AspNetRoleClaim>()
			.HasOne(e => e.AspNetRole)
			.WithMany(p => p.AspNetRoleClaims)
			.HasForeignKey(e => e.RoleId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<AspNetUserClaim>()
			.HasOne(e => e.AspNetUser)
			.WithMany(p => p.AspNetUserClaims)
			.HasForeignKey(e => e.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<AspNetUserLogin>()
			.HasOne(e => e.AspNetUser)
			.WithMany(p => p.AspNetUserLogins)
			.HasForeignKey(e => e.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<AspNetUserRole>()
			.HasOne(e => e.AspNetRole)
			.WithMany(p => p.AspNetUserRoles)
			.HasForeignKey(e => e.RoleId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<AspNetUserRole>()
			.HasOne(e => e.AspNetUser)
			.WithMany(p => p.AspNetUserRoles)
			.HasForeignKey(e => e.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<AspNetUserToken>()
			.HasOne(e => e.AspNetUser)
			.WithMany(p => p.AspNetUserTokens)
			.HasForeignKey(e => e.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
