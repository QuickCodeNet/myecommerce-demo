using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_20260406_231451_127 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
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
                name: "ColumnTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IosComponentName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IosType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IconCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColumnTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroups",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroups", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "PortalMenus",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Tooltip = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    ParentName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortalMenus", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "TableComboboxSettings",
                columns: table => new
                {
                    TableName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IdColumn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TextColumns = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    StringFormat = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableComboboxSettings", x => x.TableName);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => new { x.Name, x.ModuleName });
                    table.ForeignKey(
                        name: "FK_Models_Modules_ModuleName",
                        column: x => x.ModuleName,
                        principalTable: "Modules",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PermissionGroupName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_PermissionGroups_PermissionGroupName",
                        column: x => x.PermissionGroupName,
                        principalTable: "PermissionGroups",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApiMethodDefinitions",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModelName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HttpMethod = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    ControllerName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MethodName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UrlPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiMethodDefinitions", x => x.Key);
                    table.ForeignKey(
                        name: "FK_ApiMethodDefinitions_Models_ModelName_ModuleName",
                        columns: x => new { x.ModelName, x.ModuleName },
                        principalTable: "Models",
                        principalColumns: new[] { "Name", "ModuleName" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApiMethodDefinitions_Modules_ModuleName",
                        column: x => x.ModuleName,
                        principalTable: "Modules",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PortalPageDefinitions",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModelName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PageAction = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    PagePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortalPageDefinitions", x => x.Key);
                    table.ForeignKey(
                        name: "FK_PortalPageDefinitions_Models_ModelName_ModuleName",
                        columns: x => new { x.ModelName, x.ModuleName },
                        principalTable: "Models",
                        principalColumns: new[] { "Name", "ModuleName" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PortalPageDefinitions_Modules_ModuleName",
                        column: x => x.ModuleName,
                        principalTable: "Modules",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApiMethodAccessGrants",
                columns: table => new
                {
                    PermissionGroupName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ApiMethodDefinitionKey = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(250)", nullable: false, defaultValueSql: "'System'"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiMethodAccessGrants", x => new { x.PermissionGroupName, x.ApiMethodDefinitionKey });
                    table.ForeignKey(
                        name: "FK_ApiMethodAccessGrants_ApiMethodDefinitions_ApiMethodDefinitionKey",
                        column: x => x.ApiMethodDefinitionKey,
                        principalTable: "ApiMethodDefinitions",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApiMethodAccessGrants_PermissionGroups_PermissionGroupName",
                        column: x => x.PermissionGroupName,
                        principalTable: "PermissionGroups",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KafkaEvents",
                columns: table => new
                {
                    TopicName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ApiMethodDefinitionKey = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KafkaEvents", x => x.TopicName);
                    table.ForeignKey(
                        name: "FK_KafkaEvents_ApiMethodDefinitions_ApiMethodDefinitionKey",
                        column: x => x.ApiMethodDefinitionKey,
                        principalTable: "ApiMethodDefinitions",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PortalPageAccessGrants",
                columns: table => new
                {
                    PermissionGroupName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PortalPageDefinitionKey = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PageAction = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(250)", nullable: false, defaultValueSql: "'System'"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortalPageAccessGrants", x => new { x.PermissionGroupName, x.PortalPageDefinitionKey, x.PageAction });
                    table.ForeignKey(
                        name: "FK_PortalPageAccessGrants_PermissionGroups_PermissionGroupName",
                        column: x => x.PermissionGroupName,
                        principalTable: "PermissionGroups",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PortalPageAccessGrants_PortalPageDefinitions_PortalPageDefinitionKey",
                        column: x => x.PortalPageDefinitionKey,
                        principalTable: "PortalPageDefinitions",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopicWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KafkaEventsTopicName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WorkflowContent = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicWorkflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicWorkflows_KafkaEvents_KafkaEventsTopicName",
                        column: x => x.KafkaEventsTopicName,
                        principalTable: "KafkaEvents",
                        principalColumn: "TopicName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiMethodAccessGrants_ApiMethodDefinitionKey",
                table: "ApiMethodAccessGrants",
                column: "ApiMethodDefinitionKey");

            migrationBuilder.CreateIndex(
                name: "IX_ApiMethodDefinitions_ModelName_ModuleName",
                table: "ApiMethodDefinitions",
                columns: new[] { "ModelName", "ModuleName" });

            migrationBuilder.CreateIndex(
                name: "IX_ApiMethodDefinitions_ModuleName",
                table: "ApiMethodDefinitions",
                column: "ModuleName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PermissionGroupName",
                table: "AspNetUsers",
                column: "PermissionGroupName");

            migrationBuilder.CreateIndex(
                name: "IX_ColumnTypes_IsDeleted",
                table: "ColumnTypes",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_KafkaEvents_ApiMethodDefinitionKey",
                table: "KafkaEvents",
                column: "ApiMethodDefinitionKey");

            migrationBuilder.CreateIndex(
                name: "IX_Models_ModuleName",
                table: "Models",
                column: "ModuleName");

            migrationBuilder.CreateIndex(
                name: "IX_PortalPageAccessGrants_PortalPageDefinitionKey",
                table: "PortalPageAccessGrants",
                column: "PortalPageDefinitionKey");

            migrationBuilder.CreateIndex(
                name: "IX_PortalPageDefinitions_ModelName_ModuleName",
                table: "PortalPageDefinitions",
                columns: new[] { "ModelName", "ModuleName" });

            migrationBuilder.CreateIndex(
                name: "IX_PortalPageDefinitions_ModuleName",
                table: "PortalPageDefinitions",
                column: "ModuleName");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_IsDeleted",
                table: "RefreshTokens",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicWorkflows_IsDeleted",
                table: "TopicWorkflows",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TopicWorkflows_KafkaEventsTopicName",
                table: "TopicWorkflows",
                column: "KafkaEventsTopicName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiMethodAccessGrants");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AUDIT_LOGS");

            migrationBuilder.DropTable(
                name: "ColumnTypes");

            migrationBuilder.DropTable(
                name: "PortalMenus");

            migrationBuilder.DropTable(
                name: "PortalPageAccessGrants");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "TableComboboxSettings");

            migrationBuilder.DropTable(
                name: "TopicWorkflows");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "PortalPageDefinitions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "KafkaEvents");

            migrationBuilder.DropTable(
                name: "PermissionGroups");

            migrationBuilder.DropTable(
                name: "ApiMethodDefinitions");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
