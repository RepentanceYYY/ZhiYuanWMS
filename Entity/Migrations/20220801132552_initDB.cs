using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entity.Migrations
{
    public partial class initDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "FileInfo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RelationId = table.Column<string>(nullable: true),
                    OldFileName = table.Column<string>(nullable: true),
                    NewFileName = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Length = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInfo", x => x.Id);
                });

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ConsumableInfo");

            migrationBuilder.DropTable(
                name: "ConsumableRecord");

            migrationBuilder.DropTable(
                name: "DepartmentInfo");

            migrationBuilder.DropTable(
                name: "FileInfo");

            migrationBuilder.DropTable(
                name: "MenuInfo");

            migrationBuilder.DropTable(
                name: "R_RoleInfo_MenuInfo");

            migrationBuilder.DropTable(
                name: "R_UserInfo_RoleInfo");

            migrationBuilder.DropTable(
                name: "RoleInfo");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "WorkFlow_Instance");

            migrationBuilder.DropTable(
                name: "WorkFlow_InstanceStep");

            migrationBuilder.DropTable(
                name: "WorkFlow_Model");
        }
    }
}
