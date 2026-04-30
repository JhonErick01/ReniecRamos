using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReniecRamos.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CivilStatuses",
                columns: table => new
                {
                    CivilStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CivilStatuses", x => x.CivilStatusId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.DocumentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Ubigeos",
                columns: table => new
                {
                    UbigeoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubigeos", x => x.UbigeoId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Citizens",
                columns: table => new
                {
                    CitizenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CivilStatusId = table.Column<int>(type: "int", nullable: false),
                    BirthUbigeoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentUbigeoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizens", x => x.CitizenId);
                    table.ForeignKey(
                        name: "FK_Citizens_CivilStatuses_CivilStatusId",
                        column: x => x.CivilStatusId,
                        principalTable: "CivilStatuses",
                        principalColumn: "CivilStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Citizens_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Citizens_Ubigeos_BirthUbigeoId",
                        column: x => x.BirthUbigeoId,
                        principalTable: "Ubigeos",
                        principalColumn: "UbigeoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citizens_Ubigeos_CurrentUbigeoId",
                        column: x => x.CurrentUbigeoId,
                        principalTable: "Ubigeos",
                        principalColumn: "UbigeoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Procedures",
                columns: table => new
                {
                    ProcedureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenId = table.Column<int>(type: "int", nullable: false),
                    ProcedureType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => x.ProcedureId);
                    table.ForeignKey(
                        name: "FK_Procedures_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CivilStatuses",
                columns: new[] { "CivilStatusId", "Description" },
                values: new object[,]
                {
                    { 1, "Soltero(a)" },
                    { 2, "Casado(a)" },
                    { 3, "Divorciado(a)" },
                    { 4, "Viudo(a)" }
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "DocumentTypeId", "Abbreviation", "Description" },
                values: new object[,]
                {
                    { 1, "DNI", "Documento Nacional de Identidad" },
                    { 2, "PAS", "Pasaporte" },
                    { 3, "CE", "Carnet de Extranjería" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Name" },
                values: new object[,]
                {
                    { 1, "Administrator" },
                    { 2, "Assistant" }
                });

            migrationBuilder.InsertData(
                table: "Ubigeos",
                columns: new[] { "UbigeoId", "Department", "District", "Province" },
                values: new object[,]
                {
                    { "040101", "Arequipa", "Arequipa", "Arequipa" },
                    { "070101", "Callao", "Callao", "Callao" },
                    { "070106", "Callao", "Ventanilla", "Callao" },
                    { "080101", "Cusco", "Cusco", "Cusco" },
                    { "130101", "La Libertad", "Trujillo", "Trujillo" },
                    { "140101", "Lambayeque", "Chiclayo", "Chiclayo" },
                    { "150101", "Lima", "Lima", "Lima" },
                    { "150122", "Lima", "Miraflores", "Lima" },
                    { "150142", "Lima", "Surquillo", "Lima" },
                    { "190101", "Pasco", "Chaupimarca", "Pasco" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "FullName", "IsActive", "PasswordHash", "RoleId", "Username" },
                values: new object[,]
                {
                    { 1, "Jhon Administrador", true, new byte[] { 36, 50, 97, 36, 49, 49, 36, 109, 57, 78, 57, 56, 88, 122, 89, 57, 46, 75, 115, 116, 72, 108, 71, 46, 71, 56, 118, 55, 117, 56, 86, 118, 53, 90, 112, 54, 77, 56, 76, 112, 55, 83, 54, 82, 53, 81, 52, 80, 51, 79, 50, 78, 49, 77, 48, 76, 57, 75, 56, 74 }, 1, "jhonAd" },
                    { 2, "Jhon Asistente", true, new byte[] { 36, 50, 97, 36, 49, 49, 36, 109, 57, 78, 57, 56, 88, 122, 89, 57, 46, 75, 115, 116, 72, 108, 71, 46, 71, 56, 118, 55, 117, 56, 86, 118, 53, 90, 112, 54, 77, 56, 76, 112, 55, 83, 54, 82, 53, 81, 52, 80, 51, 79, 50, 78, 49, 77, 48, 76, 57, 75, 56, 74 }, 2, "jhonAsis" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_BirthUbigeoId",
                table: "Citizens",
                column: "BirthUbigeoId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_CivilStatusId",
                table: "Citizens",
                column: "CivilStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_CurrentUbigeoId",
                table: "Citizens",
                column: "CurrentUbigeoId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_DocumentTypeId",
                table: "Citizens",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_CitizenId",
                table: "Procedures",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Procedures");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Citizens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "CivilStatuses");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Ubigeos");
        }
    }
}
