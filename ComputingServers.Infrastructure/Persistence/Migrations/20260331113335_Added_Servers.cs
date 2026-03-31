using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputingServers.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Servers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "servers");

            migrationBuilder.CreateTable(
                name: "Servers",
                schema: "servers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MemoryCapacity = table.Column<int>(type: "int", nullable: false),
                    DiskCapacity = table.Column<int>(type: "int", nullable: false),
                    CpuNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    RentedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUsed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Servers",
                schema: "servers");
        }
    }
}
