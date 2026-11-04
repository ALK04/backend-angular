import Button from '../ui/Button';

export default function Hero() {
  return (
    <section className="mx-auto max-w-6xl px-6 py-24 text-center">
      <p className="font-display text-sm font-semibold uppercase tracking-widest text-brand">
        Bienvenue
      </p>
      <h1 className="mx-auto mt-4 max-w-3xl font-display text-5xl font-bold leading-tight">
        Votre nouveau site commence ici.
      </h1>
      <p className="mx-auto mt-5 max-w-xl text-lg text-ink/70">
        Ce projet a été généré pour vous. Modifiez ce hero, ajoutez vos sections,
        et lancez votre idée.
      </p>
      <div className="mt-8 flex justify-center gap-3">
        <Button>Commencer</Button>
        <Button variant="ghost">En savoir plus</Button>
      </div>
    </section>
  );
}
