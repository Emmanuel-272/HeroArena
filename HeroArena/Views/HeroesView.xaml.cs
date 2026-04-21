using HeroArena.Data;
using HeroArena.Helpers;
using HeroArena.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace HeroArena.Views;

public partial class HeroesView : Page
{
    private Hero? _selectedHero;

    public HeroesView()
    {
        InitializeComponent();
        LoadHeroes();
    }

    private async void LoadHeroes()
    {
        using var db = new AppDbContext(AppSettings.ConnectionString);
        var heroes = await db.Heroes
            .Include(h => h.HeroSpells)
            .ThenInclude(hs => hs.Spell)
            .ToListAsync();
        LstHeroes.ItemsSource = heroes;
    }

    private void LstHeroes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LstHeroes.SelectedItem is Hero hero)
        {
            _selectedHero = hero;
            TxtHeroName.Text = hero.Name;
            TxtHeroHP.Text = hero.Health.ToString();
            LstSpells.ItemsSource = hero.HeroSpells;
            PanelDetail.Visibility = Visibility.Visible;
            TxtPlaceholder.Visibility = Visibility.Collapsed;
        }
    }

    private void BtnSelectHero_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedHero != null)
            MessageBox.Show($"{_selectedHero.Name} sélectionné !", "Héros choisi");
    }
}