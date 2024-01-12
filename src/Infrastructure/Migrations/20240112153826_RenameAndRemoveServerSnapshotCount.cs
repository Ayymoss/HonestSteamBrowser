using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterSteamBrowser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAndRemoveServerSnapshotCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "SnapshotTaken",
                table: "EFServerSnapshots",
                newName: "Taken");

            migrationBuilder.RenameColumn(
                name: "SnapshotCount",
                table: "EFServerSnapshots",
                newName: "PlayerCount");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "610e2452-1996-46e8-b93f-9d5a1d8bdf9b", null, "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN_SEED_ID",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8d46299a-8777-471c-9c41-da8d492d7485", "AQAAAAIAAYagAAAAEKgOt3nYjQGJUN16HTEuRfn2UZM84oSykri/aJMmgEtNRwL0QlhEpDnn0Ymcil76Gw==" });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -9,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5228), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -8,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5227), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -7,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5226), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -6,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5225), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -5,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5224), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -4,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5223), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -3,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5222), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -2,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5221), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -1,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5216), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "610e2452-1996-46e8-b93f-9d5a1d8bdf9b", "ADMIN_SEED_ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "610e2452-1996-46e8-b93f-9d5a1d8bdf9b", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "610e2452-1996-46e8-b93f-9d5a1d8bdf9b");

            migrationBuilder.RenameColumn(
                name: "Taken",
                table: "EFServerSnapshots",
                newName: "SnapshotTaken");

            migrationBuilder.RenameColumn(
                name: "PlayerCount",
                table: "EFServerSnapshots",
                newName: "SnapshotCount");

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
    }
}
