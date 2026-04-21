using HeroArena.Data;
using HeroArena.Helpers;
using HeroArena.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace HeroArena.Views;

public partial class CombatView : Page
{
    private const int HP_MAX_WHEN_DIE = 12;

    private Hero? _playerHero;
    private Hero? _enemyHero;
    private int _playerCurrentHP;
    private int _enemyCurrentHP;
    private int _playerMaxHP;
    private int _enemyMaxHP;
    private int _score = 0;
    private bool _combatOver = false;
    private Random _rnd = new Random();

    public CombatView()
    {
        InitializeComponent();
        LoadCombat();
    }

    public void SetPlayerHero(Hero hero)
    {
        _playerHero = hero;
        LoadCombat();
    }

    private async void LoadCombat()
    {
        using var db = new AppDbContext(AppSettings.ConnectionString);
        var heroes = await db.Heroes
            .Include(h => h.HeroSpells)
            .ThenInclude(hs => hs.Spell)
            .ToListAsync();

        if (_playerHero == null)
            _playerHero = heroes[0];

        // Ennemi aléatoire différent du joueur avec +10% HP et +5% dégâts
        var enemies = heroes.Where(h => h.ID != _playerHero.ID).ToList();
        var baseEnemy = enemies[_rnd.Next(enemies.Count)];

        _enemyHero = new Hero
        {
            ID = baseEnemy.ID,
            Name = baseEnemy.Name + " ★",
            Health = (int)(baseEnemy.Health * 1.10),
            HeroSpells = baseEnemy.HeroSpells.Select(hs => new HeroSpell
            {
                HeroID = hs.HeroID,
                SpellID = hs.SpellID,
                Spell = new Spell
                {
                    ID = hs.Spell!.ID,
                    Name = hs.Spell.Name,
                    Damage = (int)(hs.Spell.Damage * 1.05),
                    Description = hs.Spell.Description
                }
            }).ToList()
        };

        _playerMaxHP = _playerHero.Health;
        _enemyMaxHP = _enemyHero.Health;
        _playerCurrentHP = _playerMaxHP;
        _enemyCurrentHP = _enemyMaxHP;
        _combatOver = false;

        UpdateUI();
        TxtLog.Text = "⚔ Le combat commence !\n";
    }

    private void UpdateUI()
    {
        TxtPlayerName.Text = _playerHero?.Name ?? "Joueur";
        TxtEnemyName.Text = _enemyHero?.Name ?? "Ennemi";
        TxtPlayerHP.Text = $"{_playerCurrentHP} / {_playerMaxHP}";
        TxtEnemyHP.Text = $"{_enemyCurrentHP} / {_enemyMaxHP}";
        PbPlayerHP.Value = (_playerCurrentHP / (double)_playerMaxHP) * 100;
        PbEnemyHP.Value = (_enemyCurrentHP / (double)_enemyMaxHP) * 100;
        TxtScore.Text = _score.ToString();
        LstPlayerSpells.ItemsSource = _playerHero?.HeroSpells;
        LstEnemySpells.ItemsSource = _enemyHero?.HeroSpells;
    }

    private void BtnSpell_Click(object sender, RoutedEventArgs e)
    {
        if (_combatOver || _playerHero == null || _enemyHero == null) return;
        if (sender is not Button btn || btn.Tag is not HeroSpell heroSpell) return;

        // Tour joueur
        int playerDmg = heroSpell.Spell!.Damage;
        _enemyCurrentHP -= playerDmg;
        if (_enemyCurrentHP < 0) _enemyCurrentHP = 0;
        TxtLog.Text += $"🗡 Tu utilises {heroSpell.Spell.Name} → {playerDmg} dégâts à {_enemyHero.Name}\n";

        if (_enemyCurrentHP <= 0)
        {
            _score++;
            _combatOver = true;
            UpdateUI();
            TxtLog.Text += GetDeathMessage(_enemyHero.Name, false);
            BtnNewCombat.Visibility = Visibility.Visible;
            ScrollLog.ScrollToBottom();
            return;
        }

        // Tour ennemi
        var enemySpells = _enemyHero.HeroSpells.ToList();
        var enemySpell = enemySpells[_rnd.Next(enemySpells.Count)];
        int enemyDmg = enemySpell.Spell!.Damage;
        _playerCurrentHP -= enemyDmg;
        if (_playerCurrentHP < 0) _playerCurrentHP = 0;
        TxtLog.Text += $"💀 {_enemyHero.Name} utilise {enemySpell.Spell.Name} → {enemyDmg} dégâts !\n";

        if (_playerCurrentHP <= HP_MAX_WHEN_DIE)
        {
            if (_playerCurrentHP <= 0)
            {
                _combatOver = true;
                UpdateUI();
                TxtLog.Text += GetDeathMessage(_playerHero.Name, true);
                BtnNewCombat.Visibility = Visibility.Visible;
                ScrollLog.ScrollToBottom();
                return;
            }
            TxtLog.Text += $"⚠ {_playerHero.Name} est en danger critique ! ({_playerCurrentHP} HP)\n";
        }

        UpdateUI();
        ScrollLog.ScrollToBottom();
    }

    private string GetDeathMessage(string name, bool isPlayer)
    {
        if (isPlayer)
            return $"\n💀 {name} est tombé... L'arène réclame un nouveau champion !\n";
        else
            return $"\n🏆 Victoire ! {name} a été vaincu ! Score : {_score}\n";
    }

    private void BtnNewCombat_Click(object sender, RoutedEventArgs e)
    {
        LoadCombat();
        BtnNewCombat.Visibility = Visibility.Collapsed;
    }
}