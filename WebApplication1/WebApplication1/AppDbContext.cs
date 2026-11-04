using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Statistique>    Statistiques    { get; set; }
    public DbSet<CustomPalette>  CustomPalettes  { get; set; }
    public DbSet<CatalogueModule> CatalogueModules { get; set; }
}

[Table("Statistique")]
public class Statistique
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

[Table("custom_palettes")]
public class CustomPalette
{
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("name")]
    public string Name { get; set; } = "Ma palette";

    [Column("brand_default")]
    public string BrandDefault { get; set; } = "#5b3df5";

    [Column("brand_dark")]
    public string BrandDark { get; set; } = "#3a1fd0";

    [Column("surface")]
    public string Surface { get; set; } = "#ffffff";

    [Column("page_bg")]
    public string PageBg { get; set; } = "#eef0f5";

    [Column("ink")]
    public string Ink { get; set; } = "#14121f";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Une ligne = un type de module du catalogue Calque (hero, tarifs, footer…),
/// avec le nombre de variantes disponibles pour ce type.
/// Sert à alimenter les chiffres de la section « Le catalogue » sur la landing.
/// ⚠️ Source de vérité des variantes = les fichiers `data/*-variants.ts` du front.
/// Quand on ajoute une variante, penser à mettre `variant_count` à jour ici.
/// </summary>
[Table("catalogue_modules")]
public class CatalogueModule
{
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Lettre de la famille (A → F), reprise comme repère visuel sur la landing.</summary>
    [Column("family_ref")]
    public string FamilyRef { get; set; } = string.Empty;

    [Column("family_name")]
    public string FamilyName { get; set; } = string.Empty;

    /// <summary>Identifiant du type de section, aligné sur `SECTION_CATALOG` côté front.</summary>
    [Column("module_id")]
    public string ModuleId { get; set; } = string.Empty;

    [Column("module_name")]
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>Nombre de variantes proposées dans le composeur pour ce module.</summary>
    [Column("variant_count")]
    public int VariantCount { get; set; }

    [Column("sort_order")]
    public int SortOrder { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
