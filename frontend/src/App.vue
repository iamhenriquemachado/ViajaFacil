<script setup>
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import authService from './services/authService';

const isLoading = ref(true);
const router = useRouter();

onMounted(async () => {
  try {
    // Verificar status de autenticação ao carregar o aplicativo
    const authStatus = await authService.checkAuthStatus();
    
    // Aguardar a verificação de autenticação antes de mostrar conteúdo
    isLoading.value = false;
    
    // Se estiver autenticado e estiver na rota de login, redirecionar
    if (authStatus.isAuthenticated && router.currentRoute.value.path === '/') {
      router.push('/dashboard');
    }
  } catch (error) {
    console.error('Error checking authentication:', error);
    isLoading.value = false;
  }
});
</script>

<template>
  <div id="app">
    <div v-if="isLoading" class="loading-container">
      <div class="loader"></div>
      <p>Carregando...</p>
    </div>
    <router-view v-else />
  </div>
</template>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  background-color: #f9fafb;
}

.loading-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100vh;
  color: #4b5563;
}

.loader {
  border: 4px solid #f3f3f3;
  border-radius: 50%;
  border-top: 4px solid #3b82f6;
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;
  margin-bottom: 16px;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
</style>