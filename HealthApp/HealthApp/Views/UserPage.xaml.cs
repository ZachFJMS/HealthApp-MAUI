using HealthApp.Data;
using HealthApp.Models;

namespace HealthApp.Views;

[QueryProperty(nameof(UserId), "userid")]
public partial class UserPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly HealthScoreService _healthService;

    public int UserId { get; set; }

    public UserPage(DatabaseService db, HealthScoreService healthScoreServices)
    {
        InitializeComponent();
        _db = db;
        _healthService = healthScoreServices;
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

        BmiCategoryLabel.Text = $"Category: {category}";
        BmiValueLabel.Text = $"BMI: {bmi}";

        // SAVE TO DATABASE
        await _db.AddHealthRecord(new HealthRecord
        {
            UserId = UserId,
            Date = DateTime.Now,
            Bmi = bmi,
            Category = category
        });
    }

    private async void OnViewBmiHistoryClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///BmiPage?userid={UserId}");
    }

    protected override async void OnAppearing()
    {
        var user = await _db.GetUserById(UserId);

        if (user != null)
        {
            BindingContext = user;
        }

        var record = await _db.GetLatestHealthRecord(UserId);

        if (record != null)
        {
            BmiCategoryLabel.Text = $"Category: {record.Category}";
            BmiValueLabel.Text = $"BMI: {record.Bmi}";
        }
    }
}