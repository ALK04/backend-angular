# Calque — backend

**Calque** est un générateur de projet : on compose visuellement une interface (navbar, hero, sections, couleurs…) et Calque produit un vrai projet **React + Express** téléchargeable, prêt pour `npm install` / `npm run dev`.

Ce dépôt contient :

| Dossier | Contenu |
|---|---|
| `WebApplication1/` | API ASP.NET Core (.NET 9, Minimal API) + EF Core, base PostgreSQL sur Supabase |
| `templates/react-express-starter/` | Gabarit de référence du projet React + Express généré par Calque |
| `CLAUDE.md` | Documentation détaillée de l'architecture (pour Claude Code et pour moi) |

Le **frontend Angular** est dans un dépôt séparé : [ALK04/Angular-frontend](https://github.com/ALK04/Angular-frontend). Il se clone **dans** ce dossier, sous `frontend-angular/` (déjà ignoré par le `.gitignore`).

---

## Installer le projet sur un nouveau PC

### 1. Prérequis

- **.NET 9 SDK** + **JetBrains Rider** (le backend se lance depuis l'IDE)
- **Node.js v24** + npm (pour le frontend)
- **Git**

### 2. Cloner les deux dépôts

```bash
git clone https://github.com/ALK04/backend-angular.git RiderProjects
cd RiderProjects
git clone https://github.com/ALK04/Angular-frontend.git frontend-angular
```

On obtient :

```
RiderProjects/
├── WebApplication1/     ← ce dépôt
├── templates/           ← ce dépôt
├── CLAUDE.md            ← ce dépôt
└── frontend-angular/    ← dépôt Angular-frontend
```

### 3. ⚠️ Remettre le mot de passe Supabase

Le mot de passe de la base **n'est pas sur GitHub** (volontairement). Sans lui, le backend ne démarre pas.

1. Dans `WebApplication1/WebApplication1/`, copier `appsettings.Local.example.json` en **`appsettings.Local.json`**.
2. Remplacer `<MOT_DE_PASSE_SUPABASE>` par le vrai mot de passe (Supabase → *Project Settings → Database*).

`appsettings.Local.json` est ignoré par git : **ne jamais remettre le mot de passe dans `appsettings.json`**.

### 4. Lancer le backend

Ouvrir `WebApplication1/WebApplication1.sln` dans Rider → **Run** (profil `http`).
L'API écoute sur **http://localhost:5173**. Les migrations EF Core s'appliquent automatiquement au démarrage.

### 5. Lancer le frontend

```bash
cd frontend-angular
npm install
npm start
```

→ **http://localhost:4200**. Voir le README du dépôt frontend pour le détail.

---

## Base de données

- PostgreSQL hébergé sur **Supabase** (projet `iymmlldrivijreujkxem`, région eu-west-1) : rien à installer en local.
- Connexion via le **Session pooler** (`aws-0-eu-west-1.pooler.supabase.com:5432`) — la connexion directe `db.<ref>.supabase.co` ne résout plus.
- Le schéma est géré par les **migrations EF Core** (`WebApplication1/WebApplication1/Migrations/`).

## Endpoints

| Méthode | Route | Rôle |
|---|---|---|
| `GET` / `POST` / `DELETE` | `/palettes` | Palettes de couleurs personnalisées |
| `GET` | `/catalogue/stats` | Chiffres du catalogue affichés sur la landing |
| `POST` | `/statistique` | Ancien endpoint de démo (plus appelé) |

## Secrets — à ne jamais pousser

- `appsettings.Local.json` (mot de passe Supabase)
- tout fichier `.env` (seuls les `.env.example` sont versionnés)

## Keep-alive Supabase (GitHub Action)

Le plan gratuit de Supabase met la base en pause après 7 jours sans activité. Le workflow `.github/workflows/supabase-keepalive.yml` exécute une requête SQL **tous les 3 jours**.

- Il a besoin du secret **`SUPABASE_DB_PASSWORD`** (GitHub → *Settings → Secrets and variables → Actions*).
- Test manuel : onglet **Actions** → *Supabase keep-alive* → **Run workflow**.
- ⚠️ GitHub désactive les workflows planifiés d'un dépôt sans commit depuis 60 jours : il faut alors le réactiver depuis l'onglet Actions (GitHub envoie un mail avant).
