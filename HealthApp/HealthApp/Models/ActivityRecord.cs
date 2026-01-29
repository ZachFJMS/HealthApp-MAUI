using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Models
{
    public class ActivityRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        // ---- Health Factors ----
        public int DurationMinutes { get; set; }
        public double CaloriesBurned { get; set; }

        // ---- Derived Values ----
        public string ActivityType { get; set; } = string.Empty;
    }
}
