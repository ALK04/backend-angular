# Projet RiderProjects

Monorepo avec un backend ASP.NET Core et un frontend Angular, reliés à une base PostgreSQL hébergée sur Supabase.

## Vision du produit — « Calque »

**Calque** est un **générateur / scaffolder de projet** — **≠ WordPress, ≠ site no-code hébergé**. L'utilisateur choisit visuellement des briques (navbars, heros, sections, palette de couleurs, etc.) ; à la fin, Calque **génère un vrai dossier de projet React + Node.js**, structuré proprement (dossiers de composants, routes, migrations de BDD, README/tuto). Le dev récupère le dossier, fait `npm install` puis `npm run dev`, et **continue à coder librement** dessus pour ses besoins spécifiques. **Valeur = supprimer tout le temps de mise en place de la structure**, sans lock-in : le code lui appartient.

- ⚠️ **Deux stacks à ne pas confondre** :
  - **L'app Calque** (ce repo) : Angular + .NET + Supabase.
  - **Ce qu'elle génère** : un projet **React + Vite + Tailwind** (front) + **Express** (back), en **TypeScript**, monorepo **npm workspaces** (`npm install` + `npm run dev` à la racine lance front + back). **Pas de BDD/migrations dans le MVP** (choix laissé au dev plus tard ; emplacement prévu dans `.env` + `server/`).
- **Gabarit du projet généré** : `templates/react-express-starter/` (à la racine du repo, hors de l'app Angular). C'est le squelette de référence que le moteur de Calque copiera/remplira. Contient `client/` (React/Vite/Tailwind), `server/` (Express), un `README.md` qui sert de **tuto dev**, et `.env.example`. Le `client/tailwind.config.js` porte le thème (couleurs/typo) — c'est là que les choix faits dans Calque atterriront.
- Nom : **Calque** (terme design = « couche »).
- Thèse / accroche : *« Composez votre interface. Le code suit. »*
- Identité visuelle (voir `frontend-angular/src/styles.css`) : papier froid `#eef0f5`, encre `#14121f`, violet électrique `#5b3df5` (marque/sélection), ambre `#ffb020` (guides, en touches rares). Typo : *Space Grotesk* (display), *Inter* (corps), *Space Mono* (étiquettes techniques). Polices chargées via Google Fonts dans `index.html`.
- État actuel : page de présentation (landing) **+** flux de composition (`/composer/*`) complet, **y compris l'étape finale « Générer »** qui produit et télécharge le ZIP du projet React. Voir « Le générateur » plus bas.

## Structure

- `WebApplication1/` — backend (solution .NET)
  - `WebApplication1/WebApplication1/` — le projet ASP.NET Core (Minimal API, .NET 9)
    - `Program.cs` — config des services, CORS, endpoints. Applique les migrations au démarrage (`db.Database.Migrate()`).
    - `AppDbContext.cs` — DbContext EF Core + entités (entité `Statistique`)
    - `Migrations/` — migrations EF Core (source de vérité du schéma)
    - `appsettings.Local.json` — chaîne de connexion Supabase (**non versionné**, chargé par `Program.cs`). Sur un nouveau poste : copier `appsettings.Local.example.json` et y mettre le mot de passe.
    - EF Core 9.0.2 + provider `Npgsql.EntityFrameworkCore.PostgreSQL` 9.0.4 ; package `Microsoft.EntityFrameworkCore.Design` présent.
  - `.mcp.json` — serveur MCP Supabase (voir note plus bas)
- `frontend-angular/` — frontend Angular 22 (standalone components, signals)
  - `src/app/app.ts` — composant racine = simple shell (`<router-outlet />`, template inline)
  - `src/styles.css` — tokens de design globaux (`:root` : couleurs, polices, layout). À réutiliser partout.
  - **Architecture modulaire** (mise en place pour la page de présentation) :
    - `src/app/models/` — interfaces TS (ex. `feature.model.ts`)
    - `src/app/shared/components/` — composants réutilisables : `site-header/`, `site-footer/`
    - `src/app/features/landing/` — page d'accueil (route `''`), `landing.ts` compose les sections
      - `features/landing/components/` — `hero/` (canevas-signature), `how-it-works/` (3 étapes Composez→Générez→Codez), `feature-list/`, `generator-preview/` (maquette 3 panneaux de l'éditeur)
  - Convention : un composant = un dossier ; templates/styles dans des fichiers séparés (`.html`/`.css`) sauf petits composants (inline). Styles **scopés** par composant (encapsulation Angular) → pas de collision CSS ; le global ne contient que les tokens.
  - `src/app/app.routes.ts` — routes
  - `src/app/app.config.ts` — providers (router, HttpClient)
  - **Flux Composer** (`src/app/features/composer/`) — voir section dédiée ci-dessous.

## Dépôts git

- `RiderProjects/` (backend + `templates/` + `CLAUDE.md`) → `github.com/ALK04/backend-angular` ; `frontend-angular/` y est ignoré.
- `frontend-angular/` → `github.com/ALK04/Angular-frontend` (dépôt séparé, cloné dans `RiderProjects/frontend-angular`).

## Lancer les apps

- **Backend** : se lance depuis l'IDE JetBrains (bouton Run). Profil `http` → écoute sur `http://localhost:5173`.
  - Pas de SDK .NET sur le PATH du shell — ne pas tenter `dotnet build`/`run`/`dotnet ef` en ligne de commande, lancer via l'IDE.
- **Frontend** : `npm start` **uniquement depuis le dossier `frontend-angular/`** → sert sur `http://localhost:4200`.
  - `node` (v24.17) et `npm` (v11) **sont** disponibles sur le PATH du shell : `npx ng build --configuration development` fonctionne et sert de vérification avant de rendre la main. (L'ancienne note « node absent du PATH » n'est plus vraie.)

## Base de données (Supabase)

- Projet Supabase : `iymmlldrivijreujkxem`, région **eu-west-1 (Ireland)**.
- **Important** : la connexion **directe** (`db.<ref>.supabase.co`) ne résout plus en DNS. On utilise le **Session pooler** (Supavisor) :
  - Host `aws-0-eu-west-1.pooler.supabase.com`, port `5432`, username `postgres.iymmlldrivijreujkxem`, SSL requis.
- **Schéma géré par migrations EF Core** (`Database.Migrate()` au démarrage), plus par `EnsureCreated()`.
  - ⚠️ Piège rencontré : `EnsureCreated()` ne crée le schéma que si **la base n'existe pas**. La base `postgres` de Supabase existe déjà → la table n'était jamais créée. D'où le passage aux migrations.
  - Table `Statistique` : `Id` (int, identity, PK), `Action` (text), `CreatedAt` (timestamptz).
  - Table `catalogue_modules` : une ligne par **type** de module du catalogue (`module_id` unique, aligné sur `SECTION_CATALOG`), avec `family_ref`/`family_name` (A → F), `variant_count` et `sort_order`. Semée par la migration `20260825120000_AddCatalogueModules` (21 modules, 73 variantes, 6 familles au moment de l'écriture). ⚠️ La source de vérité des variantes reste les fichiers `data/*-variants.ts` : **quand on ajoute une variante, mettre `variant_count` à jour** (SQL ou nouvelle migration), sinon le chiffre affiché sur la landing décroche.
  - La base contient déjà la table + `__EFMigrationsHistory` (baseline `20260619120000_InitialCreate`), donc `Migrate()` ne recrée rien.
- **Ajouter une migration** : `dotnet ef` n'étant pas dispo dans le shell, soit l'utilisateur la génère depuis l'IDE/terminal, soit on l'écrit à la main dans `Migrations/` (penser à maj `AppDbContextModelSnapshot.cs`) puis on l'applique via le MCP Supabase si besoin.

## Composer — architecture

Le flux de composition est dans `frontend-angular/src/app/features/composer/`.

### Routes et steps

| Route | Composant | Step id (dans `steps.ts`) |
|---|---|---|
| `/composer` | `Composer` | — |
| `/composer/sections` | `Sections` | `structure` |
| `/composer/hero` | `HeroStep` | `hero` |
| `/composer/features` | `FeaturesStep` | `fonctionnalites` |
| `/composer/pricing` | `PricingStep` | `tarifs` |
| `/composer/generer` | `GenerateStep` | `generer` |

### ComposerStore (`composer.store.ts`)

Service singleton (`providedIn: 'root'`) avec des **signals** Angular. Points clés :

- `selectedSections` — `Set<string>` des IDs cochés sur `/composer/sections`. Les IDs correspondent aux clés de `SECTION_CATALOG` : `'hero'`, `'features'`, `'pricing'`, `'stats'`, `'testimonials'`, etc.
- `visibleSteps` — computed : filtre `COMPOSER_STEPS` selon `selectedSections`. `navbar`, `structure`, `generer` sont toujours présents ; `hero`/`fonctionnalites`/`tarifs` n'apparaissent que si la section correspondante est cochée. **C'est ce signal qu'on passe aux templates pour le stepper.**
- `nextStepAfter(current)` — retourne la prochaine route `/composer/…` en sautant les steps non sélectionnés. Utilisé dans `continue()` de chaque step.
- `stepIndexFor(stepId)` — index dans `visibleSteps()`, utilisé pour le surlignage actif du stepper et l'eyebrow dynamique (`Étape 02 · Structure`).
- `nextStepLabelAfter(current)` — label lisible de la prochaine étape, affiché dans l'`action-hint` du pied de page.
- `sectionIdSignals` — index `type de section` → signal des variantes choisies. Sert à la sauvegarde/restauration sans réénumérer les 20 familles. **Une nouvelle famille de briques doit y être ajoutée**, sinon elle n'est pas persistée.
- `reset()` — remet la composition à zéro et supprime le brouillon. Câblé sur le bouton « Recommencer » de `composer-topbar` (confirmation en deux clics).

### Persistance locale (`composer.persistence.ts`)

La composition est sauvegardée dans `localStorage` (clé `calque.composer.draft`) : un `effect()` du constructeur du store relit tous les signaux persistés et réécrit le brouillon à chaque changement ; le constructeur le relit au démarrage. Le flux fait une vingtaine d'étapes, un F5 ne doit rien coûter.

- **Rien n'est écrit tant que la composition est vide** (pas de navbar, pas de section, pas de page) — sinon un simple passage sur `/composer` laisserait un brouillon fantôme.
- **`DRAFT_VERSION`** : à incrémenter dès que la forme du snapshot change. Un brouillon d'une version antérieure est **jeté**, pas migré.
- **Assainissement** : tout ce qui est relu est revalidé contre `VARIANTS_BY_TYPE` / `SECTION_CATALOG`. Une variante retirée du catalogue disparaît du brouillon au lieu de casser le générateur — c'est le vrai risque de la persistance, et c'est ce que couvre `composer.store.spec.ts`.
- Les palettes personnalisées ne sont **pas** persistées localement : elles viennent de la BDD via `PaletteApiService`.
- Tests : `npx ng test --watch=false` (vitest + jsdom). `composer.store.spec.ts` est pour l'instant la seule suite du projet.

### Pattern commun à chaque step (hero, features, pricing)

Chaque step `.ts` expose :
```ts
get steps()          { return this.store.visibleSteps(); }
get currentStepIndex(){ return this.store.stepIndexFor('hero'); /* ou fonctionnalites / tarifs */ }
get eyebrow()        { /* "Étape 03 · Hero" formaté dynamiquement */ }
get nextStepLabel()  { return this.store.nextStepLabelAfter('hero'); }
readonly previewHeight = 600;  // hauteur iframe vignette (px à 960 px de rendu)
readonly modalHeight   = 620;  // hauteur iframe dans la modale
modalVariant: BrickVariant | null = null;
openModal / closeModal / @HostListener escape
```

### Aperçus (BrickPreview + zoom CSS)

`BrickPreview` (`components/brick-preview/`) isole chaque brique dans une **iframe** avec Tailwind CDN + config `brand`/`ink`. La couleur `brand` est pilotée par `store.accent()`.

**Vignette miniature** : le composant wrapper `.frame-scaler` est forcé à `width: 960px` puis réduit via `zoom: 0.55` — l'iframe se rend en taille desktop (960 px) mais apparaît à ~528 px visuellement. Tout le contenu est visible sans scroll. `previewHeight` doit être ≥ à la hauteur réelle de la section à 960 px.

**Modale plein format** : s'ouvre au clic sur le `.frame-overlay` (overlay transparent au-dessus de l'iframe). `modalHeight` est distinct de `previewHeight`. Fermée par Échap, clic backdrop, ou bouton ✕.

### Données (fichiers `data/`)

- `section-catalog.ts` — liste des sections disponibles sur `/composer/sections` (SVG wireframe + id/nom/description). Ajouter une section ici **ne suffit pas** : il faut aussi créer le step correspondant, sa route, et le câbler dans `visibleSteps` + `nextStepAfter` du store.
- `steps.ts` — `COMPOSER_STEPS` : ordre canonique des 6 étapes. Modifier l'ordre ici modifie le stepper partout.
- `hero-variants.ts`, `features-variants.ts`, `pricing-variants.ts` — tableaux de `BrickVariant` (`id`, `name`, `description`, `previewHtml`, `tsx`). Ajouter une variante = push dans le tableau, aucun autre fichier à toucher.
- `accents.ts` — palettes de couleurs d'accent disponibles dans la topbar.

### Ajouter une nouvelle section (ex. Témoignages)

1. Vérifier que son `id` est dans `SECTION_CATALOG` (`section-catalog.ts`) — c'est déjà le cas pour `testimonials`.
2. Créer `steps/testimonials/testimonials-step.ts|html|css` en copiant le pattern hero/features/pricing.
3. Ajouter la route dans `app.routes.ts`.
4. Dans `composer.store.ts` : ajouter `'testimonials'` dans `STEP_ORDER`, `STEP_ROUTES`, `visibleSteps` (case dans le switch), et `nextStepLabelAfter`.
5. Ajouter `{ id: 'testimonials', label: 'Témoignages' }` dans `COMPOSER_STEPS` (`steps.ts`) à la bonne position.
6. Ajouter `['testimonials', this.testimonialsIds]` dans `sectionIdSignals` (`composer.store.ts`), sinon la section est perdue au rechargement.

## Le générateur (`features/composer/generator/`)

Transforme les choix du store en projet React téléchargeable. **Tout se passe dans le navigateur** — d'où le ZIP côté client (JSZip) plutôt qu'un aller-retour backend.

| Fichier | Rôle |
|---|---|
| `naming.ts` | `slugify` / `pascalCase` / `uniqueName` / `jsxSafeText`. Tout nom saisi ou dérivé passe par là. |
| `static-files.ts` | Les fichiers fixes du projet généré (Vite, tsconfig, Express…), écrits en dur car le navigateur ne peut pas lire `templates/react-express-starter/` sur le disque. **Garder les deux alignés.** |
| `build-project.ts` | Le moteur : `buildProject(input)` → `{ rootDir, files }` où `files` est un `chemin → contenu`. |
| `zip.ts` | Assemble via JSZip et déclenche le téléchargement. Les dossiers naissent des `/` dans les chemins. |
| `data/variant-registry.ts` | `VARIANTS_BY_TYPE` + `findVariant(id)` — index de tous les catalogues de briques. |

**Structure produite** : `client/src/` avec `config/theme.ts` (couleurs, **lu aussi par `tailwind.config.ts`** → source unique), `config/site.config.ts` (nom + `nav[]`), `layouts/MainLayout.tsx`, `components/layout|sections|ui/`, `pages/` (une page = un fichier), `App.tsx` (routes react-router). Un `variantId` = un fichier composant, même s'il sert sur plusieurs pages.

**Transformations de texte sur les `tsx`** (dans `build-project.ts`) — c'est la partie fragile :
- `renameDefaultExport` : toutes les variantes exportent `function Hero()`, il faut un nom unique par fichier.
- `injectNavLinks` : remplace le contenu du premier `<nav>` par une boucle sur `siteConfig.nav`. Le `className` du premier `<a>` sert de modèle de style.
- `linkifyHomeLink` : `<a href="/">` → `<Link to="/">`. **Doit tourner après `injectNavLinks`** : si le logo est dans le `<nav>`, il disparaît, et un import `Link` inutilisé casserait le build (`noUnusedLocals`).

**Liens de nav** : `page` → `/slug`, `anchor` → `/slug#ancre`, `external` → l'URL telle quelle. `NavItem.tsx` choisit `<Link>` ou `<a target="_blank">`.

**Ancres** : `NavLink.anchorVariantId` désigne la section ciblée, choisie dans un `<select>` à l'étape « Config. site » (repli sur le slug du libellé pour les liens créés avant ce champ). `generator/anchors.ts` — `anchorsForPage(page)` : `variantId` → ancre, dérivée du nom FR de `SECTION_CATALOG` et dédoublonnée (`fonctionnalites`, `fonctionnalites2`). **Ce module est la source unique** : le générateur pose ces `id` dans `pages/`, et `pages-step` s'en sert pour l'aperçu comme pour les href — les deux doivent rester d'accord, sinon un lien ne défile nulle part dans le projet livré. `slugify` est partagé pour la même raison sur les URLs de pages.

Non branchés sur le store, laissés en `href="#"` volontairement : le bouton CTA de la navbar et les liens des footers.

**⚠️ Les `tsx` du catalogue sont des chaînes de caractères.** Une erreur dedans est invisible : Angular compile (c'est du texte), et l'aperçu rend `previewHtml`, pas le React. Elle n'explose que chez le dev, après téléchargement. Deux pièges déjà rencontrés :
- **Apostrophe française** dans une chaîne délimitée par `'` → elle ferme la chaîne. Attention : dans un template literal, `\'` produit une apostrophe **nue**, donc échapper ne suffit pas — utiliser l'apostrophe typographique `’`.
- **Hook sans import** (`useState` sans `import { useState } from 'react'`) et **variables inutilisées** (`noUnusedLocals` dans le projet généré). Le générateur ajoute automatiquement les imports React manquants (`withReactImports`), mais pas les variables mortes : celles-là se corrigent dans la variante.

**Vérifier après toute modification du catalogue ou du générateur** :
1. `npm run check:variants` — contrôle syntaxique des 72 `tsx` (rapide, sans install). À lancer à chaque ajout de variante.
2. Pour une vérification de fond, générer un projet contenant **toutes** les variantes et lancer son `npm run build` : c'est ce qui attrape les erreurs de types, imports et variables inutilisées. Bundler un banc d'essai avec l'esbuild du projet (`npx --no-install esbuild harness.ts --bundle --platform=node --format=cjs`, en le sortant dans `frontend-angular/` pour que `node_modules` soit résolu), écrire les fichiers sur disque, puis `npm install && npm run build`. Un échantillon de quelques briques ne suffit pas — les 13 apostrophes cassées ont échappé à un test sur 6 variantes.
3. `renderToStaticMarkup` + `MemoryRouter` permet de contrôler les liens et les ancres sans navigateur.

## Backend — état actuel

- Endpoint `POST /statistique` toujours présent dans `Program.cs` (insère une ligne dans `Statistique`), **mais plus appelé par le frontend** (l'ancienne démo bouton a été retirée). À réutiliser/remplacer quand on branchera l'éditeur sur l'API.
- Endpoints `GET/POST/DELETE /palettes` — palettes personnalisées, consommés par `PaletteApiService` (étape `/composer/pages`).
- Endpoint `GET /catalogue/stats` — agrège `catalogue_modules` (total de composants, nb de types, nb de familles, `updatedAt`, détail par famille). Consommé par `CatalogueApiService` pour la section « § 03 · Le catalogue » de la landing, qui garde un repli statique si le backend est éteint.
- La landing n'est plus totalement statique : la section « Le catalogue » lit ses chiffres en base, et « § 05 · L'aperçu » rend de vraies briques via `buildPreviewDoc` (`features/composer/preview-doc.ts`, partagé avec l'étape `/composer/pages`).

## Notes

- Aucune sécurité en place pour l'instant (CORS ouvert, pas d'auth). Volontaire, phase de prototypage.
- **MCP Supabase** : le `.mcp.json` est dans `WebApplication1/`. Pour que les outils MCP soient disponibles, lancer Claude Code depuis `WebApplication1/`, ou déplacer/copier `.mcp.json` à la racine `RiderProjects/`.
  - Confirmé fonctionnel (authentifié) : permet d'inspecter/modifier la base directement (`list_tables`, `execute_sql`, `apply_migration`, `get_logs`…). Très utile pour diagnostiquer quand le schéma/les données ne correspondent pas à ce qu'on attend.
