using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentControl.MVC.Migrations
{
    public partial class addFileType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "FilesOnDatabase",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "FilesOnDatabase");
        }
    }
}
