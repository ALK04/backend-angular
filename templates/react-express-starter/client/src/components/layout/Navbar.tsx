import Button from '../ui/Button';

export default function Navbar() {
  return (
    <header className="sticky top-0 z-50 border-b border-ink/10 bg-white/80 backdrop-blur">
      <div className="mx-auto flex h-16 max-w-6xl items-center gap-8 px-6">
        <a href="/" className="font-display text-lg font-bold">
          Mon site
        </a>
        <nav className="ml-auto hidden gap-7 text-sm text-ink/70 sm:flex">
          <a href="#features" className="hover:text-ink">Fonctionnalités</a>
          <a href="#cta" className="hover:text-ink">Contact</a>
        </nav>
        <Button>Commencer</Button>
      </div>
    </header>
  );
}
