using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jazz.Covenant.ServiceHost.Migrations
{
    public partial class trocado_para_string : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TypeProduct",
                table: "RegisterMargin",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TypeProduct",
                table: "RegisterMargin",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
