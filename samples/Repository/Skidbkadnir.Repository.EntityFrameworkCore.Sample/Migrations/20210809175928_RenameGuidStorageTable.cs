using Microsoft.EntityFrameworkCore.Migrations;

namespace Skidbkadnir.Repository.EntityFrameworkCore.Sample.Migrations
{
    public partial class RenameGuidStorageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "GuidStorage",
                newName: "GuidsStorage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "GuidsStorage",
                newName: "GuidStorage");
        }
    }
}
