using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    public partial class retirado_parametro_convenant_modality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modality_ParameterConvent_ParameterConventId",
                table: "Modality");

            migrationBuilder.DropIndex(
                name: "IX_Modality_ParameterConventId",
                table: "Modality");

            migrationBuilder.DropColumn(
                name: "ParameterConventId",
                table: "Modality");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParameterConventId",
                table: "Modality",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modality_ParameterConventId",
                table: "Modality",
                column: "ParameterConventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modality_ParameterConvent_ParameterConventId",
                table: "Modality",
                column: "ParameterConventId",
                principalTable: "ParameterConvent",
                principalColumn: "Id");
        }
    }
}
