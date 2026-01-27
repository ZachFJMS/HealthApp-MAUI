using HealthApp.Data;
using HealthApp.Models;

namespace HealthApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _db;
    public LoginPage(DatabaseService db)
	{
		InitializeComponent();
        _db = db;
	}

    private async void OnLoginClicked(object? sender, EventArgs e)
    {

        var username = UsernameEntry.Text;   
        var password = PasswordEntry.Text;

        var user = await _db.GetUser(username, password);

        if (user == null)
        {
            await DisplayAlert("Error", "Invalid username or password", "OK");
            return;
        }
        else
        {
            await DisplayAlert("Success", "Welcome" + username, "OK");
            await Shell.Current.GoToAsync($"UserPage?userid={user.Id}");
        }
    }

    protected override async void OnAppearing()
    {
        await _db.AddUser(new User {Username = "student", Password = "health123"});
    }

}