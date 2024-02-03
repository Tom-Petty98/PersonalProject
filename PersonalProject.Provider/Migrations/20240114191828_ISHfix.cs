using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProject.Provider.Migrations
{
    /// <inheritdoc />
    public partial class ISHfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstallerStatusHistories_ApplicationStatuses_ApplicationStatusId",
                table: "InstallerStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_InstallerStatusHistories_ApplicationStatusId",
                table: "InstallerStatusHistories");

            migrationBuilder.DropColumn(
                name: "ApplicationStatusId",
                table: "InstallerStatusHistories");

            migrationBuilder.CreateIndex(
                name: "IX_InstallerStatusHistories_InstallerStatusId",
                table: "InstallerStatusHistories",
                column: "InstallerStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstallerStatusHistories_InstallerStatuses_InstallerStatusId",
                table: "InstallerStatusHistories",
                column: "InstallerStatusId",
                principalTable: "InstallerStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstallerStatusHistories_InstallerStatuses_InstallerStatusId",
                table: "InstallerStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_InstallerStatusHistories_InstallerStatusId",
                table: "InstallerStatusHistories");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatusId",
                table: "InstallerStatusHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InstallerStatusHistories_ApplicationStatusId",
                table: "InstallerStatusHistories",
                column: "ApplicationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstallerStatusHistories_ApplicationStatuses_ApplicationStatusId",
                table: "InstallerStatusHistories",
                column: "ApplicationStatusId",
                principalTable: "ApplicationStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
