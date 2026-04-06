using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_20260406_231451_127 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "BRANDS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BRANDS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CATEGORIES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PARENT_CATEGORY_ID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SLUG = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CATEGORIES_CATEGORIES_PARENT_CATEGORY_ID",
                        column: x => x.PARENT_CATEGORY_ID,
                        principalTable: "CATEGORIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_ATTRIBUTES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_ATTRIBUTES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CATEGORY_ID = table.Column<int>(type: "int", nullable: false),
                    BRAND_ID = table.Column<int>(type: "int", nullable: false),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValueSql: "0"),
                    STOCK_QUANTITY = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    STATUS = table.Column<string>(type: "nvarchar(250)", nullable: false, defaultValueSql: "'DRAFT'"),
                    IS_FEATURED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUCTS_BRANDS_BRAND_ID",
                        column: x => x.BRAND_ID,
                        principalTable: "BRANDS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCTS_CATEGORIES_CATEGORY_ID",
                        column: x => x.CATEGORY_ID,
                        principalTable: "CATEGORIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_ATTRIBUTE_VALUES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    ATTRIBUTE_ID = table.Column<int>(type: "int", nullable: false),
                    VALUE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_ATTRIBUTE_VALUES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUCT_ATTRIBUTE_VALUES_PRODUCTS_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCT_ATTRIBUTE_VALUES_PRODUCT_ATTRIBUTES_ATTRIBUTE_ID",
                        column: x => x.ATTRIBUTE_ID,
                        principalTable: "PRODUCT_ATTRIBUTES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_REVIEWS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    CUSTOMER_ID = table.Column<int>(type: "int", nullable: false),
                    RATING = table.Column<short>(type: "smallint", nullable: false),
                    TITLE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    COMMENT = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IS_APPROVED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_REVIEWS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUCT_REVIEWS_PRODUCTS_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BRANDS_IsDeleted",
                table: "BRANDS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORIES_IsDeleted",
                table: "CATEGORIES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORIES_PARENT_CATEGORY_ID",
                table: "CATEGORIES",
                column: "PARENT_CATEGORY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_ATTRIBUTE_VALUES_ATTRIBUTE_ID",
                table: "PRODUCT_ATTRIBUTE_VALUES",
                column: "ATTRIBUTE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_ATTRIBUTE_VALUES_IsDeleted",
                table: "PRODUCT_ATTRIBUTE_VALUES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_ATTRIBUTE_VALUES_PRODUCT_ID",
                table: "PRODUCT_ATTRIBUTE_VALUES",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_ATTRIBUTES_IsDeleted",
                table: "PRODUCT_ATTRIBUTES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_REVIEWS_IsDeleted",
                table: "PRODUCT_REVIEWS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_REVIEWS_PRODUCT_ID",
                table: "PRODUCT_REVIEWS",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_BRAND_ID",
                table: "PRODUCTS",
                column: "BRAND_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_CATEGORY_ID",
                table: "PRODUCTS",
                column: "CATEGORY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_IsDeleted",
                table: "PRODUCTS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDIT_LOGS");

            migrationBuilder.DropTable(
                name: "PRODUCT_ATTRIBUTE_VALUES");

            migrationBuilder.DropTable(
                name: "PRODUCT_REVIEWS");

            migrationBuilder.DropTable(
                name: "PRODUCT_ATTRIBUTES");

            migrationBuilder.DropTable(
                name: "PRODUCTS");

            migrationBuilder.DropTable(
                name: "BRANDS");

            migrationBuilder.DropTable(
                name: "CATEGORIES");
        }
    }
}
