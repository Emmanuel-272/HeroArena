using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using HeroArena.Data;
using HeroArena.Helpers;
using HeroArena.Views;
using Microsoft.EntityFrameworkCore;

namespace HeroArena.ViewModels;

public class LoginVMX : INotifyPropertyChanged
{
    public bool DebugFlag { get; set; } = false;

    private string _username = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading = false;

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }
    public ICommand GoToSettingsCommand { get; }

    public LoginVMX()
    {
        LoginCommand = new RelayCommand<string>(ExecuteLogin);
        GoToSettingsCommand = new RelayCommand(OpenSettings);
    }

    internal async void ExecuteLogin(string? password)
    {
        ErrorMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
        {
            ErrorMessage = "Veuillez remplir tous les champs.";
            return;
        }

        IsLoading = true;
        try
        {
            var hash = PasswordHelper.HashPassword(password);
            using var db = new AppDbContext(AppSettings.ConnectionString);
            var login = await db.Logins
                .FirstOrDefaultAsync(l => l.Username == Username && l.PasswordHash == hash);

            if (login == null)
            {
                ErrorMessage = "Identifiants incorrects.";
                return;
            }

            var mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows[0]?.Close();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur BDD : {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void ExecuteLoginPublic(string password)
    {
        ExecuteLogin(password);
    }

    private void OpenSettings()
    {
        MessageBox.Show("Settings à venir !", "Info");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}