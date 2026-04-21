namespace HeroArena.Helpers;

public static class AppSettings
{
    private static string _connectionString =
     "Server=BOOK-EN857VH73P\\SQLEXPRESS;Database=ExerciceHero;Trusted_Connection=True;TrustServerCertificate=True;";
    public static string ConnectionString
    {
        get => _connectionString;
        set => _connectionString = value;
    }
}