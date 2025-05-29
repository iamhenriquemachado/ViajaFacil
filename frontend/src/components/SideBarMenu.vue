<script setup>
import { ref } from 'vue'
import {
  UserIcon,
  MapPinIcon,
  CalendarIcon,
  SquareChevronRight
} from 'lucide-vue-next'

const openSection = ref(null)

function toggleSection(section) {
  openSection.value = openSection.value === section ? null : section
}
</script>

<template>
  <aside class="sidebar">
    <h2 class="title">Menu</h2>
    <div class="sidenav">

      <!-- Usuários -->
      <div class="menu-section">
        <div @click="toggleSection('usuarios')" class="menu-header">
          <UserIcon class="icon" />
          <span>Usuários</span>
          <SquareChevronRight
            class="chevron"
            :class="{ rotated: openSection === 'usuarios' }"
          />
        </div>
        <transition name="fade-slide">
          <ul v-show="openSection === 'usuarios'" class="submenu">
            <li><router-link to="/dashboard/create-user">Criar Usuário</router-link></li>
            <li><router-link to="/dashboard/list-user">Listagem Usuários</router-link></li>
          </ul>
        </transition>
      </div>

      <!-- Destinos -->
      <div class="menu-section">
        <div @click="toggleSection('destinos')" class="menu-header">
          <MapPinIcon class="icon" />
          <span>Destinos</span>
          <SquareChevronRight
            class="chevron"
            :class="{ rotated: openSection === 'destinos' }"
          />
        </div>
        <transition name="fade-slide">
          <ul v-show="openSection === 'destinos'" class="submenu">
            <li><router-link to="/dashboard/create-destine">Criar Destino</router-link></li>
            <li><router-link to="/dashboard/list-destine">Listar Destinos</router-link></li>
          </ul>
        </transition>
      </div>

      <!-- Reservas -->
      <div class="menu-section">
        <div @click="toggleSection('reservas')" class="menu-header">
          <CalendarIcon class="icon" />
          <span>Reservas</span>
          <SquareChevronRight
            class="chevron"
            :class="{ rotated: openSection === 'reservas' }"
          />
        </div>
        <transition name="fade-slide">
          <ul v-show="openSection === 'reservas'" class="submenu">
            <li><router-link to="/dashboard/manage-reserve">Gerenciar Reservas</router-link></li>
          </ul>
        </transition>
      </div>

    </div>
  </aside>
</template>

<style scoped>
.sidebar {
  width: 260px;
  height: 100vh;
  background-color: #ffffff;
  color: #111827;
  border-right: 1px solid #e5e7eb;
  padding: 24px 20px;
  box-sizing: border-box;
  display: flex;
  flex-direction: column;
  font-family: 'Inter', sans-serif;
}

.title {
  font-size: 1.5rem;
  font-weight: 600;
  margin-bottom: 24px;
  color: #111827;
}

.menu-section {
  margin-bottom: 20px;
}

.menu-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  background-color: #f9fafb;
  padding: 10px 12px;
  border-radius: 8px;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.menu-header:hover {
  background-color: #f3f4f6;
}

.icon {
  width: 20px;
  height: 20px;
  color: #374151;
}

.menu-header span {
  flex: 1;
  margin-left: 10px;
  color: #111827;
  font-weight: 500;
  font-size: 0.95rem;
}

.chevron {
  width: 20px;
  height: 20px;
  color: #6b7280;
  transition: transform 0.3s ease;
}

.chevron.rotated {
  transform: rotate(90deg);
}

.submenu {
  margin-top: 12px;
  padding-left: 20px;
}

.submenu li {
  list-style: none;
  margin: 8px 0;
}

.submenu a {
  text-decoration: none;
  color: #374151;
  font-size: 0.9rem;
  font-weight: 500;
  transition: color 0.2s;
}

.submenu a:hover {
  color: #111827;
  text-decoration: underline;
}

.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: all 0.3s ease;
}

.fade-slide-enter-from,
.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(-8px);
}
</style>
