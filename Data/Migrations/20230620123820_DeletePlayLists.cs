using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class DeletePlayLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistsToVideos");

            migrationBuilder.DropTable(
                name: "UsersToSavedPlaylists");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Notifications",
                newName: "NotificationId");

            migrationBuilder.AlterColumn<int>(
                name: "VideoId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificationId",
                table: "Notifications",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "VideoId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.PlaylistId);
                    table.ForeignKey(
                        name: "FK_Playlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PlaylistsToVideos",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "int", nullable: false),
                    VideosVideoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistsToVideos", x => new { x.PlaylistId, x.VideosVideoId });
                    table.ForeignKey(
                        name: "FK_PlaylistsToVideos_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistsToVideos_Videos_VideosVideoId",
                        column: x => x.VideosVideoId,
                        principalTable: "Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersToSavedPlaylists",
                columns: table => new
                {
                    SavedByUsersUserId = table.Column<int>(type: "int", nullable: false),
                    SavedPlaylistsPlaylistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersToSavedPlaylists", x => new { x.SavedByUsersUserId, x.SavedPlaylistsPlaylistId });
                    table.ForeignKey(
                        name: "FK_UsersToSavedPlaylists_Playlists_SavedPlaylistsPlaylistId",
                        column: x => x.SavedPlaylistsPlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistId");
                    table.ForeignKey(
                        name: "FK_UsersToSavedPlaylists_Users_SavedByUsersUserId",
                        column: x => x.SavedByUsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistsToVideos_VideosVideoId",
                table: "PlaylistsToVideos",
                column: "VideosVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersToSavedPlaylists_SavedPlaylistsPlaylistId",
                table: "UsersToSavedPlaylists",
                column: "SavedPlaylistsPlaylistId");
        }
    }
}
