using HealthApp.Data;
using HealthApp.Models;

namespace HealthApp.Views;

[QueryProperty(nameof(UserId), "userid")]
public partial class UserPage : ContentPage
{
    private readonly DatabaseService _db;

    public int UserId { get; set; }

    public UserPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void OnBmiCardTapped(object sender, EventArgs e)
    {
        string heightStr = await DisplayPromptAsync("BMI Calculator", "Enter height (cm):");
        string weightStr = await DisplayPromptAsync("BMI Calculator", "Enter weight (kg):");

        if (!double.TryParse(heightStr, out double heightCm) ||
            !double.TryParse(weightStr, out double weightKg))
        {
            await DisplayAlert("Error", "Please enter valid numbers", "OK");
            return;
        }

        double heightM = heightCm / 100.0;
        double bmi = weightKg / (heightM * heightM);
        bmi = Math.Round(bmi, 1);

        string category =
            bmi < 18.5 ? "Underweight" :
            bmi < 25 ? "Normal" :
            bmi < 30 ? "Overweight" :
            "Obese";

        BmiCategoryLabel.Text = "Category: " + category + "";
        BmiValueLabel.Text = " " + bmi + "";

        // SAVE TO DATABASE
        await _db.AddBMIRecord(new BMIRecord
        {
            UserId = UserId,
            Date = DateTime.Now,
            HeightCm = heightCm,
            WeightKg = weightKg,
            Bmi = bmi,
            Category = category
        });
    }

    private async void OnViewBmiHistoryClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///BmiPage?userid={UserId}");
    }

    private async void OnActivityCardTapped(object sender, EventArgs e)
    {
        string minutesStr = await DisplayPromptAsync("Activity", "Minutes of activity:");
        string caloriesStr = await DisplayPromptAsync("Activity", "Calories burned (estimate):");
        string type = await DisplayPromptAsync("Activity", "Type (Walking, Gym, etc):");

        if (!int.TryParse(minutesStr, out int minutes) ||
            !double.TryParse(caloriesStr, out double calories))
        {
            await DisplayAlert("Error", "Invalid input", "OK");
            return;
        }

        ActivitySummaryLabel.Text = " " + $" {minutes} min • {calories} kcal";

        // SAVE TO DATABASE
        await _db.AddActivityRecord(new ActivityRecord
        {
            UserId = UserId,
            Date = DateTime.Now,
            DurationMinutes = minutes,
            CaloriesBurned = calories,
            ActivityType = type ?? ""
        });
    }

    private async void OnViewActivityHistoryClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///ActivityPage?userid={UserId}");
    }

    private async void OnAddSleepClicked(object sender, EventArgs e)
    {
        string hoursStr = await DisplayPromptAsync("Sleep", "How many hours did you sleep?");

        if (!double.TryParse(hoursStr, out double hours))
        {
            await DisplayAlert("Error", "Enter a valid number", "OK");
            return;
        }

        SleepValueLabel.Text = $"{hours} hrs";

        // SAVE TO DATABASE
        await _db.AddSleepRecord(new SleepRecord
        {
            UserId = UserId,
            Date = DateTime.Now,
            HoursSlept = hours
        });
    }

    private async void OnViewSleepHistoryClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///SleepPage?userid={UserId}");
    }

    protected override async void OnAppearing()
    {
        var user = await _db.GetUserById(UserId);

        if (user != null)
        {
            BindingContext = user;
        }

        var record = await _db.GetLatestBMIRecord(UserId);

        if (record != null)
        {
            BmiCategoryLabel.Text = $"Category: {record.Category}";
            BmiValueLabel.Text = $"{record.Bmi}";
        }

        var activityRecord = await _db.GetLatestActivityRecord(UserId);

        if (activityRecord != null)
        {
            ActivitySummaryLabel.Text = $" {activityRecord.DurationMinutes} min • {activityRecord.CaloriesBurned} kcal";
        }

        var sleepRecord = await _db.GetLatestSleepRecord(UserId);

        if (sleepRecord != null)
        {
            SleepValueLabel.Text = $"{sleepRecord.HoursSlept} hrs";
        }
    }
}