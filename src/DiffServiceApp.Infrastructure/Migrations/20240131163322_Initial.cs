using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiffServiceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiffPayloadCouples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LeftPayloadValue = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    RightPayloadValue = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiffPayloadCouples", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiffPayloadCouples_Id",
                table: "DiffPayloadCouples",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiffPayloadCouples");
        }
    }
}
