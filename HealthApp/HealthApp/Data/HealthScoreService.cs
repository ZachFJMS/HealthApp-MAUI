using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Data
{
    public class HealthScoreService
    {
        public double CalculateScore(int age, double heightCm, double weightKg, int activityLevel)
        {
            // BMI
            double heightM = heightCm / 100.0;
            double bmi = weightKg / (heightM * heightM);

            // BMI score (ideal around 18.5–24.9)
            double bmiScore =
                bmi < 18.5 ? 0.6 :
                bmi <= 25 ? 1.0 :
                bmi <= 30 ? 0.7 :
                0.5;

            // Activity score (1–10 scaled)
            double activityScore = activityLevel / 10.0;

            // Age factor (slight reduction with age)
            double ageFactor = age < 30 ? 1.0 :
                               age < 50 ? 0.9 : 0.8;

            // Weighted final score
            double finalScore =
                (bmiScore * 0.5 + activityScore * 0.5) * ageFactor * 100;

            return Math.Round(finalScore, 1);
        }
    }
}
