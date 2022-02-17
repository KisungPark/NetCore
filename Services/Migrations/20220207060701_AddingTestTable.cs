using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Net.Service.Migrations
{
    public partial class AddingTestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedUtcDate",
                table: "UserRole",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "sysutcdatetime()",
                comment: "생성일",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "생성일");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedUtcDate",
                table: "UserRole",
                type: "datetime2",
                nullable: false,
                comment: "생성일",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "sysutcdatetime()",
                oldComment: "생성일");
        }
    }
}
