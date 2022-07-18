using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Context.Migrations
{
    public partial class Init_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskTypeId",
                table: "notes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskTypeId",
                table: "notes");
        }
    }
}
