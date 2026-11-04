export default function Footer() {
  return (
    <footer className="border-t border-ink/10 py-10">
      <div className="mx-auto flex max-w-6xl flex-wrap items-center gap-4 px-6 text-sm text-ink/60">
        <span className="font-display font-bold text-ink">Mon site</span>
        <span>© {new Date().getFullYear()}</span>
        <nav className="ml-auto flex gap-5">
          <a href="#features" className="hover:text-ink">Fonctionnalités</a>
          <a href="#cta" className="hover:text-ink">Contact</a>
        </nav>
      </div>
    </footer>
  );
}
