using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Contexts;

public partial class ReadDbContext : DbContext
{
	public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }


	public virtual DbSet<Auction> Auction { get; set; }

	public virtual DbSet<Bid> Bid { get; set; }

	public virtual DbSet<AuctionWatchlist> AuctionWatchlist { get; set; }

	public virtual DbSet<AuctionSettlement> AuctionSettlement { get; set; }

	public virtual DbSet<AuctionItem> AuctionItem { get; set; }

	public virtual DbSet<BidIncrementRule> BidIncrementRule { get; set; }

	public virtual DbSet<AuditLog> AuditLog { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Auction>()
		.Property(b => b.Status)
		.IsRequired()
		.HasDefaultValueSql("'DRAFT'");


		var converterAuctionStatus = new ValueConverter<AuctionStatus, string>(
		v => v.ToString(),
		v => (AuctionStatus)Enum.Parse(typeof(AuctionStatus), v));

		modelBuilder.Entity<Auction>()
		.Property(b => b.Status)
		.HasConversion(converterAuctionStatus);

		modelBuilder.Entity<Bid>()
		.Property(b => b.Status)
		.IsRequired()
		.HasDefaultValueSql("'ACTIVE'");


		var converterBidStatus = new ValueConverter<BidStatus, string>(
		v => v.ToString(),
		v => (BidStatus)Enum.Parse(typeof(BidStatus), v));

		modelBuilder.Entity<Bid>()
		.Property(b => b.Status)
		.HasConversion(converterBidStatus);

		modelBuilder.Entity<AuctionSettlement>()
		.Property(b => b.IsPaid)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<AuctionItem>()
		.Property(b => b.IsAvailable)
		.IsRequired()
		.HasDefaultValue(true);

		modelBuilder.Entity<BidIncrementRule>()
		.Property(b => b.IsActive)
		.IsRequired()
		.HasDefaultValue(true);

		modelBuilder.Entity<AuditLog>()
		.Property(b => b.IsChanged)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<AuditLog>()
		.Property(b => b.IsSuccess)
		.IsRequired()
		.HasDefaultValue(false);

		modelBuilder.Entity<Auction>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<Auction>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<Bid>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<Bid>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<AuctionWatchlist>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<AuctionWatchlist>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<AuctionSettlement>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<AuctionSettlement>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<AuctionItem>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<AuctionItem>().HasQueryFilter(r => !r.IsDeleted);

		modelBuilder.Entity<BidIncrementRule>().Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
		modelBuilder.Entity<BidIncrementRule>().HasQueryFilter(r => !r.IsDeleted);


		modelBuilder.Entity<Auction>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<Bid>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<AuctionWatchlist>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<AuctionSettlement>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<AuctionItem>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
		modelBuilder.Entity<BidIncrementRule>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");


		modelBuilder.Entity<Auction>()
			.HasOne(e => e.AuctionItem)
			.WithMany(p => p.Auctions)
			.HasForeignKey(e => e.ItemId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Auction>()
			.HasOne(e => e.Bid)
			.WithMany(p => p.Auctions)
			.HasForeignKey(e => e.WinningBidId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Bid>()
			.HasOne(e => e.Auction)
			.WithMany(p => p.Bids)
			.HasForeignKey(e => e.AuctionId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<AuctionWatchlist>()
			.HasOne(e => e.Auction)
			.WithMany(p => p.AuctionWatchlists)
			.HasForeignKey(e => e.AuctionId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<AuctionSettlement>()
			.HasOne(e => e.Auction)
			.WithMany(p => p.AuctionSettlements)
			.HasForeignKey(e => e.AuctionId)
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
