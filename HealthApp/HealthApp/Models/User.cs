using SQLite;

namespace HealthApp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Health data
        public int Age { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public double ActivityLevel { get; set; }

    }
}
