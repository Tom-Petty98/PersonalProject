using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProject.Provider.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDetails_Address_InstallationAddressId",
                table: "ApplicationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_InstallerDetails_Address_InstallerAddressId",
                table: "InstallerDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresses");

            migrationBuilder.RenameColumn(
                name: "IsInternalOnly",
                table: "Notes",
                newName: "IsExternal");

            migrationBuilder.AddColumn<int>(
                name: "EntityTypeId",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityTypeId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EpcNumber",
                table: "ApplicationDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechTypeId",
                table: "ApplicationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TechTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDetails_TechTypeId",
                table: "ApplicationDetails",
                column: "TechTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDetails_Addresses_InstallationAddressId",
                table: "ApplicationDetails",
                column: "InstallationAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDetails_TechTypes_TechTypeId",
                table: "ApplicationDetails",
                column: "TechTypeId",
                principalTable: "TechTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstallerDetails_Addresses_InstallerAddressId",
                table: "InstallerDetails",
                column: "InstallerAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDetails_Addresses_InstallationAddressId",
                table: "ApplicationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDetails_TechTypes_TechTypeId",
                table: "ApplicationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_InstallerDetails_Addresses_InstallerAddressId",
                table: "InstallerDetails");

            migrationBuilder.DropTable(
                name: "TechTypes");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationDetails_TechTypeId",
                table: "ApplicationDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "EntityTypeId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "EntityTypeId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "EpcNumber",
                table: "ApplicationDetails");

            migrationBuilder.DropColumn(
                name: "TechTypeId",
                table: "ApplicationDetails");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "IsExternal",
                table: "Notes",
                newName: "IsInternalOnly");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDetails_Address_InstallationAddressId",
                table: "ApplicationDetails",
                column: "InstallationAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstallerDetails_Address_InstallerAddressId",
                table: "InstallerDetails",
                column: "InstallerAddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }
    }
}
