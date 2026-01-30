using HealthApp.Data;
using HealthApp.Views.Charts;

namespace HealthApp.Views;

[QueryProperty(nameof(UserId), "userid")]
public partial class SleepPage : ContentPage
{
    private readonly DatabaseService _db;
    private ChartDrawable _chart = new();

    public int UserId { get; set; }

    public SleepPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;

        SleepChart.Drawable = _chart;
    }
    private void OnToggleChartClicked(object sender, EventArgs e)
    {
        _chart.ChartMode =
            _chart.ChartMode == ChartDrawable.ChartType.Line
            ? ChartDrawable.ChartType.Bar
            : ChartDrawable.ChartType.Line;

        SleepChart.Invalidate();
    }

    private async void OnBackToDashboardClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///UserPage?userid=" + UserId);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var records = (await _db.GetAllSleepRecords(UserId))
                        .OrderBy(r => r.Date)
                        .ToList();

        NoRecords.IsVisible = records.Count < 2;

        HistoryCollection.ItemsSource = null;
        HistoryCollection.ItemsSource = records;

        var values = records.Select(r => r.HoursSlept).ToList();
        var labels = records.Select(r => r.Date.ToString("dd/MM")).ToList();

        _chart.SetLineData(values, labels);
        SleepChart.Invalidate();
    }
}