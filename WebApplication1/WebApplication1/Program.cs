using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Secrets (chaine de connexion Supabase) : fichier local non versionne.
// Copier appsettings.Local.example.json -> appsettings.Local.json et y mettre le mot de passe.
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Connexion a la base Supabase (chaine definie dans appsettings.Local.json)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS ouvert (aucune securite pour le moment, juste pour le dev)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Applique les migrations EF au demarrage (cree/maj le schema de la base)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

// Enregistre un clic du bouton dans la table Statistique
app.MapPost("/statistique", async (AppDbContext db) =>
{
    var stat = new Statistique { Action = "click", CreatedAt = DateTime.UtcNow };
    db.Statistiques.Add(stat);
    await db.SaveChangesAsync();
    return Results.Ok(stat);
});

// ── Palettes personnalisées ──────────────────────────────────────────────────

app.MapGet("/palettes", async (AppDbContext db) =>
    Results.Ok(await db.CustomPalettes.OrderByDescending(p => p.CreatedAt).ToListAsync()));

app.MapPost("/palettes", async (AppDbContext db, CustomPaletteDto dto) =>
{
    var palette = new CustomPalette
    {
        Name         = dto.Name,
        BrandDefault = dto.BrandDefault,
        BrandDark    = dto.BrandDark,
        Surface      = dto.Surface,
        PageBg       = dto.PageBg,
        Ink          = dto.Ink,
        CreatedAt    = DateTime.UtcNow,
    };
    db.CustomPalettes.Add(palette);
    await db.SaveChangesAsync();
    return Results.Created($"/palettes/{palette.Id}", palette);
});

app.MapDelete("/palettes/{id:guid}", async (AppDbContext db, Guid id) =>
{
    var palette = await db.CustomPalettes.FindAsync(id);
    if (palette is null) return Results.NotFound();
    db.CustomPalettes.Remove(palette);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// -- Catalogue des modules -----------------------------------------------------

// Chiffres du catalogue, calcules a partir de la table catalogue_modules.
// Consomme par la section « Le catalogue » de la landing.
app.MapGet("/catalogue/stats", async (AppDbContext db) =>
{
    var modules = await db.CatalogueModules.OrderBy(m => m.SortOrder).ToListAsync();

    var families = modules
        .GroupBy(m => new { m.FamilyRef, m.FamilyName })
        .OrderBy(g => g.Key.FamilyRef)
        .Select(g => new
        {
            code         = g.Key.FamilyRef,
            name         = g.Key.FamilyName,
            variantCount = g.Sum(m => m.VariantCount),
            modules      = g.OrderBy(m => m.SortOrder)
                            .Select(m => new { id = m.ModuleId, name = m.ModuleName, variantCount = m.VariantCount })
                            .ToList(),
        })
        .ToList();

    return Results.Ok(new
    {
        totalVariants = modules.Sum(m => m.VariantCount),
        moduleCount   = modules.Count,
        familyCount   = families.Count,
        updatedAt     = modules.Count > 0 ? modules.Max(m => m.UpdatedAt) : (DateTime?)null,
        families,
    });
});

app.Run();

record CustomPaletteDto(
    string Name,
    string BrandDefault,
    string BrandDark,
    string Surface,
    string PageBg,
    string Ink
);

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
