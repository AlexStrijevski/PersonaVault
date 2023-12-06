using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonaVault.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    City = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Street = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    HouseNumber = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ApartamentNumber = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalCode = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PhoneNumber = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EmailAddress = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AddressDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalDetails_AddressDetails_AddressDetailsId",
                        column: x => x.AddressDetailsId,
                        principalTable: "AddressDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_PersonalDetails_PersonalDetailsId",
                        column: x => x.PersonalDetailsId,
                        principalTable: "PersonalDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetails_AddressDetailsId",
                table: "PersonalDetails",
                column: "AddressDetailsId",
                unique: true,
                filter: "[AddressDetailsId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonalDetailsId",
                table: "Users",
                column: "PersonalDetailsId",
                unique: true,
                filter: "[PersonalDetailsId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PersonalDetails");

            migrationBuilder.DropTable(
                name: "AddressDetails");
        }
    }
}
