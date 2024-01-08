using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterSteamBrowser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFixedSnapshotCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "efc525c8-db10-405b-b4da-6c4661ed5b00", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "efc525c8-db10-405b-b4da-6c4661ed5b00");

            migrationBuilder.AddColumn<int>(
                name: "PlayerSnapshotCount",
                table: "EFServers",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "23ac2f49-cb60-421f-864d-2db5cfc508d2", null, "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN_SEED_ID",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "74ac3b74-c461-457d-9621-4286227a6152", "AQAAAAIAAYagAAAAEFPjUsfRbAMmfXJGysLC+NQ5d8bJ6Kzq30kjzjaFD9CCXP3/4IFAPD9japik9I0CEw==" });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -9,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 8, 18, 8, 41, 401, DateTimeKind.Unspecified).AddTicks(4907), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -8,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 8, 18, 8, 41, 401, DateTimeKind.Unspecified).AddTicks(4906), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -7,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 8, 18, 8, 41, 401, DateTimeKind.Unspecified).AddTicks(4905), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -6,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 8, 18, 8, 41, 401, DateTimeKind.Unspecified).AddTicks(4904), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -5,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 8, 18, 8, 41, 401, DateTimeKind.Unspecified).AddTicks(4902), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -4,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 8, 18, 8, 41, 401, DateTimeKind.Unspecified).AddTicks(4901), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -3,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 8, 18, 8, 41, 401, DateTimeKind.Unspecified).AddTicks(4900), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -2,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 8, 18, 8, 41, 401, DateTimeKind.Unspecified).AddTicks(4899), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -1,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 8, 18, 8, 41, 401, DateTimeKind.Unspecified).AddTicks(4893), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "23ac2f49-cb60-421f-864d-2db5cfc508d2", "ADMIN_SEED_ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "23ac2f49-cb60-421f-864d-2db5cfc508d2", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23ac2f49-cb60-421f-864d-2db5cfc508d2");

            migrationBuilder.DropColumn(
                name: "PlayerSnapshotCount",
                table: "EFServers");

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
    }
}
