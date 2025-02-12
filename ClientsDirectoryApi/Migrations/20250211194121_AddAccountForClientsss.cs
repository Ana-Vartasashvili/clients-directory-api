using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientsDirectoryApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountForClientsss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "Accounts",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Accounts",
                newName: "status");
        }
    }
}
