using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamingAPI.Migrations
{
    public partial class f : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Criadores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Criadores_UsuarioId",
                table: "Criadores",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Criadores_Usuarios_UsuarioId",
                table: "Criadores",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criadores_Usuarios_UsuarioId",
                table: "Criadores");

            migrationBuilder.DropIndex(
                name: "IX_Criadores_UsuarioId",
                table: "Criadores");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Criadores");
        }
    }
}
