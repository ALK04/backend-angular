using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <summary>
    /// Catalogue des modules Calque en base : une ligne par type de section,
    /// avec son nombre de variantes. Alimente les chiffres de la landing
    /// (« Le catalogue ») via GET /catalogue/stats.
    ///
    /// Les valeurs semées correspondent exactement au contenu des fichiers
    /// `frontend-angular/src/app/features/composer/data/*-variants.ts`
    /// au moment de l'écriture : 21 modules, 73 variantes, 6 familles.
    /// </summary>
    public partial class AddCatalogueModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS catalogue_modules (
                    id            UUID        DEFAULT gen_random_uuid() PRIMARY KEY,
                    family_ref    TEXT        NOT NULL,
                    family_name   TEXT        NOT NULL,
                    module_id     TEXT        NOT NULL UNIQUE,
                    module_name   TEXT        NOT NULL,
                    variant_count INTEGER     NOT NULL DEFAULT 0,
                    sort_order    INTEGER     NOT NULL DEFAULT 0,
                    updated_at    TIMESTAMPTZ NOT NULL DEFAULT NOW()
                );
            ");

            // Semis idempotent : rejouer la migration remet les compteurs à jour
            // au lieu d'échouer sur la contrainte d'unicité de module_id.
            migrationBuilder.Sql(@"
                INSERT INTO catalogue_modules
                    (family_ref, family_name, module_id, module_name, variant_count, sort_order)
                VALUES
                    ('A', 'Navigation',    'navbar',       'Navbar',              11,  1),
                    ('A', 'Navigation',    'header',       'Header de page',       4,  2),
                    ('B', 'En-têtes',      'hero',         'Hero',                 5,  3),
                    ('B', 'En-têtes',      'cta',          'CTA',                  2,  4),
                    ('C', 'Présentation',  'features',     'Fonctionnalités',      2,  5),
                    ('C', 'Présentation',  'process',      'Processus / Étapes',   2,  6),
                    ('C', 'Présentation',  'story',        'À propos / Histoire',  4,  7),
                    ('C', 'Présentation',  'stats',        'Statistiques',         5,  8),
                    ('C', 'Présentation',  'testimonials', 'Témoignages',          3,  9),
                    ('C', 'Présentation',  'quote',        'Citation',             2, 10),
                    ('C', 'Présentation',  'logos',        'Logos / Presse',       3, 11),
                    ('C', 'Présentation',  'team',         'Équipe',               3, 12),
                    ('D', 'Conversion',    'pricing',      'Tarifs',               1, 13),
                    ('D', 'Conversion',    'faq',          'FAQ',                  3, 14),
                    ('D', 'Conversion',    'forms',        'Formulaire',           7, 15),
                    ('D', 'Conversion',    'contact',      'Contact',              3, 16),
                    ('E', 'Vitrine',       'portfolio',    'Portfolio / Galerie',  4, 17),
                    ('E', 'Vitrine',       'product',      'Fiche produit',        1, 18),
                    ('E', 'Vitrine',       'blog',         'Blog / Articles',      3, 19),
                    ('E', 'Vitrine',       'video',        'Vidéo / Démo',         2, 20),
                    ('F', 'Pied de page',  'footer',       'Footer',               3, 21)
                ON CONFLICT (module_id) DO UPDATE SET
                    family_ref    = EXCLUDED.family_ref,
                    family_name   = EXCLUDED.family_name,
                    module_name   = EXCLUDED.module_name,
                    variant_count = EXCLUDED.variant_count,
                    sort_order    = EXCLUDED.sort_order,
                    updated_at    = NOW();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS catalogue_modules;");
        }
    }
}
