/** @type {import('tailwindcss').Config} */
const defaultTheme = require('tailwindcss/defaultTheme')
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      gridTemplateColumns: {
        fluid: "repeat(auto-fit, minmax(11rem, 1fr))",
      },
      fontFamily: {
        sans: ["Poppins","Noto Sans Thai", ...defaultTheme.fontFamily.sans],
      },
    },
  },
  plugins: [],
  corePlugins: {
    preflight: false,
  },
};