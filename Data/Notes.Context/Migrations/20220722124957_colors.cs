using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Context.Migrations
{
    public partial class colors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Color",
                table: "taskTypes",
                newName: "TypeColorId");

            migrationBuilder.CreateTable(
                name: "taskTypesColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taskTypesColors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_taskTypes_TypeColorId",
                table: "taskTypes",
                column: "TypeColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_taskTypes_taskTypesColors_TypeColorId",
                table: "taskTypes",
                column: "TypeColorId",
                principalTable: "taskTypesColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_taskTypes_taskTypesColors_TypeColorId",
                table: "taskTypes");

            migrationBuilder.DropTable(
                name: "taskTypesColors");

            migrationBuilder.DropIndex(
                name: "IX_taskTypes_TypeColorId",
                table: "taskTypes");

            migrationBuilder.RenameColumn(
                name: "TypeColorId",
                table: "taskTypes",
                newName: "Color");
        }
    }
}
