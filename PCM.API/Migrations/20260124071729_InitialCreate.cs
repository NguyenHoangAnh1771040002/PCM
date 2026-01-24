using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PCM.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "002_Courts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_Courts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "002_Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RankLevel = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TotalMatches = table.Column<int>(type: "int", nullable: false),
                    WinMatches = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "002_News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "002_TransactionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_TransactionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "002_Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourtId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_002_Bookings_002_Courts_CourtId",
                        column: x => x.CourtId,
                        principalTable: "002_Courts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_002_Bookings_002_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "002_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "002_Challenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    GameMode = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Config_TargetWins = table.Column<int>(type: "int", nullable: true),
                    CurrentScore_TeamA = table.Column<int>(type: "int", nullable: false),
                    CurrentScore_TeamB = table.Column<int>(type: "int", nullable: false),
                    EntryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrizePool = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_Challenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_002_Challenges_002_Members_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "002_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "002_Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_002_Notifications_002_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "002_Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "002_Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_002_Transactions_002_Members_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "002_Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_002_Transactions_002_TransactionCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "002_TransactionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "002_Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRanked = table.Column<bool>(type: "bit", nullable: false),
                    ChallengeId = table.Column<int>(type: "int", nullable: true),
                    MatchFormat = table.Column<int>(type: "int", nullable: false),
                    Team1_Player1Id = table.Column<int>(type: "int", nullable: false),
                    Team1_Player2Id = table.Column<int>(type: "int", nullable: true),
                    Team2_Player1Id = table.Column<int>(type: "int", nullable: false),
                    Team2_Player2Id = table.Column<int>(type: "int", nullable: true),
                    WinningSide = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_002_Matches_002_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "002_Challenges",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_002_Matches_002_Members_Team1_Player1Id",
                        column: x => x.Team1_Player1Id,
                        principalTable: "002_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_002_Matches_002_Members_Team1_Player2Id",
                        column: x => x.Team1_Player2Id,
                        principalTable: "002_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_002_Matches_002_Members_Team2_Player1Id",
                        column: x => x.Team2_Player1Id,
                        principalTable: "002_Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_002_Matches_002_Members_Team2_Player2Id",
                        column: x => x.Team2_Player2Id,
                        principalTable: "002_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "002_Participants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChallengeId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Team = table.Column<int>(type: "int", nullable: false),
                    EntryFeePaid = table.Column<bool>(type: "bit", nullable: false),
                    EntryFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_002_Participants_002_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "002_Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_002_Participants_002_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "002_Members",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "002_MatchScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    SetNumber = table.Column<int>(type: "int", nullable: false),
                    Team1Score = table.Column<int>(type: "int", nullable: false),
                    Team2Score = table.Column<int>(type: "int", nullable: false),
                    IsFinalSet = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_002_MatchScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_002_MatchScores_002_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "002_Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "002_Courts",
                columns: new[] { "Id", "CreatedDate", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Sân chính - có đèn chiếu sáng", true, "Sân 1" },
                    { 2, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Sân phụ - có quạt mát", true, "Sân 2" },
                    { 3, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Đang bảo trì", false, "Sân 3" }
                });

            migrationBuilder.InsertData(
                table: "002_Members",
                columns: new[] { "Id", "CreatedDate", "DateOfBirth", "Email", "FullName", "IsActive", "JoinDate", "ModifiedDate", "PhoneNumber", "RankLevel", "TotalMatches", "UserId", "WinMatches" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "member1@pcm.com", "Nguyễn Văn An", true, new DateTime(2025, 12, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345600", 4.5, 20, "member-user-id-001", 15 },
                    { 2, new DateTime(2025, 11, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1991, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "member2@pcm.com", "Trần Thị Bình", true, new DateTime(2025, 11, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345601", 4.2000000000000002, 19, "member-user-id-002", 14 },
                    { 3, new DateTime(2025, 10, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1992, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "member3@pcm.com", "Lê Văn Cường", true, new DateTime(2025, 10, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345602", 4.0, 18, "member-user-id-003", 13 },
                    { 4, new DateTime(2025, 9, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1993, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "member4@pcm.com", "Phạm Thị Dung", true, new DateTime(2025, 9, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345603", 3.7999999999999998, 17, "member-user-id-004", 12 },
                    { 5, new DateTime(2025, 8, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1994, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "member5@pcm.com", "Hoàng Văn Em", true, new DateTime(2025, 8, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345604", 3.6000000000000001, 16, "member-user-id-005", 11 },
                    { 6, new DateTime(2025, 7, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1995, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "member6@pcm.com", "Vũ Thị Phương", true, new DateTime(2025, 7, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345605", 3.5, 15, "member-user-id-006", 10 },
                    { 7, new DateTime(2025, 6, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1996, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "member7@pcm.com", "Đặng Văn Giang", true, new DateTime(2025, 6, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345606", 3.3999999999999999, 14, "member-user-id-007", 9 },
                    { 8, new DateTime(2025, 5, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1997, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "member8@pcm.com", "Bùi Thị Hoa", true, new DateTime(2025, 5, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345607", 3.2000000000000002, 13, "member-user-id-008", 8 },
                    { 9, new DateTime(2025, 4, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1998, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "member9@pcm.com", "Ngô Văn Khải", true, new DateTime(2025, 4, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345608", 3.0, 12, "member-user-id-009", 7 },
                    { 10, new DateTime(2025, 3, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "member10@pcm.com", "Lý Thị Lan", true, new DateTime(2025, 3, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345609", 2.7999999999999998, 11, "member-user-id-010", 6 },
                    { 11, new DateTime(2025, 2, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "member11@pcm.com", "Đinh Văn Minh", true, new DateTime(2025, 2, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345610", 2.5, 10, "member-user-id-011", 5 },
                    { 12, new DateTime(2025, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1991, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "member12@pcm.com", "Trương Thị Ngọc", true, new DateTime(2025, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "09012345611", 2.0, 9, "member-user-id-012", 4 }
                });

            migrationBuilder.InsertData(
                table: "002_News",
                columns: new[] { "Id", "Content", "CreatedDate", "IsPinned", "ModifiedDate", "Title" },
                values: new object[,]
                {
                    { 1, "CLB tổ chức giải đấu Team Battle với Entry Fee 50.000đ/người. Thời gian: 25-26/01/2026. Đăng ký ngay!", new DateTime(2026, 1, 19, 12, 0, 0, 0, DateTimeKind.Unspecified), true, null, "🏆 Giải đấu Mini-game Team Battle tháng 1/2026" },
                    { 2, "Các thành viên vui lòng đóng quỹ tháng 1/2026 (100.000đ) trước ngày 31/01. Liên hệ Thủ quỹ để chuyển khoản.", new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), true, null, "📢 Thông báo đóng quỹ tháng 1/2026" },
                    { 3, "Chúc mừng An, Bình, Cường, Dung, Em đã đạt Top 5 Rank DUPR cao nhất tháng 12/2025!", new DateTime(2026, 1, 14, 12, 0, 0, 0, DateTimeKind.Unspecified), false, null, "🎉 Vinh danh Top 5 cao thủ tháng 12/2025" },
                    { 4, "Sân 3 sẽ tạm đóng để sửa chữa mặt sân và thay đèn. Các bạn vui lòng đặt sân 1 hoặc sân 2.", new DateTime(2026, 1, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), false, null, "🔧 Bảo trì Sân 3 từ 20-30/01/2026" }
                });

            migrationBuilder.InsertData(
                table: "002_TransactionCategories",
                columns: new[] { "Id", "CreatedDate", "Name", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Tiền sân", 1 },
                    { 2, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Quỹ tháng", 1 },
                    { 3, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Tiền thưởng từ giải", 1 },
                    { 4, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Tài trợ", 1 },
                    { 5, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Nước", 2 },
                    { 6, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Phạt", 2 },
                    { 7, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Mua bóng", 2 },
                    { 8, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Unspecified), "Sửa chữa", 2 }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "c2bfbc9b-f2d2-491b-8094-c8609203651f", "Admin", "ADMIN" },
                    { "2", "8f146224-6d31-4cc3-be8b-96b6b5cd118b", "Treasurer", "TREASURER" },
                    { "3", "35cf0b28-9a0d-46d4-9bce-6b97334bc3ed", "Referee", "REFEREE" },
                    { "4", "16b99ea8-460a-46b8-b0be-f9ff881da59e", "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "admin-user-id-001", 0, "admin-concurrency-stamp-001", "admin@pcm.com", true, false, null, "ADMIN@PCM.COM", "ADMIN@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "ADMIN-SECURITY-STAMP-001", false, "admin@pcm.com" },
                    { "member-user-id-001", 0, "member-concurrency-stamp-001", "member1@pcm.com", true, false, null, "MEMBER1@PCM.COM", "MEMBER1@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-001", false, "member1@pcm.com" },
                    { "member-user-id-002", 0, "member-concurrency-stamp-002", "member2@pcm.com", true, false, null, "MEMBER2@PCM.COM", "MEMBER2@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-002", false, "member2@pcm.com" },
                    { "member-user-id-003", 0, "member-concurrency-stamp-003", "member3@pcm.com", true, false, null, "MEMBER3@PCM.COM", "MEMBER3@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-003", false, "member3@pcm.com" },
                    { "member-user-id-004", 0, "member-concurrency-stamp-004", "member4@pcm.com", true, false, null, "MEMBER4@PCM.COM", "MEMBER4@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-004", false, "member4@pcm.com" },
                    { "member-user-id-005", 0, "member-concurrency-stamp-005", "member5@pcm.com", true, false, null, "MEMBER5@PCM.COM", "MEMBER5@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-005", false, "member5@pcm.com" },
                    { "member-user-id-006", 0, "member-concurrency-stamp-006", "member6@pcm.com", true, false, null, "MEMBER6@PCM.COM", "MEMBER6@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-006", false, "member6@pcm.com" },
                    { "member-user-id-007", 0, "member-concurrency-stamp-007", "member7@pcm.com", true, false, null, "MEMBER7@PCM.COM", "MEMBER7@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-007", false, "member7@pcm.com" },
                    { "member-user-id-008", 0, "member-concurrency-stamp-008", "member8@pcm.com", true, false, null, "MEMBER8@PCM.COM", "MEMBER8@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-008", false, "member8@pcm.com" },
                    { "member-user-id-009", 0, "member-concurrency-stamp-009", "member9@pcm.com", true, false, null, "MEMBER9@PCM.COM", "MEMBER9@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-009", false, "member9@pcm.com" },
                    { "member-user-id-010", 0, "member-concurrency-stamp-010", "member10@pcm.com", true, false, null, "MEMBER10@PCM.COM", "MEMBER10@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-010", false, "member10@pcm.com" },
                    { "member-user-id-011", 0, "member-concurrency-stamp-011", "member11@pcm.com", true, false, null, "MEMBER11@PCM.COM", "MEMBER11@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-011", false, "member11@pcm.com" },
                    { "member-user-id-012", 0, "member-concurrency-stamp-012", "member12@pcm.com", true, false, null, "MEMBER12@PCM.COM", "MEMBER12@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "MEMBER-SECURITY-STAMP-012", false, "member12@pcm.com" },
                    { "referee-user-id-001", 0, "referee-concurrency-stamp-001", "referee@pcm.com", true, false, null, "REFEREE@PCM.COM", "REFEREE@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "REFEREE-SECURITY-STAMP-001", false, "referee@pcm.com" },
                    { "treasurer-user-id-001", 0, "treasurer-concurrency-stamp-001", "treasurer@pcm.com", true, false, null, "TREASURER@PCM.COM", "TREASURER@PCM.COM", "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==", null, false, "TREASURER-SECURITY-STAMP-001", false, "treasurer@pcm.com" }
                });

            migrationBuilder.InsertData(
                table: "002_Challenges",
                columns: new[] { "Id", "Config_TargetWins", "CreatedById", "CreatedDate", "CurrentScore_TeamA", "CurrentScore_TeamB", "Description", "EndDate", "EntryFee", "GameMode", "ModifiedDate", "PrizePool", "StartDate", "Status", "Title", "Type" },
                values: new object[,]
                {
                    { 1, 5, 1, new DateTime(2026, 1, 19, 12, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, "Giải đấu chia 2 phe A-B, đánh chạm mốc 5 trận thắng.", new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Unspecified), 50000m, 1, null, 500000m, new DateTime(2026, 1, 22, 12, 0, 0, 0, DateTimeKind.Unspecified), 2, "Giải Mini-game Team Battle tháng 1/2026", 2 },
                    { 2, null, 1, new DateTime(2026, 1, 23, 12, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, "Kèo vui vẻ, thua mời nước!", null, 0m, 0, null, 0m, null, 1, "Kèo thách đấu: An vs Cường", 1 }
                });

            migrationBuilder.InsertData(
                table: "002_Matches",
                columns: new[] { "Id", "ChallengeId", "CreatedDate", "Date", "IsRanked", "MatchFormat", "ModifiedDate", "Team1_Player1Id", "Team1_Player2Id", "Team2_Player1Id", "Team2_Player2Id", "WinningSide" },
                values: new object[] { 4, null, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), true, 1, null, 1, null, 3, null, 1 });

            migrationBuilder.InsertData(
                table: "002_Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "CreatedById", "CreatedDate", "Date", "Description" },
                values: new object[,]
                {
                    { 1, 1000000m, 2, 1, new DateTime(2025, 12, 25, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 25, 12, 0, 0, 0, DateTimeKind.Unspecified), "Thu quỹ tháng 12/2025" },
                    { 2, 500000m, 1, 1, new DateTime(2025, 12, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), "Thu tiền sân tuần 1" },
                    { 3, 300000m, 4, 1, new DateTime(2026, 1, 4, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 4, 12, 0, 0, 0, DateTimeKind.Unspecified), "Tài trợ từ shop bóng" },
                    { 4, 200000m, 5, 1, new DateTime(2025, 12, 27, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 27, 12, 0, 0, 0, DateTimeKind.Unspecified), "Mua nước cho hội viên" },
                    { 5, 150000m, 7, 1, new DateTime(2026, 1, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), "Mua bóng mới" },
                    { 6, 100000m, 8, 1, new DateTime(2026, 1, 9, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 9, 12, 0, 0, 0, DateTimeKind.Unspecified), "Sửa vợt CLB" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1", "admin-user-id-001" },
                    { "4", "member-user-id-001" },
                    { "4", "member-user-id-002" },
                    { "4", "member-user-id-003" },
                    { "4", "member-user-id-004" },
                    { "4", "member-user-id-005" },
                    { "4", "member-user-id-006" },
                    { "4", "member-user-id-007" },
                    { "4", "member-user-id-008" },
                    { "4", "member-user-id-009" },
                    { "4", "member-user-id-010" },
                    { "4", "member-user-id-011" },
                    { "4", "member-user-id-012" },
                    { "3", "referee-user-id-001" },
                    { "2", "treasurer-user-id-001" }
                });

            migrationBuilder.InsertData(
                table: "002_Matches",
                columns: new[] { "Id", "ChallengeId", "CreatedDate", "Date", "IsRanked", "MatchFormat", "ModifiedDate", "Team1_Player1Id", "Team1_Player2Id", "Team2_Player1Id", "Team2_Player2Id", "WinningSide" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 23, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 23, 12, 0, 0, 0, DateTimeKind.Unspecified), true, 1, null, 1, null, 6, null, 1 },
                    { 2, 1, new DateTime(2026, 1, 23, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 23, 14, 0, 0, 0, DateTimeKind.Unspecified), true, 1, null, 2, null, 7, null, 1 },
                    { 3, 1, new DateTime(2026, 1, 24, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 24, 7, 0, 0, 0, DateTimeKind.Unspecified), true, 2, null, 3, 4, 8, 9, 2 }
                });

            migrationBuilder.InsertData(
                table: "002_Participants",
                columns: new[] { "Id", "ChallengeId", "EntryFeeAmount", "EntryFeePaid", "JoinedDate", "MemberId", "Status", "Team" },
                values: new object[,]
                {
                    { 1, 1, 50000m, true, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 1 },
                    { 2, 1, 50000m, true, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 1 },
                    { 3, 1, 50000m, true, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, 1 },
                    { 4, 1, 50000m, true, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 4, 2, 1 },
                    { 5, 1, 50000m, true, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, 1 },
                    { 6, 1, 50000m, true, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 6, 2, 2 },
                    { 7, 1, 50000m, true, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 7, 2, 2 },
                    { 8, 1, 50000m, true, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 8, 2, 2 },
                    { 9, 1, 50000m, true, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 9, 2, 2 },
                    { 10, 1, 50000m, true, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 10, 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_002_Bookings_CourtId",
                table: "002_Bookings",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_002_Bookings_MemberId",
                table: "002_Bookings",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_002_Challenges_CreatedById",
                table: "002_Challenges",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_002_Matches_ChallengeId",
                table: "002_Matches",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_002_Matches_Team1_Player1Id",
                table: "002_Matches",
                column: "Team1_Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_002_Matches_Team1_Player2Id",
                table: "002_Matches",
                column: "Team1_Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_002_Matches_Team2_Player1Id",
                table: "002_Matches",
                column: "Team2_Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_002_Matches_Team2_Player2Id",
                table: "002_Matches",
                column: "Team2_Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_002_MatchScores_MatchId",
                table: "002_MatchScores",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_002_Notifications_MemberId",
                table: "002_Notifications",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_002_Participants_ChallengeId",
                table: "002_Participants",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_002_Participants_MemberId",
                table: "002_Participants",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_002_Transactions_CategoryId",
                table: "002_Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_002_Transactions_CreatedById",
                table: "002_Transactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "002_Bookings");

            migrationBuilder.DropTable(
                name: "002_MatchScores");

            migrationBuilder.DropTable(
                name: "002_News");

            migrationBuilder.DropTable(
                name: "002_Notifications");

            migrationBuilder.DropTable(
                name: "002_Participants");

            migrationBuilder.DropTable(
                name: "002_Transactions");

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
                name: "002_Courts");

            migrationBuilder.DropTable(
                name: "002_Matches");

            migrationBuilder.DropTable(
                name: "002_TransactionCategories");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "002_Challenges");

            migrationBuilder.DropTable(
                name: "002_Members");
        }
    }
}
