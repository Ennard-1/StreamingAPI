using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamingAPI.Migrations
{
    public partial class j : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemPlaylists");

            migrationBuilder.AddColumn<int>(
                name: "PlaylistId",
                table: "Conteudos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conteudos_PlaylistId",
                table: "Conteudos",
                column: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conteudos_Playlists_PlaylistId",
                table: "Conteudos",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conteudos_Playlists_PlaylistId",
                table: "Conteudos");

            migrationBuilder.DropIndex(
                name: "IX_Conteudos_PlaylistId",
                table: "Conteudos");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "Conteudos");

            migrationBuilder.CreateTable(
                name: "ItemPlaylists",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConteudoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPlaylists", x => new { x.PlaylistId, x.ConteudoId });
                    table.ForeignKey(
                        name: "FK_ItemPlaylists_Conteudos_ConteudoId",
                        column: x => x.ConteudoId,
                        principalTable: "Conteudos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPlaylists_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemPlaylists_ConteudoId",
                table: "ItemPlaylists",
                column: "ConteudoId");
        }
    }
}
