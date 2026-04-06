using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_20260406_231451_127 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AUCTION_ITEMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    CONDITION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OWNER_ID = table.Column<int>(type: "int", nullable: false),
                    IS_AVAILABLE = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUCTION_ITEMS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_LOGS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ENTITY_NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ENTITY_ID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ACTION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    USER_ID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    USER_NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    USER_GROUP = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TIMESTAMP = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OLD_VALUES = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    NEW_VALUES = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    CHANGED_COLUMNS = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    IS_CHANGED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CHANGE_SUMMARY = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    IP_ADDRESS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    USER_AGENT = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CORRELATION_ID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IS_SUCCESS = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERROR_MESSAGE = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    HASH = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_LOGS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BID_INCREMENT_RULES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRICE_FROM = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PRICE_TO = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    INCREMENT_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BID_INCREMENT_RULES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AUCTION_SETTLEMENTS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AUCTION_ID = table.Column<int>(type: "int", nullable: false),
                    WINNER_ID = table.Column<int>(type: "int", nullable: false),
                    FINAL_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PAYMENT_ID = table.Column<int>(type: "int", nullable: false),
                    PAYMENT_DUE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IS_PAID = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PAID_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUCTION_SETTLEMENTS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AUCTION_WATCHLISTS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AUCTION_ID = table.Column<int>(type: "int", nullable: false),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUCTION_WATCHLISTS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AUCTIONS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ITEM_ID = table.Column<int>(type: "int", nullable: false),
                    ITEM_NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ITEM_DESCRIPTION = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SELLER_ID = table.Column<int>(type: "int", nullable: false),
                    START_PRICE = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RESERVE_PRICE = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CURRENT_PRICE = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    WINNING_BID_ID = table.Column<int>(type: "int", nullable: false),
                    WINNER_ID = table.Column<int>(type: "int", nullable: false),
                    START_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    END_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(250)", nullable: false, defaultValueSql: "'DRAFT'"),
                    CREATED_DATE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUCTIONS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AUCTIONS_AUCTION_ITEMS_ITEM_ID",
                        column: x => x.ITEM_ID,
                        principalTable: "AUCTION_ITEMS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BIDS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AUCTION_ID = table.Column<int>(type: "int", nullable: false),
                    BIDDER_ID = table.Column<int>(type: "int", nullable: false),
                    BID_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BID_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(250)", nullable: false, defaultValueSql: "'ACTIVE'"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIDS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BIDS_AUCTIONS_AUCTION_ID",
                        column: x => x.AUCTION_ID,
                        principalTable: "AUCTIONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AUCTION_ITEMS_IsDeleted",
                table: "AUCTION_ITEMS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AUCTION_SETTLEMENTS_AUCTION_ID",
                table: "AUCTION_SETTLEMENTS",
                column: "AUCTION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AUCTION_SETTLEMENTS_IsDeleted",
                table: "AUCTION_SETTLEMENTS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AUCTION_WATCHLISTS_AUCTION_ID",
                table: "AUCTION_WATCHLISTS",
                column: "AUCTION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AUCTION_WATCHLISTS_IsDeleted",
                table: "AUCTION_WATCHLISTS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AUCTIONS_IsDeleted",
                table: "AUCTIONS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AUCTIONS_ITEM_ID",
                table: "AUCTIONS",
                column: "ITEM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AUCTIONS_WINNING_BID_ID",
                table: "AUCTIONS",
                column: "WINNING_BID_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BID_INCREMENT_RULES_IsDeleted",
                table: "BID_INCREMENT_RULES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BIDS_AUCTION_ID",
                table: "BIDS",
                column: "AUCTION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BIDS_IsDeleted",
                table: "BIDS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_AUCTION_SETTLEMENTS_AUCTIONS_AUCTION_ID",
                table: "AUCTION_SETTLEMENTS",
                column: "AUCTION_ID",
                principalTable: "AUCTIONS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AUCTION_WATCHLISTS_AUCTIONS_AUCTION_ID",
                table: "AUCTION_WATCHLISTS",
                column: "AUCTION_ID",
                principalTable: "AUCTIONS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AUCTIONS_BIDS_WINNING_BID_ID",
                table: "AUCTIONS",
                column: "WINNING_BID_ID",
                principalTable: "BIDS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BIDS_AUCTIONS_AUCTION_ID",
                table: "BIDS");

            migrationBuilder.DropTable(
                name: "AUCTION_SETTLEMENTS");

            migrationBuilder.DropTable(
                name: "AUCTION_WATCHLISTS");

            migrationBuilder.DropTable(
                name: "AUDIT_LOGS");

            migrationBuilder.DropTable(
                name: "BID_INCREMENT_RULES");

            migrationBuilder.DropTable(
                name: "AUCTIONS");

            migrationBuilder.DropTable(
                name: "AUCTION_ITEMS");

            migrationBuilder.DropTable(
                name: "BIDS");
        }
    }
}
