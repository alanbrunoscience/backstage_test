using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    public partial class corrigido_nome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisterMargin");

            migrationBuilder.CreateTable(
                name: "RegisterNotMargin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enrollment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeProduct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CovenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterNotMargin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisterNotMargin_Covenants_CovenantId",
                        column: x => x.CovenantId,
                        principalTable: "Covenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisterNotMargin_CovenantId",
                table: "RegisterNotMargin",
                column: "CovenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisterNotMargin");

            migrationBuilder.CreateTable(
                name: "RegisterMargin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CovenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Enrollment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeProduct = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterMargin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisterMargin_Covenants_CovenantId",
                        column: x => x.CovenantId,
                        principalTable: "Covenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisterMargin_CovenantId",
                table: "RegisterMargin",
                column: "CovenantId");
        }
    }
}
