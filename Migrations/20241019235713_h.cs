using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamingAPI.Migrations
{
    public partial class h : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlaylists_Conteudos_ConteudoID",
                table: "ItemPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlaylists_Playlists_PlaylistID",
                table: "ItemPlaylists");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Playlists",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ConteudoID",
                table: "ItemPlaylists",
                newName: "ConteudoId");

            migrationBuilder.RenameColumn(
                name: "PlaylistID",
                table: "ItemPlaylists",
                newName: "PlaylistId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPlaylists_ConteudoID",
                table: "ItemPlaylists",
                newName: "IX_ItemPlaylists_ConteudoId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Conteudos",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlaylists_Conteudos_ConteudoId",
                table: "ItemPlaylists",
                column: "ConteudoId",
                principalTable: "Conteudos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlaylists_Playlists_PlaylistId",
                table: "ItemPlaylists",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlaylists_Conteudos_ConteudoId",
                table: "ItemPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlaylists_Playlists_PlaylistId",
                table: "ItemPlaylists");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Playlists",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ConteudoId",
                table: "ItemPlaylists",
                newName: "ConteudoID");

            migrationBuilder.RenameColumn(
                name: "PlaylistId",
                table: "ItemPlaylists",
                newName: "PlaylistID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPlaylists_ConteudoId",
                table: "ItemPlaylists",
                newName: "IX_ItemPlaylists_ConteudoID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Conteudos",
                newName: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlaylists_Conteudos_ConteudoID",
                table: "ItemPlaylists",
                column: "ConteudoID",
                principalTable: "Conteudos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlaylists_Playlists_PlaylistID",
                table: "ItemPlaylists",
                column: "PlaylistID",
                principalTable: "Playlists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
