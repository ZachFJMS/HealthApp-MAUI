using HealthApp.Data;
using HealthApp.Models;
using HealthApp.Views.Charts;

namespace HealthApp.Views;

[QueryProperty(nameof(UserId), "userid")]
public partial class BmiPage : ContentPage
{
    private readonly DatabaseService _db;

    private ChartDrawable _chart = new();

    public int UserId { get; set; }

    public BmiPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
        BmiChart.Drawable = _chart;
    }
    private void OnToggleChartClicked(object sender, EventArgs e)
    {
        _chart.ChartMode =
            _chart.ChartMode == ChartDrawable.ChartType.Line
            ? ChartDrawable.ChartType.Bar
            : ChartDrawable.ChartType.Line;

        BmiChart.Invalidate();
    }

    public async void OnBackToDashboardClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///UserPage?userid=" + UserId);
    }

    protected override async void OnAppearing()
    {
        var records = (await _db.GetAllBMIRecords(UserId))
                .OrderBy(r => r.Date)
                .Reverse()
                .ToList();

        if (records.Count < 2)
        {
            NoRecords.IsVisible = true;
        }
        else
        {
            NoRecords.IsVisible = false;
        }

        HistoryCollection.ItemsSource = null;
        HistoryCollection.ItemsSource = records;

        var bmiValues = records.Select(r => r.Bmi).ToList();
        var dateLabels = records.Select(r => r.Date.ToString("dd/MM")).ToList();

        _chart.SetLineData(bmiValues, dateLabels);
        BmiChart.Invalidate();
    }
}