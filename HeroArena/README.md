# Hero Arena - Projet WPF B2

Projet réalisé dans le cadre du cours de développement C#.
Application de combat tour par tour avec des héros.

## Ce qu'il faut pour lancer le projet

- Visual Studio 2022
- .NET 8
- SQL Server Express
- SSMS

## Mise en place de la base de données

1. Ouvrir SSMS
2. Exécuter le script `database.sql` qui se trouve à la racine du projet
3. Ça va créer la base `ExerciceHero` avec toutes les tables et les données

## Lancer l'appli

1. Cloner le repo
2. Ouvrir `HeroArena.sln` dans Visual Studio
3. Vérifier la connection string dans `Helpers/AppSettings.cs`
4. Lancer avec F5

## Comptes disponibles

- `admin` / `admin`
- `player1` / `password1`

## Packages NuGet utilisés

- Microsoft.EntityFrameworkCore.SqlServer 8.0.0
- Microsoft.EntityFrameworkCore.Tools 8.0.0

## Notes

La connection string peut aussi être modifiée directement depuis l'onglet Settings de l'application sans avoir à recompiler.