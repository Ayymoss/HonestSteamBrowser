using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BetterSteamBrowser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Snapshotting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "edb9f7df-2489-4864-8a58-97470291f8f2", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edb9f7df-2489-4864-8a58-97470291f8f2");

            migrationBuilder.CreateTable(
                name: "EFSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Snapshot = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Stored = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFSnapshots", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "35e41962-0016-42f2-b7b8-bea97c77bb7e", null, "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN_SEED_ID",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "62c7cfd8-01c3-4ff8-9ace-19c2216ca565", "AQAAAAIAAYagAAAAEH88I3W2CIRvFQh4Qh4REspiPte+/Ff+5Upg3d1gaji3Eidf3f+1zCLXkbzhAAzJIw==" });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -9,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 23, 30, 1, 465, DateTimeKind.Unspecified).AddTicks(1497), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -8,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 23, 30, 1, 465, DateTimeKind.Unspecified).AddTicks(1496), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -7,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 23, 30, 1, 465, DateTimeKind.Unspecified).AddTicks(1495), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -6,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 23, 30, 1, 465, DateTimeKind.Unspecified).AddTicks(1494), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -5,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 23, 30, 1, 465, DateTimeKind.Unspecified).AddTicks(1493), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -4,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 23, 30, 1, 465, DateTimeKind.Unspecified).AddTicks(1491), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -3,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 23, 30, 1, 465, DateTimeKind.Unspecified).AddTicks(1490), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -2,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 23, 30, 1, 465, DateTimeKind.Unspecified).AddTicks(1489), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -1,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 23, 30, 1, 465, DateTimeKind.Unspecified).AddTicks(1483), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "35e41962-0016-42f2-b7b8-bea97c77bb7e", "ADMIN_SEED_ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EFSnapshots");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "35e41962-0016-42f2-b7b8-bea97c77bb7e", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35e41962-0016-42f2-b7b8-bea97c77bb7e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "edb9f7df-2489-4864-8a58-97470291f8f2", null, "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN_SEED_ID",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c443cbad-9a78-4c12-9904-7e7239187f57", "AQAAAAIAAYagAAAAEOSO1D5iO2OvDn4lZbA9mvBlxy548zmMQgET68L9a//rawahrHWO7UQ6o4d+6pHKzA==" });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -9,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 19, 47, 43, 743, DateTimeKind.Unspecified).AddTicks(6634), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -8,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 19, 47, 43, 743, DateTimeKind.Unspecified).AddTicks(6633), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -7,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 19, 47, 43, 743, DateTimeKind.Unspecified).AddTicks(6632), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -6,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 19, 47, 43, 743, DateTimeKind.Unspecified).AddTicks(6631), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -5,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 19, 47, 43, 743, DateTimeKind.Unspecified).AddTicks(6630), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -4,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 19, 47, 43, 743, DateTimeKind.Unspecified).AddTicks(6629), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -3,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 19, 47, 43, 743, DateTimeKind.Unspecified).AddTicks(6628), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -2,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 19, 47, 43, 743, DateTimeKind.Unspecified).AddTicks(6627), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -1,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2023, 12, 13, 19, 47, 43, 743, DateTimeKind.Unspecified).AddTicks(6618), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "edb9f7df-2489-4864-8a58-97470291f8f2", "ADMIN_SEED_ID" });
        }
    }
}
