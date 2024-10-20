using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamingAPI.Migrations
{
    public partial class g : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conteudos_Criadores_CriadorID",
                table: "Conteudos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlaylist_Conteudos_ConteudoID",
                table: "ItemPlaylist");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlaylist_Playlists_PlaylistID",
                table: "ItemPlaylist");

            migrationBuilder.DropTable(
                name: "Criadores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemPlaylist",
                table: "ItemPlaylist");

            migrationBuilder.RenameTable(
                name: "ItemPlaylist",
                newName: "ItemPlaylists");

            migrationBuilder.RenameColumn(
                name: "CriadorID",
                table: "Conteudos",
                newName: "UsuarioID");

            migrationBuilder.RenameIndex(
                name: "IX_Conteudos_CriadorID",
                table: "Conteudos",
                newName: "IX_Conteudos_UsuarioID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPlaylist_ConteudoID",
                table: "ItemPlaylists",
                newName: "IX_ItemPlaylists_ConteudoID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemPlaylists",
                table: "ItemPlaylists",
                columns: new[] { "PlaylistID", "ConteudoID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Conteudos_Usuarios_UsuarioID",
                table: "Conteudos",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conteudos_Usuarios_UsuarioID",
                table: "Conteudos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlaylists_Conteudos_ConteudoID",
                table: "ItemPlaylists");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlaylists_Playlists_PlaylistID",
                table: "ItemPlaylists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemPlaylists",
                table: "ItemPlaylists");

            migrationBuilder.RenameTable(
                name: "ItemPlaylists",
                newName: "ItemPlaylist");

            migrationBuilder.RenameColumn(
                name: "UsuarioID",
                table: "Conteudos",
                newName: "CriadorID");

            migrationBuilder.RenameIndex(
                name: "IX_Conteudos_UsuarioID",
                table: "Conteudos",
                newName: "IX_Conteudos_CriadorID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPlaylists_ConteudoID",
                table: "ItemPlaylist",
                newName: "IX_ItemPlaylist_ConteudoID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemPlaylist",
                table: "ItemPlaylist",
                columns: new[] { "PlaylistID", "ConteudoID" });

            migrationBuilder.CreateTable(
                name: "Criadores",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criadores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Criadores_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Criadores_UsuarioId",
                table: "Criadores",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conteudos_Criadores_CriadorID",
                table: "Conteudos",
                column: "CriadorID",
                principalTable: "Criadores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlaylist_Conteudos_ConteudoID",
                table: "ItemPlaylist",
                column: "ConteudoID",
                principalTable: "Conteudos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlaylist_Playlists_PlaylistID",
                table: "ItemPlaylist",
                column: "PlaylistID",
                principalTable: "Playlists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
