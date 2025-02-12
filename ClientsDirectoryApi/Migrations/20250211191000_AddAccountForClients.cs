using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientsDirectoryApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountForClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Account_ClientId",
                table: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_Account_ClientId",
                table: "Account",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Account_ClientId",
                table: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_Account_ClientId",
                table: "Account",
                column: "ClientId",
                unique: true);
        }
    }
}
