namespace PCM.API.Models
{
    // Transaction Category Type
    public enum TransactionType
    {
        Income = 1,  // Thu
        Expense = 2  // Chi
    }

    // Booking Status
    public enum BookingStatus
    {
        Pending = 1,
        Confirmed = 2,
        Cancelled = 3
    }

    // Challenge Type
    public enum ChallengeType
    {
        Duel = 1,
        MiniGame = 2
    }

    // Game Mode
    public enum GameMode
    {
        None = 0,
        TeamBattle = 1,
        RoundRobin = 2
    }

    // Challenge Status
    public enum ChallengeStatus
    {
        Open = 1,
        Ongoing = 2,
        Finished = 3
    }

    // Match Format
    public enum MatchFormat
    {
        Singles = 1,  // 1vs1
        Doubles = 2   // 2vs2
    }

    // Winning Side
    public enum WinningSide
    {
        None = 0,
        Team1 = 1,
        Team2 = 2
    }

    // Participant Team
    public enum ParticipantTeam
    {
        None = 0,
        TeamA = 1,
        TeamB = 2
    }

    // Participant Status
    public enum ParticipantStatus
    {
        Pending = 1,
        Confirmed = 2,
        Withdrawn = 3
    }

    // Notification Type
    public enum NotificationType
    {
        Info = 1,
        Warning = 2,
        Success = 3,
        Error = 4
    }
}
