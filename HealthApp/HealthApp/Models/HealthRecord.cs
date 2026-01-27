using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Models
{
    public class HealthRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }   // links to User

        public DateTime Date { get; set; }

        public double Bmi { get; set; }

        public string Category { get; set; } = string.Empty;
    }

}
