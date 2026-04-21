using HeroArena.Data;
using HeroArena.Helpers;
using HeroArena.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace HeroArena.Views;

public partial class SpellsView : Page
{
    private List<Hero> _heroes = new();
    private List<Spell> _allSpells = new();

    public SpellsView()
    {
        InitializeComponent();
        LoadData();
    }

    private async void LoadData()
    {
        using var db = new AppDbContext(AppSettings.ConnectionString);

        _heroes = await db.Heroes
            .Include(h => h.HeroSpells)
            .ThenInclude(hs => hs.Spell)
            .ToListAsync();

        _allSpells = await db.Spells.ToListAsync();

        var filtres = new List<Hero> { new Hero { ID = 0, Name = "Tous les héros" } };
        filtres.AddRange(_heroes);
        CmbHeroes.ItemsSource = filtres;
        CmbHeroes.DisplayMemberPath = "Name";
        CmbHeroes.SelectedIndex = 0;

        LstSpells.ItemsSource = _allSpells;
    }

    private void CmbHeroes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CmbHeroes.SelectedItem is Hero hero)
        {
            if (hero.ID == 0)
                LstSpells.ItemsSource = _allSpells;
            else
            {
                var spells = hero.HeroSpells.Select(hs => hs.Spell).ToList();
                LstSpells.ItemsSource = spells;
            }
        }
    }
}