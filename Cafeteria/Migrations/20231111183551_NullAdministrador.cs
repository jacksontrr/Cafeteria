using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cafeteria.Migrations
{
    public partial class NullAdministrador : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Administradores_AdministradorId",
                table: "Pedidos");

            migrationBuilder.AlterColumn<int>(
                name: "AdministradorId",
                table: "Pedidos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Administradores_AdministradorId",
                table: "Pedidos",
                column: "AdministradorId",
                principalTable: "Administradores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Administradores_AdministradorId",
                table: "Pedidos");

            migrationBuilder.AlterColumn<int>(
                name: "AdministradorId",
                table: "Pedidos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Administradores_AdministradorId",
                table: "Pedidos",
                column: "AdministradorId",
                principalTable: "Administradores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
