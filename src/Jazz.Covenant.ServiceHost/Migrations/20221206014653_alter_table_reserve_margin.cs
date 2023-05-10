using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    public partial class alter_table_reserve_margin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReserveMargin_Endoser_EndoserId",
                table: "ReserveMargin");

            migrationBuilder.DropIndex(
                name: "IX_ReserveMargin_EndoserId",
                table: "ReserveMargin");

            migrationBuilder.CreateTable(
                name: "MarginReserve",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Enrollment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValueInstallment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountReleased = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfInstallments = table.Column<int>(type: "int", nullable: false),
                    ContractNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rubric = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortableIdentifierNumberReserveCovenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarginIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgencyAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    CovenantAutorization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Agency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FounderRegistration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IOFValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CETValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDateCIPProcess = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CIPProcessNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxIdPortedInstitution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameConsigneeCarried = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortedContractNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValueInstallmentPortability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValueInstallmentRefinancing = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContractDuration = table.Column<int>(type: "int", nullable: false),
                    ExpirationDay = table.Column<int>(type: "int", nullable: false),
                    ContractEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdentifierNumberReserveCovenant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefinancedContractNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxIdBankingAgency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FederatedStateContracting = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CovenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndoserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentifierInEndoser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndoserIdentifier = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MarginReserveStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarginReserve", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarginReserve_Covenants_CovenantId",
                        column: x => x.CovenantId,
                        principalTable: "Covenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarginReserve_Endoser_EndoserId",
                        column: x => x.EndoserId,
                        principalTable: "Endoser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarginReserveStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarginReserveId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Messagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarginReserveStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarginReserveStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarginReserveStatusHistory_MarginReserve_MarginReserveId",
                        column: x => x.MarginReserveId,
                        principalTable: "MarginReserve",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarginReserve_CovenantId",
                table: "MarginReserve",
                column: "CovenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MarginReserve_EndoserId",
                table: "MarginReserve",
                column: "EndoserId");

            migrationBuilder.CreateIndex(
                name: "IX_MarginReserveStatusHistory_MarginReserveId",
                table: "MarginReserveStatusHistory",
                column: "MarginReserveId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarginReserveStatusHistory");

            migrationBuilder.DropTable(
                name: "MarginReserve");

            migrationBuilder.CreateIndex(
                name: "IX_ReserveMargin_EndoserId",
                table: "ReserveMargin",
                column: "EndoserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReserveMargin_Endoser_EndoserId",
                table: "ReserveMargin",
                column: "EndoserId",
                principalTable: "Endoser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
