namespace HeroArena.Models;

public class Hero
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Health { get; set; }
    public string? ImageURL { get; set; }
    public ICollection<HeroSpell> HeroSpells { get; set; } = new List<HeroSpell>();
    public ICollection<PlayerHero> PlayerHeroes { get; set; } = new List<PlayerHero>();
}