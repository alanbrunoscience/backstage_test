using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    public partial class add_relacao_covenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CovenantId",
                table: "RegisterMargin",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RegisterMargin_CovenantId",
                table: "RegisterMargin",
                column: "CovenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisterMargin_Covenants_CovenantId",
                table: "RegisterMargin",
                column: "CovenantId",
                principalTable: "Covenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisterMargin_Covenants_CovenantId",
                table: "RegisterMargin");

            migrationBuilder.DropIndex(
                name: "IX_RegisterMargin_CovenantId",
                table: "RegisterMargin");

            migrationBuilder.DropColumn(
                name: "CovenantId",
                table: "RegisterMargin");
        }
    }
}
