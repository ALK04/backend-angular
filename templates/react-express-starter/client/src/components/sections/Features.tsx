interface Feature {
  title: string;
  description: string;
}

const features: Feature[] = [
  { title: 'Rapide', description: 'Construit avec Vite : démarrage instantané et rechargement à chaud.' },
  { title: 'Modulaire', description: 'Des composants bien rangés, prêts à être étendus.' },
  { title: 'À vous', description: 'Un projet standard, sans lock-in. Vous possédez le code.' },
];

export default function Features() {
  return (
    <section id="features" className="mx-auto max-w-6xl px-6 py-20">
      <h2 className="font-display text-3xl font-bold">Pourquoi ce starter</h2>
      <div className="mt-10 grid gap-6 sm:grid-cols-3">
        {features.map((feature) => (
          <article key={feature.title} className="rounded-2xl border border-ink/10 p-6">
            <h3 className="font-display text-xl font-semibold">{feature.title}</h3>
            <p className="mt-2 text-ink/70">{feature.description}</p>
          </article>
        ))}
      </div>
    </section>
  );
}
