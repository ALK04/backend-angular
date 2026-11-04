# Mon site — généré par Calque

Ce projet a été généré par **Calque**. Il contient un front **React + Vite + Tailwind**
et un back **Express**, déjà organisés. À vous de coder : tout est prêt à être modifié.

> Pas de lock-in : c'est un projet npm tout à fait standard. Vous pouvez tout changer.

## Prérequis

- [Node.js](https://nodejs.org) **18+** (qui inclut `npm`).

## Démarrer en 30 secondes

```bash
npm install          # installe le front ET le back (npm workspaces)
cp .env.example .env # crée votre config locale
npm run dev          # lance le front (5173) et le back (3001) en parallèle
```

- Front : http://localhost:5173
- API : http://localhost:3001/api/health

## Structure du projet

```
.
├─ package.json          ← scripts globaux + workspaces (client + server)
├─ .env.example          ← variables d'environnement à copier en .env
│
├─ client/               ← FRONT — React + Vite + Tailwind
│  ├─ index.html
│  ├─ tailwind.config.js ← votre thème (couleurs, polices) choisi dans Calque
│  └─ src/
│     ├─ main.tsx        ← point d'entrée React
│     ├─ App.tsx         ← assemble les sections de la page
│     ├─ index.css       ← directives Tailwind + import des polices
│     ├─ components/
│     │  ├─ layout/      ← Navbar, Footer (présents sur toutes les pages)
│     │  ├─ sections/    ← Hero, Features, CTA… (les « briques » de la page)
│     │  └─ ui/          ← petits composants réutilisables (Button…)
│     └─ lib/
│        └─ api.ts       ← appels vers l'API back
│
└─ server/               ← BACK — Express
   └─ src/
      ├─ index.ts        ← création du serveur + montage des routes
      ├─ routes/         ← une route = un fichier (ex. health.ts)
      └─ middleware/     ← middlewares Express (ex. gestion d'erreurs)
```

## Comment faire…

### …ajouter une section à la page d'accueil
1. Créez `client/src/components/sections/MaSection.tsx`.
2. Importez-la dans `client/src/App.tsx` et placez-la dans `<main>`.

### …changer les couleurs / la typo
Tout est dans `client/tailwind.config.js`, section `theme.extend`. Modifiez les
valeurs de `colors` et `fontFamily` : elles s'appliquent à tout le projet.

### …ajouter une route API
1. Créez `server/src/routes/maRoute.ts` (copiez `health.ts` comme modèle).
2. Montez-la dans `server/src/index.ts` : `app.use('/api/ma-route', maRouteRouter);`
3. Appelez-la depuis le front via `client/src/lib/api.ts`.

### …brancher une base de données (plus tard)
Le starter n'inclut volontairement **pas** de BDD (hors MVP). Quand vous serez
prêt, ajoutez l'ORM de votre choix (Prisma, Drizzle, Knex…) dans `server/`,
mettez la connexion dans `.env` (`DATABASE_URL`) et créez un dossier
`server/src/db/` + vos migrations.

## Scripts disponibles (à la racine)

| Commande        | Effet                                            |
| --------------- | ------------------------------------------------ |
| `npm run dev`   | Lance front + back en mode développement         |
| `npm run build` | Build de production du front et du back           |
| `npm run start` | Démarre le serveur en production                  |

---

Généré avec ❤️ par **Calque**.
