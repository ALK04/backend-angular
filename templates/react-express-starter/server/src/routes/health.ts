import { Router } from 'express';

export const healthRouter = Router();

// GET /api/health → vérifie que l'API répond
healthRouter.get('/', (_req, res) => {
  res.json({ status: 'ok' });
});
