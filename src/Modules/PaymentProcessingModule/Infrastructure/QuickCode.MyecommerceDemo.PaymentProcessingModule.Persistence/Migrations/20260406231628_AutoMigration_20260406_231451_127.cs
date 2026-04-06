using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Persistence.Migrations
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
                name: "PAYMENT_GATEWAYS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PROVIDER_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYMENT_GATEWAYS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PAYMENT_METHODS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CUSTOMER_ID = table.Column<int>(type: "int", nullable: false),
                    METHOD_TYPE = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    TOKEN = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CARD_BRAND = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LAST_FOUR_DIGITS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EXPIRATION_DATE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IS_DEFAULT = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYMENT_METHODS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GATEWAY_CONFIGS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GATEWAY_ID = table.Column<int>(type: "int", nullable: false),
                    CONFIG_KEY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CONFIG_VALUE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IS_SECRET = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GATEWAY_CONFIGS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GATEWAY_CONFIGS_PAYMENT_GATEWAYS_GATEWAY_ID",
                        column: x => x.GATEWAY_ID,
                        principalTable: "PAYMENT_GATEWAYS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PAYMENTS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REFERENCE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ORDER_ID = table.Column<int>(type: "int", nullable: false),
                    CUSTOMER_ID = table.Column<int>(type: "int", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CURRENCY_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValueSql: "'USD'"),
                    STATUS = table.Column<string>(type: "nvarchar(250)", nullable: false, defaultValueSql: "'PENDING'"),
                    PAYMENT_GATEWAY_ID = table.Column<int>(type: "int", nullable: false),
                    GATEWAY_TRANSACTION_ID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYMENTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PAYMENTS_PAYMENT_GATEWAYS_PAYMENT_GATEWAY_ID",
                        column: x => x.PAYMENT_GATEWAY_ID,
                        principalTable: "PAYMENT_GATEWAYS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "REFUNDS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PAYMENT_ID = table.Column<int>(type: "int", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    REASON = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(250)", nullable: false, defaultValueSql: "'REQUESTED'"),
                    GATEWAY_REFUND_ID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    REQUESTED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PROCESSED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REFUNDS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_REFUNDS_PAYMENTS_PAYMENT_ID",
                        column: x => x.PAYMENT_ID,
                        principalTable: "PAYMENTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TRANSACTION_LOGS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PAYMENT_ID = table.Column<int>(type: "int", nullable: false),
                    GATEWAY_ID = table.Column<int>(type: "int", nullable: false),
                    REQUEST_PAYLOAD = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    RESPONSE_PAYLOAD = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    LOG_LEVEL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MESSAGE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TIMESTAMP = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSACTION_LOGS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TRANSACTION_LOGS_PAYMENTS_PAYMENT_ID",
                        column: x => x.PAYMENT_ID,
                        principalTable: "PAYMENTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TRANSACTION_LOGS_PAYMENT_GATEWAYS_GATEWAY_ID",
                        column: x => x.GATEWAY_ID,
                        principalTable: "PAYMENT_GATEWAYS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GATEWAY_CONFIGS_GATEWAY_ID",
                table: "GATEWAY_CONFIGS",
                column: "GATEWAY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_GATEWAY_CONFIGS_IsDeleted",
                table: "GATEWAY_CONFIGS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PAYMENT_GATEWAYS_IsDeleted",
                table: "PAYMENT_GATEWAYS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PAYMENT_METHODS_IsDeleted",
                table: "PAYMENT_METHODS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PAYMENTS_IsDeleted",
                table: "PAYMENTS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PAYMENTS_PAYMENT_GATEWAY_ID",
                table: "PAYMENTS",
                column: "PAYMENT_GATEWAY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_REFUNDS_IsDeleted",
                table: "REFUNDS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_REFUNDS_PAYMENT_ID",
                table: "REFUNDS",
                column: "PAYMENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_LOGS_GATEWAY_ID",
                table: "TRANSACTION_LOGS",
                column: "GATEWAY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_LOGS_IsDeleted",
                table: "TRANSACTION_LOGS",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_LOGS_PAYMENT_ID",
                table: "TRANSACTION_LOGS",
                column: "PAYMENT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDIT_LOGS");

            migrationBuilder.DropTable(
                name: "GATEWAY_CONFIGS");

            migrationBuilder.DropTable(
                name: "PAYMENT_METHODS");

            migrationBuilder.DropTable(
                name: "REFUNDS");

            migrationBuilder.DropTable(
                name: "TRANSACTION_LOGS");

            migrationBuilder.DropTable(
                name: "PAYMENTS");

            migrationBuilder.DropTable(
                name: "PAYMENT_GATEWAYS");
        }
    }
}
