/** Petit client HTTP vers l'API Express. */
const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:3001';

export async function getHealth(): Promise<{ status: string }> {
  const res = await fetch(`${API_URL}/api/health`);
  if (!res.ok) {
    throw new Error(`Requête échouée (${res.status})`);
  }
  return res.json();
}
