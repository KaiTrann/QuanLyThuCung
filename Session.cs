using System;

namespace Nhóm_7
{
    public static class Session
    {
        public static int UserId { get; private set; }
        public static string Username { get; private set; }
        public static string FullName { get; private set; }
        public static string Role { get; private set; }
        public static DateTime? CreatedAt { get; private set; }

        public static bool IsDarkTheme { get; set; } = false;

        public static void Set(int userId, string username, string fullName, string role, DateTime? createdAt = null)
        {
            UserId = userId;
            Username = username;
            FullName = fullName;
            Role = role;
            CreatedAt = createdAt;
        }

        public static void Clear()
        {
            UserId = 0;
            Username = null;
            FullName = null;
            Role = null;
            CreatedAt = null;
            IsDarkTheme = false;
        }
    }
}