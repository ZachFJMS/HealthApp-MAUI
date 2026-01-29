using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Models
{
    public class SleepRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        // ---- Health Factors ----
        public double HoursSlept { get; set; }
    }
}
