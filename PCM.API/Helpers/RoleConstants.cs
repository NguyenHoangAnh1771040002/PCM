namespace PCM.API.Helpers
{
    public static class RoleConstants
    {
        public const string Admin = "Admin";
        public const string Treasurer = "Treasurer";
        public const string Referee = "Referee";
        public const string Member = "Member";

        // Kết hợp roles
        public const string AdminOrTreasurer = "Admin,Treasurer";
        public const string AdminOrReferee = "Admin,Referee";
        public const string AdminTreasurerReferee = "Admin,Treasurer,Referee";
        public const string AllRoles = "Admin,Treasurer,Referee,Member";
    }
}
