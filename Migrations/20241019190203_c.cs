using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamingAPI.Migrations
{
    public partial class c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conteudos_Criadores_CriadorID",
                table: "Conteudos");

            migrationBuilder.AddColumn<string>(
                name: "NomeArquivo",
                table: "Conteudos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Conteudos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Conteudos_Criadores_CriadorID",
                table: "Conteudos",
                column: "CriadorID",
                principalTable: "Criadores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conteudos_Criadores_CriadorID",
                table: "Conteudos");

            migrationBuilder.DropColumn(
                name: "NomeArquivo",
                table: "Conteudos");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Conteudos");

            migrationBuilder.AddForeignKey(
                name: "FK_Conteudos_Criadores_CriadorID",
                table: "Conteudos",
                column: "CriadorID",
                principalTable: "Criadores",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
