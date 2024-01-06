using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterSteamBrowser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AverageAndBounds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "6e972fb8-3976-47f8-aa03-c6456e8aa3c9", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e972fb8-3976-47f8-aa03-c6456e8aa3c9");

            migrationBuilder.AddColumn<double>(
                name: "PlayerAverage",
                table: "EFServers",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerLowerBound",
                table: "EFServers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerUpperBound",
                table: "EFServers",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "efc525c8-db10-405b-b4da-6c4661ed5b00", null, "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN_SEED_ID",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d3f8e700-7c8b-4fa4-a7cd-9e4b781f466b", "AQAAAAIAAYagAAAAEJQqoxF+jjSvPEeUT7esACm1aMgPMh37p9BZKCR3uD1vYehgR+s559sJNcCgGcM1bQ==" });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -9,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 6, 2, 30, 28, 582, DateTimeKind.Unspecified).AddTicks(4916), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -8,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 6, 2, 30, 28, 582, DateTimeKind.Unspecified).AddTicks(4914), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -7,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 6, 2, 30, 28, 582, DateTimeKind.Unspecified).AddTicks(4913), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -6,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 6, 2, 30, 28, 582, DateTimeKind.Unspecified).AddTicks(4912), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -5,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 6, 2, 30, 28, 582, DateTimeKind.Unspecified).AddTicks(4911), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -4,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 6, 2, 30, 28, 582, DateTimeKind.Unspecified).AddTicks(4910), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -3,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 6, 2, 30, 28, 582, DateTimeKind.Unspecified).AddTicks(4909), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -2,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 6, 2, 30, 28, 582, DateTimeKind.Unspecified).AddTicks(4908), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -1,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 6, 2, 30, 28, 582, DateTimeKind.Unspecified).AddTicks(4903), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "efc525c8-db10-405b-b4da-6c4661ed5b00", "ADMIN_SEED_ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "efc525c8-db10-405b-b4da-6c4661ed5b00", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "efc525c8-db10-405b-b4da-6c4661ed5b00");

            migrationBuilder.DropColumn(
                name: "PlayerAverage",
                table: "EFServers");

            migrationBuilder.DropColumn(
                name: "PlayerLowerBound",
                table: "EFServers");

            migrationBuilder.DropColumn(
                name: "PlayerUpperBound",
                table: "EFServers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6e972fb8-3976-47f8-aa03-c6456e8aa3c9", null, "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN_SEED_ID",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6a0e814d-8aef-41aa-ab69-16725e436a78", "AQAAAAIAAYagAAAAEAPi1XvCI6Jb749h/hAVuOMyDw5uQMKtEd5r5vkH+FUasIScj3zxMKP+bj4lrDRyPQ==" });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -9,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 5, 1, 8, 49, 949, DateTimeKind.Unspecified).AddTicks(4697), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -8,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 5, 1, 8, 49, 949, DateTimeKind.Unspecified).AddTicks(4695), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -7,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 5, 1, 8, 49, 949, DateTimeKind.Unspecified).AddTicks(4693), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -6,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 5, 1, 8, 49, 949, DateTimeKind.Unspecified).AddTicks(4692), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -5,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 5, 1, 8, 49, 949, DateTimeKind.Unspecified).AddTicks(4691), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -4,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 5, 1, 8, 49, 949, DateTimeKind.Unspecified).AddTicks(4690), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -3,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 5, 1, 8, 49, 949, DateTimeKind.Unspecified).AddTicks(4689), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -2,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 5, 1, 8, 49, 949, DateTimeKind.Unspecified).AddTicks(4687), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -1,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 5, 1, 8, 49, 949, DateTimeKind.Unspecified).AddTicks(4682), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "6e972fb8-3976-47f8-aa03-c6456e8aa3c9", "ADMIN_SEED_ID" });
        }
    }
}
