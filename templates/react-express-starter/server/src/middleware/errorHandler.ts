import type { NextFunction, Request, Response } from 'express';

/** Middleware de gestion d'erreurs : renvoie une réponse JSON propre. */
export function errorHandler(err: unknown, _req: Request, res: Response, _next: NextFunction) {
  console.error(err);
  res.status(500).json({ error: 'Erreur serveur' });
}
