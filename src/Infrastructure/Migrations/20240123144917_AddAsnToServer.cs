using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterSteamBrowser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAsnToServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "610e2452-1996-46e8-b93f-9d5a1d8bdf9b", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "610e2452-1996-46e8-b93f-9d5a1d8bdf9b");

            migrationBuilder.AddColumn<long>(
                name: "IpAddressAsn",
                table: "EFServers",
                type: "bigint",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f25cf932-446f-45bc-8777-374d5a246d47", null, "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN_SEED_ID",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3bbb5469-8ff0-404e-9e5a-90bdec09c1dc", "AQAAAAIAAYagAAAAEPoOhBjTypJW78Vj+Bilr/ijKJMWrfMa3MpgTh23q/uO4+y3KjjxTJeaHDyCcO3nRg==" });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -9,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 14, 49, 16, 921, DateTimeKind.Unspecified).AddTicks(4546), new TimeSpan(0, 0, 0, 0, 0)), 3 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -8,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 14, 49, 16, 921, DateTimeKind.Unspecified).AddTicks(4545), new TimeSpan(0, 0, 0, 0, 0)), 4 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -7,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 14, 49, 16, 921, DateTimeKind.Unspecified).AddTicks(4544), new TimeSpan(0, 0, 0, 0, 0)), 4 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -6,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 14, 49, 16, 921, DateTimeKind.Unspecified).AddTicks(4543), new TimeSpan(0, 0, 0, 0, 0)), 3 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -5,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 14, 49, 16, 921, DateTimeKind.Unspecified).AddTicks(4542), new TimeSpan(0, 0, 0, 0, 0)), 3 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 14, 49, 16, 921, DateTimeKind.Unspecified).AddTicks(4541), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 14, 49, 16, 921, DateTimeKind.Unspecified).AddTicks(4539), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 14, 49, 16, 921, DateTimeKind.Unspecified).AddTicks(4538), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 14, 49, 16, 921, DateTimeKind.Unspecified).AddTicks(4532), new TimeSpan(0, 0, 0, 0, 0)), 3 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f25cf932-446f-45bc-8777-374d5a246d47", "ADMIN_SEED_ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f25cf932-446f-45bc-8777-374d5a246d47", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f25cf932-446f-45bc-8777-374d5a246d47");

            migrationBuilder.DropColumn(
                name: "IpAddressAsn",
                table: "EFServers");

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
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5228), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -8,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5227), new TimeSpan(0, 0, 0, 0, 0)), 3 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -7,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5226), new TimeSpan(0, 0, 0, 0, 0)), 3 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -6,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5225), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -5,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5224), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5223), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5222), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5221), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 12, 15, 38, 26, 364, DateTimeKind.Unspecified).AddTicks(5216), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "610e2452-1996-46e8-b93f-9d5a1d8bdf9b", "ADMIN_SEED_ID" });
        }
    }
}
