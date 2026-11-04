import Button from '../ui/Button';

export default function CTA() {
  return (
    <section id="cta" className="mx-auto max-w-6xl px-6 py-20">
      <div className="rounded-3xl bg-ink px-8 py-14 text-center text-white">
        <h2 className="font-display text-3xl font-bold">Prêt à vous lancer ?</h2>
        <p className="mx-auto mt-3 max-w-md text-white/70">
          Éditez cette section dans <code>client/src/components/sections/CTA.tsx</code>.
        </p>
        <div className="mt-7 flex justify-center">
          <Button>Commencer maintenant</Button>
        </div>
      </div>
    </section>
  );
}
