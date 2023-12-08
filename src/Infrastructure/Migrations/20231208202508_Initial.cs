using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BetterSteamBrowser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EFSteamGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFSteamGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EFBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ApiFilter = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Added = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SteamGameId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EFBlocks_EFSteamGames_SteamGameId",
                        column: x => x.SteamGameId,
                        principalTable: "EFSteamGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EFServers",
                columns: table => new
                {
                    Hash = table.Column<string>(type: "text", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Players = table.Column<int>(type: "integer", nullable: false),
                    MaxPlayers = table.Column<int>(type: "integer", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    Map = table.Column<string>(type: "text", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Blocked = table.Column<bool>(type: "boolean", nullable: false),
                    SteamGameId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFServers", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_EFServers_EFSteamGames_SteamGameId",
                        column: x => x.SteamGameId,
                        principalTable: "EFSteamGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EFFavourites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServerId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EFFavourites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EFFavourites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EFFavourites_EFServers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "EFServers",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fea5f7a9-b2e4-4e12-9790-977ca690cc89", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ADMIN_SEED_ID", 0, "5d0316ab-6a64-41f3-b1d7-50f88e465c43", "superadmin@example.com", true, false, null, "SUPERADMIN@EXAMPLE.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAELnQmDSM9pPHpB+9exdpzz5E11W0lGEJmrnXXYSdkBreKDWCG0iItB/9qRQ/iCFmCw==", null, false, "", false, "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "EFSteamGames",
                columns: new[] { "Id", "AppId", "Name" },
                values: new object[,]
                {
                    { -2, -2, "All Games" },
                    { -1, -1, "Unknown" },
                    { 1, 10, "Counter-Strike" },
                    { 2, 30, "Day of Defeat" },
                    { 3, 50, "Half-Life: Opposing Force" },
                    { 4, 70, "Half-Life" },
                    { 5, 80, "Condition Zero" },
                    { 6, 240, "Counter-Strike: Source" },
                    { 7, 300, "Day of Defeat: Source" },
                    { 8, 320, "Half-Life 2: Deathmatch" },
                    { 9, 440, "Team Fortress 2" },
                    { 10, 500, "Left 4 Dead" },
                    { 11, 550, "Left 4 Dead 2" },
                    { 12, 730, "Counter-Strike 2" },
                    { 13, 1250, "Killing Floor" },
                    { 14, 4000, "Garry's Mod" },
                    { 15, 4920, "Natural Selection 2" },
                    { 16, 17520, "Synergy" },
                    { 17, 17550, "Eternal Silence" },
                    { 18, 33930, "Arma 2: Operation Arrowhead" },
                    { 19, 107410, "Arma 3" },
                    { 20, 108600, "Project Zomboid" },
                    { 21, 221100, "DayZ" },
                    { 22, 222880, "Insurgency" },
                    { 23, 232090, "Killing Floor 2" },
                    { 24, 242760, "The Forest" },
                    { 25, 246900, "Viscera Cleanup Detail" },
                    { 26, 251570, "7 Days to Die" },
                    { 27, 252490, "Rust" },
                    { 28, 304930, "Unturned" },
                    { 29, 311210, "Call of Duty: Black Ops III" },
                    { 30, 312660, "Sniper Elite 4" },
                    { 31, 393380, "Squad" },
                    { 32, 394690, "Tower Unite" },
                    { 33, 466560, "Northgard" },
                    { 34, 632360, "Risk of Rain 2" },
                    { 35, 686810, "Hell Let Loose" },
                    { 36, 1604030, "V Rising" },
                    { 37, 346110, "ARK: Survival Evolved" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "fea5f7a9-b2e4-4e12-9790-977ca690cc89", "ADMIN_SEED_ID" });

            migrationBuilder.InsertData(
                table: "EFBlocks",
                columns: new[] { "Id", "Added", "ApiFilter", "SteamGameId", "Type", "UserId", "Value" },
                values: new object[,]
                {
                    { -9, new DateTimeOffset(new DateTime(2023, 12, 8, 20, 25, 7, 727, DateTimeKind.Unspecified).AddTicks(8748), new TimeSpan(0, 0, 0, 0, 0)), false, 12, 2, "ADMIN_SEED_ID", "Counter-Strike 2" },
                    { -8, new DateTimeOffset(new DateTime(2023, 12, 8, 20, 25, 7, 727, DateTimeKind.Unspecified).AddTicks(8747), new TimeSpan(0, 0, 0, 0, 0)), false, 27, 3, "ADMIN_SEED_ID", "RU" },
                    { -7, new DateTimeOffset(new DateTime(2023, 12, 8, 20, 25, 7, 727, DateTimeKind.Unspecified).AddTicks(8745), new TimeSpan(0, 0, 0, 0, 0)), false, 12, 3, "ADMIN_SEED_ID", "RU" },
                    { -6, new DateTimeOffset(new DateTime(2023, 12, 8, 20, 25, 7, 727, DateTimeKind.Unspecified).AddTicks(8679), new TimeSpan(0, 0, 0, 0, 0)), false, -2, 2, "ADMIN_SEED_ID", "FACEIT" },
                    { -5, new DateTimeOffset(new DateTime(2023, 12, 8, 20, 25, 7, 727, DateTimeKind.Unspecified).AddTicks(8678), new TimeSpan(0, 0, 0, 0, 0)), false, -2, 2, "ADMIN_SEED_ID", "Develop" },
                    { -4, new DateTimeOffset(new DateTime(2023, 12, 8, 20, 25, 7, 727, DateTimeKind.Unspecified).AddTicks(8677), new TimeSpan(0, 0, 0, 0, 0)), true, -2, 1, "ADMIN_SEED_ID", "no-steam" },
                    { -3, new DateTimeOffset(new DateTime(2023, 12, 8, 20, 25, 7, 727, DateTimeKind.Unspecified).AddTicks(8676), new TimeSpan(0, 0, 0, 0, 0)), true, -2, 1, "ADMIN_SEED_ID", "nosteam" },
                    { -2, new DateTimeOffset(new DateTime(2023, 12, 8, 20, 25, 7, 727, DateTimeKind.Unspecified).AddTicks(8674), new TimeSpan(0, 0, 0, 0, 0)), true, 12, 1, "ADMIN_SEED_ID", "uwujka" },
                    { -1, new DateTimeOffset(new DateTime(2023, 12, 8, 20, 25, 7, 727, DateTimeKind.Unspecified).AddTicks(8666), new TimeSpan(0, 0, 0, 0, 0)), false, -2, 2, "ADMIN_SEED_ID", "FASTCUP" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EFBlocks_SteamGameId",
                table: "EFBlocks",
                column: "SteamGameId");

            migrationBuilder.CreateIndex(
                name: "IX_EFFavourites_ServerId",
                table: "EFFavourites",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_EFFavourites_UserId",
                table: "EFFavourites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EFServers_SteamGameId",
                table: "EFServers",
                column: "SteamGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EFBlocks");

            migrationBuilder.DropTable(
                name: "EFFavourites");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EFServers");

            migrationBuilder.DropTable(
                name: "EFSteamGames");
        }
    }
}
