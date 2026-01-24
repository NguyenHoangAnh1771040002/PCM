// @ts-ignore
import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

export default createVuetify({
  components,
  directives,
  theme: {
    defaultTheme: 'light',
    themes: {
      light: {
        dark: false,
        colors: {
          primary: '#4CAF50',     // Màu xanh lá (pickleball)
          secondary: '#FFC107',   // Màu vàng
          accent: '#2196F3',      // Màu xanh dương
          error: '#F44336',
          warning: '#FF9800',
          info: '#03A9F4',
          success: '#4CAF50',
          background: '#F5F5F5',
          surface: '#FFFFFF'
        }
      },
      dark: {
        dark: true,
        colors: {
          primary: '#66BB6A',
          secondary: '#FFD54F',
          accent: '#42A5F5',
          background: '#121212',
          surface: '#1E1E1E'
        }
      }
    }
  },
  defaults: {
    VBtn: {
      rounded: 'lg',
      elevation: 2
    },
    VCard: {
      rounded: 'lg',
      elevation: 2
    },
    VTextField: {
      variant: 'outlined',
      density: 'comfortable'
    }
  }
})
