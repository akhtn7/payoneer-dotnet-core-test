using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payoneer.DotnetCore.Rds.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "AccountHolderId", "AccountHolderName", "Amount", "Currency", "PaymentDate", "Reason", "Status", "StatusDescription" },
                values: new object[,]
                {
                    { 832321, 15651, "Alex Dumsky", 445.12m, "EUR", new DateTime(2015, 1, 23, 18, 25, 43, 511, DateTimeKind.Utc), null, 0, "Pending" },
                    { 806532, 46556, "Dudi Elias", 4511.12m, "EUR", new DateTime(2015, 2, 10, 18, 25, 43, 511, DateTimeKind.Utc), null, 0, "Pending" },
                    { 7845431, 48481, "Niv Cohen", 10.99m, "USD", new DateTime(2015, 4, 1, 18, 25, 43, 511, DateTimeKind.Utc), "Good Person", 1, "Approved" },
                    { 545341, 32131, "Alex Dumsky", 9952.48m, "EUR", new DateTime(2016, 2, 21, 18, 25, 43, 511, DateTimeKind.Utc), "This is suspicious", 99, "Rejected" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 545341);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 806532);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 832321);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 7845431);
        }
    }
}
