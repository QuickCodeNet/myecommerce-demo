using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_20260406_231451_127 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADDRESSES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CUSTOMER_ID = table.Column<int>(type: "int", nullable: false),
                    ADDRESS_LINE_1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ADDRESS_LINE_2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CITY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    STATE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    POSTAL_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    COUNTRY_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FULL_ADDRESS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IS_DEFAULT_SHIPPING = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IS_DEFAULT_BILLING = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADDRESSES", x => x.ID);
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
                name: "SHIPPING_METHODS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    COST = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHIPPING_METHODS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ORDERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORDER_NUMBER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CUSTOMER_ID = table.Column<int>(type: "int", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(250)", nullable: false, defaultValueSql: "'PENDING'"),
                    TOTAL_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SHIPPING_ADDRESS_ID = table.Column<int>(type: "int", nullable: false),
                    BILLING_ADDRESS_ID = table.Column<int>(type: "int", nullable: false),
                    SHIPPING_METHOD_ID = table.Column<int>(type: "int", nullable: false),
                    PAYMENT_ID = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDERS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDERS_ADDRESSES_BILLING_ADDRESS_ID",
                        column: x => x.BILLING_ADDRESS_ID,
                        principalTable: "ADDRESSES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ORDERS_ADDRESSES_SHIPPING_ADDRESS_ID",
                        column: x => x.SHIPPING_ADDRESS_ID,
                        principalTable: "ADDRESSES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ORDERS_SHIPPING_METHODS_SHIPPING_METHOD_ID",
                        column: x => x.SHIPPING_METHOD_ID,
                        principalTable: "SHIPPING_METHODS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ORDER_ITEMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORDER_ID = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QUANTITY = table.Column<int>(type: "int", nullable: false),
                    UNIT_PRICE = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_ITEMS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDER_ITEMS_ORDERS_ORDER_ID",
                        column: x => x.ORDER_ID,
                        principalTable: "ORDERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ORDER_NOTES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORDER_ID = table.Column<int>(type: "int", nullable: false),
                    NOTE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IS_CUSTOMER_VISIBLE = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CREATED_BY_USER_ID = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_NOTES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDER_NOTES_ORDERS_ORDER_ID",
                        column: x => x.ORDER_ID,
                        principalTable: "ORDERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ORDER_STATUS_HISTORIES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORDER_ID = table.Column<int>(type: "int", nullable: false),
                    PREVIOUS_STATUS = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    NEW_STATUS = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    REASON = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CHANGED_BY_USER_ID = table.Column<int>(type: "int", nullable: false),
                    CHANGED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_STATUS_HISTORIES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDER_STATUS_HISTORIES_ORDERS_ORDER_ID",
                        column: x => x.ORDER_ID,
                        principalTable: "ORDERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADDRESSES_IsDeleted",
                table: "ADDRESSES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_ITEMS_IsDeleted",
                table: "ORDER_ITEMS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_ITEMS_ORDER_ID",
                table: "ORDER_ITEMS",
                column: "ORDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_NOTES_IsDeleted",
                table: "ORDER_NOTES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_NOTES_ORDER_ID",
                table: "ORDER_NOTES",
                column: "ORDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_STATUS_HISTORIES_IsDeleted",
                table: "ORDER_STATUS_HISTORIES",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_STATUS_HISTORIES_ORDER_ID",
                table: "ORDER_STATUS_HISTORIES",
                column: "ORDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDERS_BILLING_ADDRESS_ID",
                table: "ORDERS",
                column: "BILLING_ADDRESS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDERS_IsDeleted",
                table: "ORDERS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ORDERS_SHIPPING_ADDRESS_ID",
                table: "ORDERS",
                column: "SHIPPING_ADDRESS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDERS_SHIPPING_METHOD_ID",
                table: "ORDERS",
                column: "SHIPPING_METHOD_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPPING_METHODS_IsDeleted",
                table: "SHIPPING_METHODS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDIT_LOGS");

            migrationBuilder.DropTable(
                name: "ORDER_ITEMS");

            migrationBuilder.DropTable(
                name: "ORDER_NOTES");

            migrationBuilder.DropTable(
                name: "ORDER_STATUS_HISTORIES");

            migrationBuilder.DropTable(
                name: "ORDERS");

            migrationBuilder.DropTable(
                name: "ADDRESSES");

            migrationBuilder.DropTable(
                name: "SHIPPING_METHODS");
        }
    }
}
