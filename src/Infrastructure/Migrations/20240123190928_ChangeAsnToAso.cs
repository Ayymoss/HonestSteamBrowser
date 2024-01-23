using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterSteamBrowser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAsnToAso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "AutonomousSystemOrganization",
                table: "EFServers",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ADMIN_ROLE_ID", null, "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN_SEED_ID",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2cff2fc5-c391-446b-b980-68975bf47f71", "AQAAAAIAAYagAAAAEH+vFla8Y1SKLqZzERXjdrDFE3BdrOyaxTsrML+N59YPivKRCLOc510mpPKMCnkBHg==" });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -9,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 19, 9, 28, 85, DateTimeKind.Unspecified).AddTicks(5594), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -8,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 19, 9, 28, 85, DateTimeKind.Unspecified).AddTicks(5593), new TimeSpan(0, 0, 0, 0, 0)), 3 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -7,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 19, 9, 28, 85, DateTimeKind.Unspecified).AddTicks(5592), new TimeSpan(0, 0, 0, 0, 0)), 3 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -6,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 19, 9, 28, 85, DateTimeKind.Unspecified).AddTicks(5591), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -5,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 19, 9, 28, 85, DateTimeKind.Unspecified).AddTicks(5590), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 19, 9, 28, 85, DateTimeKind.Unspecified).AddTicks(5589), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 19, 9, 28, 85, DateTimeKind.Unspecified).AddTicks(5588), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 19, 9, 28, 85, DateTimeKind.Unspecified).AddTicks(5586), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Added", "Type" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 23, 19, 9, 28, 85, DateTimeKind.Unspecified).AddTicks(5581), new TimeSpan(0, 0, 0, 0, 0)), 2 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ADMIN_ROLE_ID", "ADMIN_SEED_ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ADMIN_ROLE_ID", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ADMIN_ROLE_ID");

            migrationBuilder.DropColumn(
                name: "AutonomousSystemOrganization",
                table: "EFServers");

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
    }
}
