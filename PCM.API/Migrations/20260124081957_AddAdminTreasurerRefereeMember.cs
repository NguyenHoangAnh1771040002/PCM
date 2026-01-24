using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PCM.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminTreasurerRefereeMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "002_Matches",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Team1_Player1Id", "Team2_Player1Id" },
                values: new object[] { 4, 9 });

            migrationBuilder.UpdateData(
                table: "002_Matches",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Team1_Player1Id", "Team2_Player1Id" },
                values: new object[] { 5, 10 });

            migrationBuilder.UpdateData(
                table: "002_Matches",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Team1_Player1Id", "Team1_Player2Id", "Team2_Player1Id", "Team2_Player2Id" },
                values: new object[] { 6, 7, 11, 12 });

            migrationBuilder.UpdateData(
                table: "002_Matches",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Team1_Player1Id", "Team2_Player1Id" },
                values: new object[] { 4, 6 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2021, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@pcm.com", "Admin CLB", new DateTime(2021, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "0901234500", 5.0, 100, "admin-user-id-001", 80 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2023, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1988, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "treasurer@pcm.com", "Thủ quỹ CLB", new DateTime(2023, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "0901234501", 4.0, 50, "treasurer-user-id-001", 35 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2024, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "referee@pcm.com", "Trọng tài CLB", new DateTime(2024, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "0901234502", 4.5, 30, "referee-user-id-001", 20 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 12, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "member1@pcm.com", "Nguyễn Văn An", new DateTime(2025, 12, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345600", 4.5, 20, "member-user-id-001", 15 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 11, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1991, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "member2@pcm.com", "Trần Thị Bình", new DateTime(2025, 11, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345601", 4.2000000000000002, 19, "member-user-id-002", 14 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 10, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1992, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "member3@pcm.com", "Lê Văn Cường", new DateTime(2025, 10, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345602", 4.0, 18, "member-user-id-003", 13 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 9, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1993, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "member4@pcm.com", "Phạm Thị Dung", new DateTime(2025, 9, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345603", 3.7999999999999998, 17, "member-user-id-004", 12 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 8, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1994, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "member5@pcm.com", "Hoàng Văn Em", new DateTime(2025, 8, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345604", 3.6000000000000001, 16, "member-user-id-005", 11 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 7, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1995, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "member6@pcm.com", "Vũ Thị Phương", new DateTime(2025, 7, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345605", 3.5, 15, "member-user-id-006", 10 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 6, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1996, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "member7@pcm.com", "Đặng Văn Giang", new DateTime(2025, 6, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345606", 3.3999999999999999, 14, "member-user-id-007", 9 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 5, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1997, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "member8@pcm.com", "Bùi Thị Hoa", new DateTime(2025, 5, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345607", 3.2000000000000002, 13, "member-user-id-008", 8 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 4, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1998, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "member9@pcm.com", "Ngô Văn Khải", new DateTime(2025, 4, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345608", 3.0, 12, "member-user-id-009", 7 });

            migrationBuilder.InsertData(
                table: "002_Members",
                columns: new[] { "Id", "CreatedDate", "DateOfBirth", "Email", "FullName", "IsActive", "JoinDate", "ModifiedDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[,]
                {
                    { 13, new DateTime(2025, 3, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "member10@pcm.com", "Lý Thị Lan", true, new DateTime(2025, 3, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345609", 2.7999999999999998, 11, "member-user-id-010", 6 },
                    { 14, new DateTime(2025, 2, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "member11@pcm.com", "Đinh Văn Minh", true, new DateTime(2025, 2, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345610", 2.5, 10, "member-user-id-011", 5 },
                    { 15, new DateTime(2025, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1991, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "member12@pcm.com", "Trương Thị Ngọc", true, new DateTime(2025, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345611", 2.0, 9, "member-user-id-012", 4 }
                });

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 1,
                column: "MemberId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 2,
                column: "MemberId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 3,
                column: "MemberId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 4,
                column: "MemberId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 5,
                column: "MemberId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 6,
                column: "MemberId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 7,
                column: "MemberId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 8,
                column: "MemberId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 9,
                column: "MemberId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 10,
                column: "MemberId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "f190ffb7-883a-4707-9064-43ce6711d788");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "7327b048-6b69-435a-a9ca-fbbb7187c4ea");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "d8f33f44-63e8-491f-95c1-efb8ee00e30b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "226c8c82-6b31-4bb3-8406-81fe67c34f5c");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "002_Matches",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Team1_Player1Id", "Team2_Player1Id" },
                values: new object[] { 1, 6 });

            migrationBuilder.UpdateData(
                table: "002_Matches",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Team1_Player1Id", "Team2_Player1Id" },
                values: new object[] { 2, 7 });

            migrationBuilder.UpdateData(
                table: "002_Matches",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Team1_Player1Id", "Team1_Player2Id", "Team2_Player1Id", "Team2_Player2Id" },
                values: new object[] { 3, 4, 8, 9 });

            migrationBuilder.UpdateData(
                table: "002_Matches",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Team1_Player1Id", "Team2_Player1Id" },
                values: new object[] { 1, 3 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 12, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "member1@pcm.com", "Nguyễn Văn An", new DateTime(2025, 12, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345600", 4.5, 20, "member-user-id-001", 15 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 11, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1991, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "member2@pcm.com", "Trần Thị Bình", new DateTime(2025, 11, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345601", 4.2000000000000002, 19, "member-user-id-002", 14 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 10, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1992, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "member3@pcm.com", "Lê Văn Cường", new DateTime(2025, 10, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345602", 4.0, 18, "member-user-id-003", 13 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 9, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1993, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "member4@pcm.com", "Phạm Thị Dung", new DateTime(2025, 9, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345603", 3.7999999999999998, 17, "member-user-id-004", 12 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 8, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1994, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "member5@pcm.com", "Hoàng Văn Em", new DateTime(2025, 8, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345604", 3.6000000000000001, 16, "member-user-id-005", 11 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 7, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1995, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "member6@pcm.com", "Vũ Thị Phương", new DateTime(2025, 7, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345605", 3.5, 15, "member-user-id-006", 10 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 6, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1996, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "member7@pcm.com", "Đặng Văn Giang", new DateTime(2025, 6, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345606", 3.3999999999999999, 14, "member-user-id-007", 9 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 5, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1997, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "member8@pcm.com", "Bùi Thị Hoa", new DateTime(2025, 5, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345607", 3.2000000000000002, 13, "member-user-id-008", 8 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 4, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1998, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "member9@pcm.com", "Ngô Văn Khải", new DateTime(2025, 4, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345608", 3.0, 12, "member-user-id-009", 7 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 3, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "member10@pcm.com", "Lý Thị Lan", new DateTime(2025, 3, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345609", 2.7999999999999998, 11, "member-user-id-010", 6 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 2, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "member11@pcm.com", "Đinh Văn Minh", new DateTime(2025, 2, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345610", 2.5, 10, "member-user-id-011", 5 });

            migrationBuilder.UpdateData(
                table: "002_Members",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "DateOfBirth", "Email", "FullName", "JoinDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[] { new DateTime(2025, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1991, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "member12@pcm.com", "Trương Thị Ngọc", new DateTime(2025, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "09012345611", 2.0, 9, "member-user-id-012", 4 });

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 1,
                column: "MemberId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 2,
                column: "MemberId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 3,
                column: "MemberId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 4,
                column: "MemberId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 5,
                column: "MemberId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 6,
                column: "MemberId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 7,
                column: "MemberId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 8,
                column: "MemberId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 9,
                column: "MemberId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "002_Participants",
                keyColumn: "Id",
                keyValue: 10,
                column: "MemberId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "c2bfbc9b-f2d2-491b-8094-c8609203651f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "8f146224-6d31-4cc3-be8b-96b6b5cd118b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "35cf0b28-9a0d-46d4-9bce-6b97334bc3ed");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "16b99ea8-460a-46b8-b0be-f9ff881da59e");
        }
    }
}
