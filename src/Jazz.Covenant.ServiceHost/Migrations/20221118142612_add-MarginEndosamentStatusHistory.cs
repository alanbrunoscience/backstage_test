using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    public partial class addMarginEndosamentStatusHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarginEndosamentStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusEndosament = table.Column<int>(type: "int", nullable: false),
                    EndosamentMarginId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Messagem = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarginEndosamentStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarginEndosamentStatusHistory_EndosamentMargin_EndosamentMarginId",
                        column: x => x.EndosamentMarginId,
                        principalTable: "EndosamentMargin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarginEndosamentStatusHistory_EndosamentMarginId",
                table: "MarginEndosamentStatusHistory",
                column: "EndosamentMarginId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarginEndosamentStatusHistory");
        }
    }
}
