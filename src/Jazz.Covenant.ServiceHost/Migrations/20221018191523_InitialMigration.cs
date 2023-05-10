using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Endoser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndoserIdentifier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endoser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modality",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modality", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Covenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    EndoserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentifierInEndoser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Covenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Covenants_Endoser_EndoserId",
                        column: x => x.EndoserId,
                        principalTable: "Endoser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModalityCovenant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CovenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModalityCovenant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModalityCovenant_Covenants_CovenantId",
                        column: x => x.CovenantId,
                        principalTable: "Covenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModalityCovenant_Modality_ModalityId",
                        column: x => x.ModalityId,
                        principalTable: "Modality",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Covenants_EndoserId",
                table: "Covenants",
                column: "EndoserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModalityCovenant_CovenantId",
                table: "ModalityCovenant",
                column: "CovenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ModalityCovenant_ModalityId",
                table: "ModalityCovenant",
                column: "ModalityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModalityCovenant");

            migrationBuilder.DropTable(
                name: "Covenants");

            migrationBuilder.DropTable(
                name: "Modality");

            migrationBuilder.DropTable(
                name: "Endoser");
        }
    }
}
