/**
 * Thème du projet — généré par Calque.
 * Modifiez librement : ces valeurs s'appliquent à tout le front.
 * @type {import('tailwindcss').Config}
 */
export default {
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        brand: {
          DEFAULT: '#5b3df5',
          dark: '#3a1fd0',
        },
        ink: '#14121f',
      },
      fontFamily: {
        display: ['"Space Grotesk"', 'system-ui', 'sans-serif'],
        sans: ['Inter', 'system-ui', 'sans-serif'],
      },
    },
  },
  plugins: [],
};
