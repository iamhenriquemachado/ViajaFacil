<script setup>
import { useRouter } from 'vue-router'
import { ref } from 'vue'
import authService from '../services/authService'

const router = useRouter()
const email = ref('')
const password = ref('')
const errorMessage = ref('')
const isLoading = ref(false)

async function handleLogin() {
  if (!email.value.includes('@') || password.value.length < 8) {
    errorMessage.value = 'Por favor, insira um email válido e uma senha com pelo menos 8 caracteres.'
    return
  }

  isLoading.value = true
  errorMessage.value = ''

  try {
    const result = await authService.login(email.value, password.value)
    
    if (result.success) {
      if (result.isAdmin === true) {
        router.push('/dashboard')
      } else {
        console.log("Common user")
        // Redirecionar para área de usuário comum se necessário
      }
    } else {
      errorMessage.value = result.error || 'Falha no login. Por favor, tente novamente.'
    }
  } catch (error) {
    console.error('Network or server error:', error)
    errorMessage.value = 'Não foi possível conectar ao servidor. Por favor, tente novamente mais tarde.'
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="login-page">
    <div class="login-box">
      <h1>ViajaFacil ✈️</h1>
      <form @submit.prevent="handleLogin">
        <div class="form-group">
          <label for="email">E-mail</label>
          <input 
            type="email" 
            id="email" 
            v-model="email" 
            placeholder="you@example.com" 
            required 
          />
        </div>
        <div class="form-group">
          <label for="password">Senha</label>
          <input 
            type="password" 
            id="password" 
            v-model="password" 
            placeholder="••••••••" 
            required 
          />
        </div>
        <div v-if="errorMessage" class="error-message">
          {{ errorMessage }}
        </div>
        <button 
          type="submit" 
          class="btn" 
          :disabled="isLoading"
        >
          {{ isLoading ? 'Entrando...' : 'Entrar' }}
        </button>
      </form>
      <p class="register-text">
        Não tem conta? <a href="#">Crie uma agora</a>
      </p>
    </div>
  </div>
</template>

<style scoped>
.login-page {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: #f9fafb;
  padding: 20px;
}
.login-box {
  background: white;
  padding: 32px;
  border-radius: 16px;
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.05);
  width: 100%;
  max-width: 380px;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}
h1 {
  font-size: 1.6rem;
  color: #111827;
  margin-bottom: 24px;
  text-align: center;
}
.form-group {
  display: flex;
  flex-direction: column;
  margin-bottom: 18px;
}
label {
  font-size: 0.9rem;
  color: #4b5563;
  margin-bottom: 6px;
  font-weight: 500;
}
input {
  padding: 12px;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  font-size: 1rem;
  transition: all 0.2s;
  background-color: #f9fafb;
}
input:focus {
  outline: none;
  border-color: #3b82f6;
  background-color: white;
  box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.2);
}
.btn {
  width: 100%;
  padding: 12px;
  background-color: #3b82f6;
  color: white;
  font-weight: 600;
  border: none;
  border-radius: 10px;
  font-size: 1rem;
  cursor: pointer;
  transition: background 0.3s ease;
}
.btn:hover:not([disabled]) {
  background-color: #2563eb;
}
.btn[disabled] {
  background-color: #93c5fd;
  cursor: not-allowed;
}
.register-text {
  text-align: center;
  margin-top: 16px;
  font-size: 0.9rem;
  color: #6b7280;
}
.register-text a {
  color: #3b82f6;
  font-weight: 500;
  text-decoration: none;
}
.register-text a:hover {
  text-decoration: underline;
}
.error-message {
  color: #dc2626;
  font-size: 0.9rem;
  margin-bottom: 16px;
  background-color: #fee2e2;
  padding: 8px 12px;
  border-radius: 8px;
}
</style>