using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Models
{
    public class BMIRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }

        // ---- Health Factors ----
        public double HeightCm { get; set; }
        public double WeightKg { get; set; }
        public double Bmi { get; set; }

        // ---- Derived Values ----
        public string Category { get; set; } = string.Empty;  // BMI or health category
    }

}
