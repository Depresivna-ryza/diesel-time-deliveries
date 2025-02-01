using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Couriers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Weight_Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Destination = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.PackageId);
                });

            migrationBuilder.InsertData(
                table: "Couriers",
                column: "id",
                values: new object[]
                {
                    new Guid("35ed4826-3385-4794-b533-7dabc5abe2fc"),
                    new Guid("4c063e5f-065f-4a89-8e1e-56e3c824f6ed"),
                    new Guid("bc5d74f3-915d-4724-9e4b-dad2ddcb6663"),
                    new Guid("fe580e45-4a7a-469e-a5a5-c5a76f26e5d0")
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Couriers");

            migrationBuilder.DropTable(
                name: "Packages");
        }
    }
}
