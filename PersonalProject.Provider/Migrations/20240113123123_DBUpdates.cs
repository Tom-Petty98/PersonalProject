using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalProject.Provider.Migrations
{
    /// <inheritdoc />
    public partial class DBUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "AS_Index_Code",
                table: "UserInviteStatuses",
                newName: "UIS_Index_Code");

            migrationBuilder.RenameIndex(
                name: "AS_Index_Code",
                table: "InstallerStatuses",
                newName: "IS_Index_Code");

            migrationBuilder.AlterColumn<int>(
                name: "EntityId",
                table: "AuditLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "AuditLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResultStatus",
                table: "AuditLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "AuditLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstallerId",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "ResultStatus",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "InstallerId",
                table: "Applications");

            migrationBuilder.RenameIndex(
                name: "UIS_Index_Code",
                table: "UserInviteStatuses",
                newName: "AS_Index_Code");

            migrationBuilder.RenameIndex(
                name: "IS_Index_Code",
                table: "InstallerStatuses",
                newName: "AS_Index_Code");

            migrationBuilder.AlterColumn<int>(
                name: "EntityId",
                table: "AuditLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
