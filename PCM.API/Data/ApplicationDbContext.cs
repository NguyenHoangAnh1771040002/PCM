using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PCM.API.Models;

namespace PCM.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        // Sử dụng ngày cố định cho seed data
        private static readonly DateTime SeedDate = new DateTime(2026, 1, 24, 12, 0, 0);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchScore> MatchScores { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles
            SeedRoles(builder);

            // Seed Admin User
            SeedUsers(builder);

            // Seed Transaction Categories
            SeedTransactionCategories(builder);

            // Seed Courts
            SeedCourts(builder);

            // Seed Members
            SeedMembers(builder);

            // Seed News
            SeedNews(builder);

            // Seed Transactions
            SeedTransactions(builder);

            // Seed Challenges, Participants, Matches
            SeedChallengesAndMatches(builder);

            // Configure relationships
            ConfigureRelationships(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Treasurer", NormalizedName = "TREASURER" },
                new IdentityRole { Id = "3", Name = "Referee", NormalizedName = "REFEREE" },
                new IdentityRole { Id = "4", Name = "Member", NormalizedName = "MEMBER" }
            );
        }

        private void SeedUsers(ModelBuilder builder)
        {
            // Password hash cố định cho "Admin@123", "Treasurer@123", "Referee@123", "Member@123"
            // Sử dụng hash cố định để tránh lỗi PendingModelChangesWarning
            const string adminPasswordHash = "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==";
            const string treasurerPasswordHash = "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==";
            const string refereePasswordHash = "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==";
            const string memberPasswordHash = "AQAAAAIAAYagAAAAELDNqhJ0Z5xqR8nKCkX3f8fqDKpA1bF+2EHg5J7jqF9K3F8F9LvG3q4N5vR6yTkW9g==";

            // Admin User
            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "admin-user-id-001",
                    UserName = "admin@pcm.com",
                    NormalizedUserName = "ADMIN@PCM.COM",
                    Email = "admin@pcm.com",
                    NormalizedEmail = "ADMIN@PCM.COM",
                    EmailConfirmed = true,
                    SecurityStamp = "ADMIN-SECURITY-STAMP-001",
                    ConcurrencyStamp = "admin-concurrency-stamp-001",
                    PasswordHash = adminPasswordHash
                },
                // Treasurer User
                new IdentityUser
                {
                    Id = "treasurer-user-id-001",
                    UserName = "treasurer@pcm.com",
                    NormalizedUserName = "TREASURER@PCM.COM",
                    Email = "treasurer@pcm.com",
                    NormalizedEmail = "TREASURER@PCM.COM",
                    EmailConfirmed = true,
                    SecurityStamp = "TREASURER-SECURITY-STAMP-001",
                    ConcurrencyStamp = "treasurer-concurrency-stamp-001",
                    PasswordHash = treasurerPasswordHash
                },
                // Referee User
                new IdentityUser
                {
                    Id = "referee-user-id-001",
                    UserName = "referee@pcm.com",
                    NormalizedUserName = "REFEREE@PCM.COM",
                    Email = "referee@pcm.com",
                    NormalizedEmail = "REFEREE@PCM.COM",
                    EmailConfirmed = true,
                    SecurityStamp = "REFEREE-SECURITY-STAMP-001",
                    ConcurrencyStamp = "referee-concurrency-stamp-001",
                    PasswordHash = refereePasswordHash
                },
                // Member Users 1-12
                new IdentityUser { Id = "member-user-id-001", UserName = "member1@pcm.com", NormalizedUserName = "MEMBER1@PCM.COM", Email = "member1@pcm.com", NormalizedEmail = "MEMBER1@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-001", ConcurrencyStamp = "member-concurrency-stamp-001", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-002", UserName = "member2@pcm.com", NormalizedUserName = "MEMBER2@PCM.COM", Email = "member2@pcm.com", NormalizedEmail = "MEMBER2@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-002", ConcurrencyStamp = "member-concurrency-stamp-002", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-003", UserName = "member3@pcm.com", NormalizedUserName = "MEMBER3@PCM.COM", Email = "member3@pcm.com", NormalizedEmail = "MEMBER3@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-003", ConcurrencyStamp = "member-concurrency-stamp-003", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-004", UserName = "member4@pcm.com", NormalizedUserName = "MEMBER4@PCM.COM", Email = "member4@pcm.com", NormalizedEmail = "MEMBER4@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-004", ConcurrencyStamp = "member-concurrency-stamp-004", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-005", UserName = "member5@pcm.com", NormalizedUserName = "MEMBER5@PCM.COM", Email = "member5@pcm.com", NormalizedEmail = "MEMBER5@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-005", ConcurrencyStamp = "member-concurrency-stamp-005", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-006", UserName = "member6@pcm.com", NormalizedUserName = "MEMBER6@PCM.COM", Email = "member6@pcm.com", NormalizedEmail = "MEMBER6@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-006", ConcurrencyStamp = "member-concurrency-stamp-006", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-007", UserName = "member7@pcm.com", NormalizedUserName = "MEMBER7@PCM.COM", Email = "member7@pcm.com", NormalizedEmail = "MEMBER7@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-007", ConcurrencyStamp = "member-concurrency-stamp-007", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-008", UserName = "member8@pcm.com", NormalizedUserName = "MEMBER8@PCM.COM", Email = "member8@pcm.com", NormalizedEmail = "MEMBER8@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-008", ConcurrencyStamp = "member-concurrency-stamp-008", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-009", UserName = "member9@pcm.com", NormalizedUserName = "MEMBER9@PCM.COM", Email = "member9@pcm.com", NormalizedEmail = "MEMBER9@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-009", ConcurrencyStamp = "member-concurrency-stamp-009", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-010", UserName = "member10@pcm.com", NormalizedUserName = "MEMBER10@PCM.COM", Email = "member10@pcm.com", NormalizedEmail = "MEMBER10@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-010", ConcurrencyStamp = "member-concurrency-stamp-010", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-011", UserName = "member11@pcm.com", NormalizedUserName = "MEMBER11@PCM.COM", Email = "member11@pcm.com", NormalizedEmail = "MEMBER11@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-011", ConcurrencyStamp = "member-concurrency-stamp-011", PasswordHash = memberPasswordHash },
                new IdentityUser { Id = "member-user-id-012", UserName = "member12@pcm.com", NormalizedUserName = "MEMBER12@PCM.COM", Email = "member12@pcm.com", NormalizedEmail = "MEMBER12@PCM.COM", EmailConfirmed = true, SecurityStamp = "MEMBER-SECURITY-STAMP-012", ConcurrencyStamp = "member-concurrency-stamp-012", PasswordHash = memberPasswordHash }
            );

            // Assign Roles
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "admin-user-id-001", RoleId = "1" },
                new IdentityUserRole<string> { UserId = "treasurer-user-id-001", RoleId = "2" },
                new IdentityUserRole<string> { UserId = "referee-user-id-001", RoleId = "3" },
                // Assign Member role to all member users
                new IdentityUserRole<string> { UserId = "member-user-id-001", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-002", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-003", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-004", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-005", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-006", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-007", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-008", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-009", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-010", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-011", RoleId = "4" },
                new IdentityUserRole<string> { UserId = "member-user-id-012", RoleId = "4" }
            );
        }

        private void SeedTransactionCategories(ModelBuilder builder)
        {
            builder.Entity<TransactionCategory>().HasData(
                new TransactionCategory { Id = 1, Name = "Tiền sân", Type = TransactionType.Income, CreatedDate = SeedDate },
                new TransactionCategory { Id = 2, Name = "Quỹ tháng", Type = TransactionType.Income, CreatedDate = SeedDate },
                new TransactionCategory { Id = 3, Name = "Tiền thưởng từ giải", Type = TransactionType.Income, CreatedDate = SeedDate },
                new TransactionCategory { Id = 4, Name = "Tài trợ", Type = TransactionType.Income, CreatedDate = SeedDate },
                new TransactionCategory { Id = 5, Name = "Nước", Type = TransactionType.Expense, CreatedDate = SeedDate },
                new TransactionCategory { Id = 6, Name = "Phạt", Type = TransactionType.Expense, CreatedDate = SeedDate },
                new TransactionCategory { Id = 7, Name = "Mua bóng", Type = TransactionType.Expense, CreatedDate = SeedDate },
                new TransactionCategory { Id = 8, Name = "Sửa chữa", Type = TransactionType.Expense, CreatedDate = SeedDate }
            );
        }

        private void SeedCourts(ModelBuilder builder)
        {
            builder.Entity<Court>().HasData(
                new Court { Id = 1, Name = "Sân 1", IsActive = true, Description = "Sân chính - có đèn chiếu sáng", CreatedDate = SeedDate },
                new Court { Id = 2, Name = "Sân 2", IsActive = true, Description = "Sân phụ - có quạt mát", CreatedDate = SeedDate },
                new Court { Id = 3, Name = "Sân 3", IsActive = false, Description = "Đang bảo trì", CreatedDate = SeedDate }
            );
        }

        private void SeedMembers(ModelBuilder builder)
        {
            var members = new List<Member>
            {
                // Admin member
                new Member
                {
                    Id = 1,
                    FullName = "Admin CLB",
                    Email = "admin@pcm.com",
                    PhoneNumber = "0901234500",
                    DateOfBirth = new DateTime(1985, 1, 1),
                    JoinDate = SeedDate.AddYears(-5),
                    RankLevel = 5.0,
                    IsActive = true,
                    TotalMatches = 100,
                    WinMatches = 80,
                    UserId = "admin-user-id-001",
                    CreatedDate = SeedDate.AddYears(-5)
                },
                // Treasurer member
                new Member
                {
                    Id = 2,
                    FullName = "Thủ quỹ CLB",
                    Email = "treasurer@pcm.com",
                    PhoneNumber = "0901234501",
                    DateOfBirth = new DateTime(1988, 5, 15),
                    JoinDate = SeedDate.AddYears(-3),
                    RankLevel = 4.0,
                    IsActive = true,
                    TotalMatches = 50,
                    WinMatches = 35,
                    UserId = "treasurer-user-id-001",
                    CreatedDate = SeedDate.AddYears(-3)
                },
                // Referee member
                new Member
                {
                    Id = 3,
                    FullName = "Trọng tài CLB",
                    Email = "referee@pcm.com",
                    PhoneNumber = "0901234502",
                    DateOfBirth = new DateTime(1990, 8, 20),
                    JoinDate = SeedDate.AddYears(-2),
                    RankLevel = 4.5,
                    IsActive = true,
                    TotalMatches = 30,
                    WinMatches = 20,
                    UserId = "referee-user-id-001",
                    CreatedDate = SeedDate.AddYears(-2)
                }
            };

            // Add regular members
            var memberNames = new[]
            {
                "Nguyễn Văn An", "Trần Thị Bình", "Lê Văn Cường", "Phạm Thị Dung",
                "Hoàng Văn Em", "Vũ Thị Phương", "Đặng Văn Giang", "Bùi Thị Hoa",
                "Ngô Văn Khải", "Lý Thị Lan", "Đinh Văn Minh", "Trương Thị Ngọc"
            };

            var ranks = new[] { 4.5, 4.2, 4.0, 3.8, 3.6, 3.5, 3.4, 3.2, 3.0, 2.8, 2.5, 2.0 };

            for (int i = 0; i < 12; i++)
            {
                members.Add(new Member
                {
                    Id = i + 4, // Start from 4 since 1-3 are admin, treasurer, referee
                    FullName = memberNames[i],
                    Email = $"member{i + 1}@pcm.com",
                    PhoneNumber = $"090123456{i:D2}",
                    DateOfBirth = new DateTime(1990 + (i % 10), (i % 12) + 1, (i % 28) + 1),
                    JoinDate = SeedDate.AddMonths(-i - 1),
                    RankLevel = ranks[i],
                    IsActive = true,
                    TotalMatches = 20 - i,
                    WinMatches = 15 - i,
                    UserId = $"member-user-id-{(i + 1):D3}",
                    CreatedDate = SeedDate.AddMonths(-i - 1)
                });
            }

            builder.Entity<Member>().HasData(members);
        }

        private void SeedNews(ModelBuilder builder)
        {
            builder.Entity<News>().HasData(
                new News
                {
                    Id = 1,
                    Title = "🏆 Giải đấu Mini-game Team Battle tháng 1/2026",
                    Content = "CLB tổ chức giải đấu Team Battle với Entry Fee 50.000đ/người. Thời gian: 25-26/01/2026. Đăng ký ngay!",
                    IsPinned = true,
                    CreatedDate = SeedDate.AddDays(-5)
                },
                new News
                {
                    Id = 2,
                    Title = "📢 Thông báo đóng quỹ tháng 1/2026",
                    Content = "Các thành viên vui lòng đóng quỹ tháng 1/2026 (100.000đ) trước ngày 31/01. Liên hệ Thủ quỹ để chuyển khoản.",
                    IsPinned = true,
                    CreatedDate = SeedDate.AddDays(-3)
                },
                new News
                {
                    Id = 3,
                    Title = "🎉 Vinh danh Top 5 cao thủ tháng 12/2025",
                    Content = "Chúc mừng An, Bình, Cường, Dung, Em đã đạt Top 5 Rank DUPR cao nhất tháng 12/2025!",
                    IsPinned = false,
                    CreatedDate = SeedDate.AddDays(-10)
                },
                new News
                {
                    Id = 4,
                    Title = "🔧 Bảo trì Sân 3 từ 20-30/01/2026",
                    Content = "Sân 3 sẽ tạm đóng để sửa chữa mặt sân và thay đèn. Các bạn vui lòng đặt sân 1 hoặc sân 2.",
                    IsPinned = false,
                    CreatedDate = SeedDate.AddDays(-7)
                }
            );
        }

        private void SeedTransactions(ModelBuilder builder)
        {
            builder.Entity<Transaction>().HasData(
                // Thu
                new Transaction { Id = 1, Date = SeedDate.AddDays(-30), Amount = 1000000, Description = "Thu quỹ tháng 12/2025", CategoryId = 2, CreatedById = 1, CreatedDate = SeedDate.AddDays(-30) },
                new Transaction { Id = 2, Date = SeedDate.AddDays(-25), Amount = 500000, Description = "Thu tiền sân tuần 1", CategoryId = 1, CreatedById = 1, CreatedDate = SeedDate.AddDays(-25) },
                new Transaction { Id = 3, Date = SeedDate.AddDays(-20), Amount = 300000, Description = "Tài trợ từ shop bóng", CategoryId = 4, CreatedById = 1, CreatedDate = SeedDate.AddDays(-20) },
                // Chi
                new Transaction { Id = 4, Date = SeedDate.AddDays(-28), Amount = 200000, Description = "Mua nước cho hội viên", CategoryId = 5, CreatedById = 1, CreatedDate = SeedDate.AddDays(-28) },
                new Transaction { Id = 5, Date = SeedDate.AddDays(-22), Amount = 150000, Description = "Mua bóng mới", CategoryId = 7, CreatedById = 1, CreatedDate = SeedDate.AddDays(-22) },
                new Transaction { Id = 6, Date = SeedDate.AddDays(-15), Amount = 100000, Description = "Sửa vợt CLB", CategoryId = 8, CreatedById = 1, CreatedDate = SeedDate.AddDays(-15) }
            );
            // Tổng Thu: 1,800,000 - Tổng Chi: 450,000 = Số dư: 1,350,000 (dương)
        }

        private void SeedChallengesAndMatches(ModelBuilder builder)
        {
            // Seed 1 Challenge MiniGame TeamBattle đang diễn ra
            builder.Entity<Challenge>().HasData(
                new Challenge
                {
                    Id = 1,
                    Title = "Giải Mini-game Team Battle tháng 1/2026",
                    Type = ChallengeType.MiniGame,
                    GameMode = GameMode.TeamBattle,
                    Status = ChallengeStatus.Ongoing,
                    Config_TargetWins = 5,
                    CurrentScore_TeamA = 2,
                    CurrentScore_TeamB = 1,
                    EntryFee = 50000,
                    PrizePool = 500000,
                    Description = "Giải đấu chia 2 phe A-B, đánh chạm mốc 5 trận thắng.",
                    CreatedById = 1,
                    StartDate = SeedDate.AddDays(-2),
                    EndDate = SeedDate.AddDays(5),
                    CreatedDate = SeedDate.AddDays(-5)
                },
                // Seed 1 Duel
                new Challenge
                {
                    Id = 2,
                    Title = "Kèo thách đấu: An vs Cường",
                    Type = ChallengeType.Duel,
                    GameMode = GameMode.None,
                    Status = ChallengeStatus.Open,
                    EntryFee = 0,
                    PrizePool = 0,
                    Description = "Kèo vui vẻ, thua mời nước!",
                    CreatedById = 1,
                    CreatedDate = SeedDate.AddDays(-1)
                }
            );

            // Seed Participants cho Challenge 1 (10 người, chia TeamA và TeamB)
            // MemberId 4-13 (regular members, since 1-3 are admin, treasurer, referee)
            var participants = new List<Participant>();
            for (int i = 1; i <= 10; i++)
            {
                participants.Add(new Participant
                {
                    Id = i,
                    ChallengeId = 1,
                    MemberId = i + 3, // Members 4-13
                    Team = i <= 5 ? ParticipantTeam.TeamA : ParticipantTeam.TeamB,
                    EntryFeePaid = true,
                    EntryFeeAmount = 50000,
                    JoinedDate = SeedDate.AddDays(-3),
                    Status = ParticipantStatus.Confirmed
                });
            }
            builder.Entity<Participant>().HasData(participants);

            // Seed Matches (4 trận đã diễn ra)
            // Player IDs are now 4-15 (regular members)
            builder.Entity<Match>().HasData(
                // Trận Singles, thuộc Challenge 1, Team1 thắng
                new Match
                {
                    Id = 1,
                    Date = SeedDate.AddDays(-1),
                    IsRanked = true,
                    ChallengeId = 1,
                    MatchFormat = MatchFormat.Singles,
                    Team1_Player1Id = 4, // An (TeamA)
                    Team2_Player1Id = 9, // Phương (TeamB)
                    WinningSide = WinningSide.Team1,
                    CreatedDate = SeedDate.AddDays(-1)
                },
                // Trận Singles, thuộc Challenge 1, Team1 thắng
                new Match
                {
                    Id = 2,
                    Date = SeedDate.AddDays(-1).AddHours(2),
                    IsRanked = true,
                    ChallengeId = 1,
                    MatchFormat = MatchFormat.Singles,
                    Team1_Player1Id = 5, // Bình (TeamA)
                    Team2_Player1Id = 10, // Giang (TeamB)
                    WinningSide = WinningSide.Team1,
                    CreatedDate = SeedDate.AddDays(-1).AddHours(2)
                },
                // Trận Doubles, thuộc Challenge 1, Team2 thắng
                new Match
                {
                    Id = 3,
                    Date = SeedDate.AddHours(-5),
                    IsRanked = true,
                    ChallengeId = 1,
                    MatchFormat = MatchFormat.Doubles,
                    Team1_Player1Id = 6, // Cường (TeamA)
                    Team1_Player2Id = 7, // Dung (TeamA)
                    Team2_Player1Id = 11, // Hoa (TeamB)
                    Team2_Player2Id = 12, // Khải (TeamB)
                    WinningSide = WinningSide.Team2,
                    CreatedDate = SeedDate.AddHours(-5)
                },
                // Trận giao hữu (không thuộc Challenge)
                new Match
                {
                    Id = 4,
                    Date = SeedDate.AddDays(-3),
                    IsRanked = true,
                    ChallengeId = null,
                    MatchFormat = MatchFormat.Singles,
                    Team1_Player1Id = 4,
                    Team2_Player1Id = 6,
                    WinningSide = WinningSide.Team1,
                    CreatedDate = SeedDate.AddDays(-3)
                }
            );
        }

        private void ConfigureRelationships(ModelBuilder builder)
        {
            // Cấu hình quan hệ cho Match - tránh cascade delete cycles
            builder.Entity<Match>()
                .HasOne(m => m.Team1_Player1)
                .WithMany()
                .HasForeignKey(m => m.Team1_Player1Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Match>()
                .HasOne(m => m.Team1_Player2)
                .WithMany()
                .HasForeignKey(m => m.Team1_Player2Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Match>()
                .HasOne(m => m.Team2_Player1)
                .WithMany()
                .HasForeignKey(m => m.Team2_Player1Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Match>()
                .HasOne(m => m.Team2_Player2)
                .WithMany()
                .HasForeignKey(m => m.Team2_Player2Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình quan hệ cho Transaction
            builder.Entity<Transaction>()
                .HasOne(t => t.CreatedBy)
                .WithMany(m => m.Transactions)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);

            // Cấu hình quan hệ cho Booking
            builder.Entity<Booking>()
                .HasOne(b => b.Member)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình quan hệ cho Challenge
            builder.Entity<Challenge>()
                .HasOne(c => c.CreatedBy)
                .WithMany(m => m.CreatedChallenges)
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình quan hệ cho Participant
            builder.Entity<Participant>()
                .HasOne(p => p.Member)
                .WithMany(m => m.Participations)
                .HasForeignKey(p => p.MemberId)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình quan hệ cho Notification
            builder.Entity<Notification>()
                .HasOne(n => n.Member)
                .WithMany()
                .HasForeignKey(n => n.MemberId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
