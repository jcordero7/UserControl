using System;
//using Microsoft.EntityFrameworkCore.Migrations;
//using MySql.Data.EntityFrameworkCore.Metadata;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserControl.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoricalTable",
                columns: table => new
                {
                    TableId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 40, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricalTable", x => x.TableId);
                });

            migrationBuilder.CreateTable(
                name: "Program",
                columns: table => new
                {
                    ProgramId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program", x => x.ProgramId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 15, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Securities",
                columns: table => new
                {
                    SecurityId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    User = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 200, nullable: false),
                    Role = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Securities", x => x.SecurityId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    key = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(maxLength: 30, nullable: false),
                    Password = table.Column<string>(maxLength: 200, nullable: false),
                    Names = table.Column<string>(maxLength: 50, nullable: false),
                    SurNames = table.Column<string>(maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(maxLength: 10, nullable: true),
                    State = table.Column<string>(maxLength: 15, nullable: false),
                    Token = table.Column<string>(nullable: true),
                    Attempts = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RoleXProgram",
                columns: table => new
                {
                    ProgramId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleXProgram", x => new { x.ProgramId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RoleXProgram_Program_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Program",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleXProgram_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Historical",
                columns: table => new
                {
                    HistoricalId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TableId = table.Column<int>(nullable: false),
                    OriginId = table.Column<int>(nullable: false),
                    Activity = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Observation = table.Column<string>(nullable: false),
                    HistoricalTableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historical", x => x.HistoricalId);
                    table.ForeignKey(
                        name: "FK_Historical_HistoricalTable_HistoricalTableId",
                        column: x => x.HistoricalTableId,
                        principalTable: "HistoricalTable",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Historical_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAccess",
                columns: table => new
                {
                    AccessId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Token = table.Column<string>(maxLength: 300, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    OperatingSystem = table.Column<string>(maxLength: 200, nullable: false),
                    Browser = table.Column<string>(maxLength: 200, nullable: false),
                    City = table.Column<string>(maxLength: 200, nullable: false),
                    State = table.Column<string>(maxLength: 200, nullable: false),
                    RefreshToken = table.Column<string>(maxLength: 200, nullable: false),
                    DateRefresh = table.Column<DateTime>(nullable: false),
                    KeepSession = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccess", x => x.AccessId);
                    table.ForeignKey(
                        name: "FK_UserAccess_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgramXUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ProgramId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramXUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramXUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramXUser_RoleXProgram_ProgramId_RoleId",
                        columns: x => new { x.ProgramId, x.RoleId },
                        principalTable: "RoleXProgram",
                        principalColumns: new[] { "ProgramId", "RoleId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Program",
                columns: new[] { "ProgramId", "Description", "IsActive", "Key", "Name" },
                values: new object[] { 1, "Sistema para anuncio de eventos", true, "Ev", "Eventos" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Administrador" },
                    { 2, true, "Operator" },
                    { 3, true, "Consumer" }
                });

            migrationBuilder.InsertData(
                table: "RoleXProgram",
                columns: new[] { "ProgramId", "RoleId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "RoleXProgram",
                columns: new[] { "ProgramId", "RoleId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "RoleXProgram",
                columns: new[] { "ProgramId", "RoleId" },
                values: new object[] { 1, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_Historical_HistoricalTableId",
                table: "Historical",
                column: "HistoricalTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Historical_UserId",
                table: "Historical",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramXUser_UserId",
                table: "ProgramXUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramXUser_ProgramId_RoleId",
                table: "ProgramXUser",
                columns: new[] { "ProgramId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_RoleXProgram_RoleId",
                table: "RoleXProgram",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UI_UserEmail",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UI_RefreshToken",
                table: "UserAccess",
                column: "RefreshToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UI_Token",
                table: "UserAccess",
                columns: new[] { "UserId", "Token" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historical");

            migrationBuilder.DropTable(
                name: "ProgramXUser");

            migrationBuilder.DropTable(
                name: "Securities");

            migrationBuilder.DropTable(
                name: "UserAccess");

            migrationBuilder.DropTable(
                name: "HistoricalTable");

            migrationBuilder.DropTable(
                name: "RoleXProgram");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Program");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
