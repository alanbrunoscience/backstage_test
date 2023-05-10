using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    public partial class add_table_convenant_favorite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CovenantFavorite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Favorite = table.Column<bool>(type: "bit", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CovenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CovenantFavorite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CovenantFavorite_Covenants_CovenantId",
                        column: x => x.CovenantId,
                        principalTable: "Covenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CovenantFavorite_CovenantId",
                table: "CovenantFavorite",
                column: "CovenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CovenantFavorite");
        }
    }
}
