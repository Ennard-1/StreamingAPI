using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamingAPI.Migrations
{
    public partial class m : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlaylist_Conteudos_ConteudoId",
                table: "ItemPlaylist");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlaylist_Playlists_PlaylistId",
                table: "ItemPlaylist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemPlaylist",
                table: "ItemPlaylist");

            migrationBuilder.RenameTable(
                name: "ItemPlaylist",
                newName: "ItemPlaylists");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPlaylist_ConteudoId",
                table: "ItemPlaylists",
                newName: "IX_ItemPlaylists_ConteudoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemPlaylists",
                table: "ItemPlaylists",
                columns: new[] { "PlaylistId", "ConteudoId" });

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemPlaylists",
                table: "ItemPlaylists");

            migrationBuilder.RenameTable(
                name: "ItemPlaylists",
                newName: "ItemPlaylist");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPlaylists_ConteudoId",
                table: "ItemPlaylist",
                newName: "IX_ItemPlaylist_ConteudoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemPlaylist",
                table: "ItemPlaylist",
                columns: new[] { "PlaylistId", "ConteudoId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlaylist_Conteudos_ConteudoId",
                table: "ItemPlaylist",
                column: "ConteudoId",
                principalTable: "Conteudos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlaylist_Playlists_PlaylistId",
                table: "ItemPlaylist",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
