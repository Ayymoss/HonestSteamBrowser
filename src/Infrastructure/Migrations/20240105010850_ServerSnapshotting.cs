using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BetterSteamBrowser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ServerSnapshotting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5eb3d78b-774d-450b-9738-6f4015f967ad", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5eb3d78b-774d-450b-9738-6f4015f967ad");

            migrationBuilder.DropColumn(
                name: "PlayerHistory",
                table: "EFServers");

            migrationBuilder.CreateTable(
                name: "EFServerSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SnapshotCount = table.Column<int>(type: "integer", nullable: false),
                    SnapshotTaken = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ServerHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFServerSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EFServerSnapshots_EFServers_ServerHash",
                        column: x => x.ServerHash,
                        principalTable: "EFServers",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_EFServerSnapshots_ServerHash",
                table: "EFServerSnapshots",
                column: "ServerHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EFServerSnapshots");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "6e972fb8-3976-47f8-aa03-c6456e8aa3c9", "ADMIN_SEED_ID" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e972fb8-3976-47f8-aa03-c6456e8aa3c9");

            migrationBuilder.AddColumn<List<int>>(
                name: "PlayerHistory",
                table: "EFServers",
                type: "integer[]",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5eb3d78b-774d-450b-9738-6f4015f967ad", null, "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN_SEED_ID",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c78e5e3b-fdc3-40cc-aaa1-126e0c6867ac", "AQAAAAIAAYagAAAAENwKB2oHPSc092owWNj75fw3KgMiZFmETuVjY2atJWweRlimsV3ez1NKpi4lbIpnSA==" });

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -9,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 4, 16, 37, 32, 564, DateTimeKind.Unspecified).AddTicks(1727), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -8,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 4, 16, 37, 32, 564, DateTimeKind.Unspecified).AddTicks(1726), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -7,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 4, 16, 37, 32, 564, DateTimeKind.Unspecified).AddTicks(1724), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -6,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 4, 16, 37, 32, 564, DateTimeKind.Unspecified).AddTicks(1723), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -5,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 4, 16, 37, 32, 564, DateTimeKind.Unspecified).AddTicks(1722), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -4,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 4, 16, 37, 32, 564, DateTimeKind.Unspecified).AddTicks(1721), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -3,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 4, 16, 37, 32, 564, DateTimeKind.Unspecified).AddTicks(1720), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -2,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 4, 16, 37, 32, 564, DateTimeKind.Unspecified).AddTicks(1719), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "EFBlocks",
                keyColumn: "Id",
                keyValue: -1,
                column: "Added",
                value: new DateTimeOffset(new DateTime(2024, 1, 4, 16, 37, 32, 564, DateTimeKind.Unspecified).AddTicks(1714), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "5eb3d78b-774d-450b-9738-6f4015f967ad", "ADMIN_SEED_ID" });
        }
    }
}
