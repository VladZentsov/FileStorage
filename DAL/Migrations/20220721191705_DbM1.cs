using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class DbM1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAccessibility_UserProfiles_UserProfileId",
                table: "FileAccessibility");

            migrationBuilder.DropForeignKey(
                name: "FK_StFile_UserProfiles_UserProfileId",
                table: "StFile");

            migrationBuilder.DropForeignKey(
                name: "FK_StFile_UserStorage_UserStorageId",
                table: "StFile");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageAccessibility_UserProfiles_UserProfileId",
                table: "StorageAccessibility");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "StFile");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserStorage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserProfileId",
                table: "StorageAccessibility",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserStorageId",
                table: "StFile",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserProfileId",
                table: "StFile",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserProfileId",
                table: "FileAccessibility",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAccessibility_UserProfiles_UserProfileId",
                table: "FileAccessibility",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StFile_UserProfiles_UserProfileId",
                table: "StFile",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StFile_UserStorage_UserStorageId",
                table: "StFile",
                column: "UserStorageId",
                principalTable: "UserStorage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageAccessibility_UserProfiles_UserProfileId",
                table: "StorageAccessibility",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAccessibility_UserProfiles_UserProfileId",
                table: "FileAccessibility");

            migrationBuilder.DropForeignKey(
                name: "FK_StFile_UserProfiles_UserProfileId",
                table: "StFile");

            migrationBuilder.DropForeignKey(
                name: "FK_StFile_UserStorage_UserStorageId",
                table: "StFile");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageAccessibility_UserProfiles_UserProfileId",
                table: "StorageAccessibility");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserStorage");

            migrationBuilder.AlterColumn<string>(
                name: "UserProfileId",
                table: "StorageAccessibility",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserStorageId",
                table: "StFile",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserProfileId",
                table: "StFile",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "StFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserProfileId",
                table: "FileAccessibility",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAccessibility_UserProfiles_UserProfileId",
                table: "FileAccessibility",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StFile_UserProfiles_UserProfileId",
                table: "StFile",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StFile_UserStorage_UserStorageId",
                table: "StFile",
                column: "UserStorageId",
                principalTable: "UserStorage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageAccessibility_UserProfiles_UserProfileId",
                table: "StorageAccessibility",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }
    }
}
