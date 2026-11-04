import express from 'express';
import cors from 'cors';
import { healthRouter } from './routes/health';
import { errorHandler } from './middleware/errorHandler';

const app = express();
const PORT = process.env.PORT ?? 3001;

app.use(cors());
app.use(express.json());

// Routes (une route = un fichier dans ./routes)
app.use('/api/health', healthRouter);

// Gestion centralisée des erreurs (toujours en dernier)
app.use(errorHandler);

app.listen(PORT, () => {
  console.log(`API prête sur http://localhost:${PORT}`);
});
