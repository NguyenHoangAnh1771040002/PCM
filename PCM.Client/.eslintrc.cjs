module.exports = {
  root: true,
  env: {
    node: true,
    'vue/setup-compiler-macros': true
  },
  extends: [
    'plugin:vue/vue3-essential'
  ],
  parserOptions: {
    ecmaVersion: 2022
  },
  rules: {
    // Vuetify 3 uses v-slot:item.xxx syntax which is valid
    'vue/valid-v-slot': ['error', {
      allowModifiers: true
    }],
    // Allow unused vars with underscore prefix
    'no-unused-vars': ['warn', { 'argsIgnorePattern': '^_' }]
  }
}
