using HeroArena.Views;
using System.Windows;

namespace HeroArena;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        TxtWelcome.Text = "Bienvenue, Joueur";
        FrameHeroes.Navigate(new HeroesView());
        FrameSpells.Navigate(new SpellsView());
        FrameCombat.Navigate(new CombatView());
        FrameSettings.Navigate(new SettingsView());
    }
}