using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientsDirectoryApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    DocumentId = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    LegalAddressCountry = table.Column<string>(type: "TEXT", nullable: false),
                    LegalAddressCity = table.Column<string>(type: "TEXT", nullable: false),
                    LegalAddressLine = table.Column<string>(type: "TEXT", nullable: false),
                    ActualAddressCountry = table.Column<string>(type: "TEXT", nullable: false),
                    ActualAddressCity = table.Column<string>(type: "TEXT", nullable: false),
                    ActualAddressLine = table.Column<string>(type: "TEXT", nullable: false),
                    ProfileImage = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
