using HealthApp.Data;
using HealthApp.Views.Charts;

namespace HealthApp.Views;

[QueryProperty(nameof(UserId), "userid")]
public partial class ActivityPage : ContentPage
{
    private readonly DatabaseService _db;

    public int UserId { get; set; }

    private ChartDrawable _chart = new();

    public ActivityPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;

        ActivityChart.Drawable = _chart;
    }


    private void OnToggleChartClicked(object sender, EventArgs e)
    {
        _chart.ChartMode =
            _chart.ChartMode == ChartDrawable.ChartType.Line
            ? ChartDrawable.ChartType.Bar
            : ChartDrawable.ChartType.Line;

        _chart.YAxisLabel = "Minutes Active";
        _chart.XAxisLabel = "Date";

        ActivityChart.Invalidate();
    }

    public async void OnBackToDashboardClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///UserPage?userid=" + UserId);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var records = (await _db.GetAllActivityRecords(UserId))
                        .OrderBy(r => r.Date)
                        .ToList();

        if (records.Count < 2)
        {
            NoRecords.IsVisible = true;
        }
        else
        {
            NoRecords.IsVisible = false;
        }

        ActivityCollection.ItemsSource = null;
        ActivityCollection.ItemsSource = records;

        // ? SEND DATA TO SAME CHART
        var values = records.Select(r => r.CaloriesBurned).ToList();
        var labels = records.Select(r => r.Date.ToString("dd/MM")).ToList();

        _chart.SetLineData(values, labels);
        ActivityChart.Invalidate();
    }
}