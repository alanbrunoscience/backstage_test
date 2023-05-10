using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    public partial class removido_paramentro_covenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParameterConvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParameterConvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CovenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxAmountMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxQtyInstallmentMonth = table.Column<int>(type: "int", nullable: false),
                    MinAmountMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinQtyInstallmentMonth = table.Column<int>(type: "int", nullable: false),
                    PenaltyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SecurityMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_ParameterConvent_CovenantId",
                table: "ParameterConvent",
                column: "CovenantId");
        }
    }
}
