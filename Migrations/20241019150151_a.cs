using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamingAPI.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Users_UserId",
                table: "Playlists");

            migrationBuilder.DropTable(
                name: "VideoPlaylists");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Playlists",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Playlists",
                newName: "UsuarioID");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Playlists",
                newName: "Nome");

            migrationBuilder.RenameIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists",
                newName: "IX_Playlists_UsuarioID");

            migrationBuilder.CreateTable(
                name: "Criadores",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criadores", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Conteudos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    CriadorID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conteudos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Conteudos_Criadores_CriadorID",
                        column: x => x.CriadorID,
                        principalTable: "Criadores",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ItemPlaylist",
                columns: table => new
                {
                    PlaylistID = table.Column<int>(type: "INTEGER", nullable: false),
                    ConteudoID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPlaylist", x => new { x.PlaylistID, x.ConteudoID });
                    table.ForeignKey(
                        name: "FK_ItemPlaylist_Conteudos_ConteudoID",
                        column: x => x.ConteudoID,
                        principalTable: "Conteudos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPlaylist_Playlists_PlaylistID",
                        column: x => x.PlaylistID,
                        principalTable: "Playlists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conteudos_CriadorID",
                table: "Conteudos",
                column: "CriadorID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPlaylist_ConteudoID",
                table: "ItemPlaylist",
                column: "ConteudoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Usuarios_UsuarioID",
                table: "Playlists",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Usuarios_UsuarioID",
                table: "Playlists");

            migrationBuilder.DropTable(
                name: "ItemPlaylist");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Conteudos");

            migrationBuilder.DropTable(
                name: "Criadores");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Playlists",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UsuarioID",
                table: "Playlists",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Playlists",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Playlists_UsuarioID",
                table: "Playlists",
                newName: "IX_Playlists_UserId");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsCreator = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    VideoPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoPlaylists",
                columns: table => new
                {
                    VideoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlaylistId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoPlaylists", x => new { x.VideoId, x.PlaylistId });
                    table.ForeignKey(
                        name: "FK_VideoPlaylists_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoPlaylists_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoPlaylists_PlaylistId",
                table: "VideoPlaylists",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_CreatorId",
                table: "Videos",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Users_UserId",
                table: "Playlists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
