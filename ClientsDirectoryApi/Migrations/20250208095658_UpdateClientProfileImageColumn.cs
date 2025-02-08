using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientsDirectoryApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientProfileImageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "Clients");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "Clients",
                type: "BLOB",
                nullable: true);
        }
    }
}
