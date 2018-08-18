using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payoneer.DotnetCore.Rds.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountHolderId = table.Column<int>(nullable: false),
                    AccountHolderName = table.Column<string>(maxLength: 100, nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Currency = table.Column<string>(maxLength: 3, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    StatusDescription = table.Column<string>(maxLength: 50, nullable: true),
                    Reason = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
