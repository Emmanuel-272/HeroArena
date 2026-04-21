using HeroArena.Data;
using HeroArena.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace HeroArena.Views;

public partial class SettingsView : Page
{
    public SettingsView()
    {
        InitializeComponent();
        TxtConnectionString.Text = AppSettings.ConnectionString;
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var connStr = TxtConnectionString.Text.Trim();
        if (string.IsNullOrEmpty(connStr))
        {
            TxtStatus.Text = "❌ La chaîne de connexion ne peut pas être vide.";
            TxtStatus.Foreground = System.Windows.Media.Brushes.Red;
            return;
        }

        try
        {
            using var db = new AppDbContext(connStr);
            await db.Database.CanConnectAsync();
            AppSettings.ConnectionString = connStr;
            TxtStatus.Text = "✅ Connexion réussie et sauvegardée !";
            TxtStatus.Foreground = System.Windows.Media.Brushes.LightGreen;
        }
        catch (Exception ex)
        {
            TxtStatus.Text = $"❌ Erreur : {ex.Message}";
            TxtStatus.Foreground = System.Windows.Media.Brushes.Red;
        }
    }
}