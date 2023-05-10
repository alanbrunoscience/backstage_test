using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    public partial class add_RegisterMargin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParameterConventId",
                table: "Modality",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ParameterConvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CovenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinAmountMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxAmountMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SecurityMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PenaltyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxQtyInstallmentMonth = table.Column<int>(type: "int", nullable: false),
                    MinQtyInstallmentMonth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterConvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParameterConvent_Covenants_CovenantId",
                        column: x => x.CovenantId,
                        principalTable: "Covenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegisterMargin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enrollment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeProduct = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterMargin", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modality_ParameterConventId",
                table: "Modality",
                column: "ParameterConventId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterConvent_CovenantId",
                table: "ParameterConvent",
                column: "CovenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modality_ParameterConvent_ParameterConventId",
                table: "Modality",
                column: "ParameterConventId",
                principalTable: "ParameterConvent",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modality_ParameterConvent_ParameterConventId",
                table: "Modality");

            migrationBuilder.DropTable(
                name: "ParameterConvent");

            migrationBuilder.DropTable(
                name: "RegisterMargin");

            migrationBuilder.DropIndex(
                name: "IX_Modality_ParameterConventId",
                table: "Modality");

            migrationBuilder.DropColumn(
                name: "ParameterConventId",
                table: "Modality");
        }
    }
}
