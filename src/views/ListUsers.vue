<script setup>
import { ref, onMounted, computed } from 'vue'
import { Trash2Icon, PencilIcon, SearchIcon, XIcon, AlertTriangleIcon } from 'lucide-vue-next'

const users = ref([])
const searchQuery = ref('')
const showModal = ref(false)
const showDeleteModal = ref(false) 
const editingUser = ref(null)
const deletingUser = ref(null)
const isLoading = ref(false)
const updateMessage = ref({ text: '', type: '' })

// Form data para edição
const editForm = ref({
  name: '',
  email: '',
  isAdmin: false
})

// Usuários filtrados com base na pesquisa
const filteredUsers = computed(() => {
  const query = searchQuery.value.toLowerCase().trim()
  if (!query) return users.value
  
  return users.value.filter(user => 
    user.name.toLowerCase().includes(query) || 
    user.email.toLowerCase().includes(query) ||
    user.id.toString().includes(query)
  )
})

async function listUsers() {
  try {
    const response = await fetch("https://localhost:7073/api/users/listusers", {
      method: "GET",
      credentials: "include"
    });
    if (!response.ok) {
      throw new Error("Erro no servidor: " + response.status);
    }
    const data = await response.json();
    users.value = data;
  } catch (error) {
    console.error("Erro de rede ou servidor:", error);
  }
}

// Função para abrir o modal de confirmação de deleção
function openDeleteModal(user) {
  deletingUser.value = user
  showDeleteModal.value = true
  // Limpar qualquer mensagem anterior ao abrir o modal
  updateMessage.value = { text: '', type: '' }
}

// Função para fechar o modal de confirmação de deleção
function closeDeleteModal() {
  showDeleteModal.value = false
  updateMessage.value = { text: '', type: '' } // Limpar a mensagem ao fechar o modal
  setTimeout(() => {
    deletingUser.value = null
  }, 300) // Pequeno atraso para animação de fechamento
}

async function deleteUser() {
  if (!deletingUser.value) return
 
  isLoading.value = true
 
  try {
    const response = await fetch(`https://localhost:7073/api/users/deleteuser/${deletingUser.value.id}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
    });
   
    if (!response.ok) {
      throw new Error("Erro no servidor: " + response.status);
    }
   
    const index = users.value.findIndex(u => u.id === deletingUser.value.id)
    if (index !== -1) {
      users.value.splice(index, 1); // Remove o usuário do array
    }
   
    updateMessage.value = { text: 'Usuário deletado com sucesso!', type: 'success' }
   
    // Fechar o modal após deleção bem-sucedida
    setTimeout(() => {
      closeDeleteModal()
    }, 1500)
   
  } catch (error) {
    console.error("Erro ao deletar usuário:", error);
    updateMessage.value = { text: 'Erro ao deletar usuário.', type: 'error' }
  } finally {
    isLoading.value = false
  }
}

function openEditModal(user) {
  editingUser.value = user
  editForm.value.name = user.name
  editForm.value.email = user.email
  editForm.value.isAdmin = user.isAdmin || false
  showModal.value = true
}

function closeModal() {
  showModal.value = false
  updateMessage.value = { text: '', type: '' }
  setTimeout(() => {
    editingUser.value = null
  }, 300) // Pequeno atraso para animação de fechamento
}

async function updateUser() {
  if (!editingUser.value) return
  
  isLoading.value = true
  
  try {
    const response = await fetch(`https://localhost:7073/api/users/updateuser/${editingUser.value.id}`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include", 
      body: JSON.stringify({
        name: editForm.value.name,
        email: editForm.value.email,
        isAdmin: editForm.value.isAdmin
      }),
    });
    
    if (!response.ok) {
      throw new Error("Erro no servidor: " + response.status);
    }
    
    const updatedUser = await response.json();
    
    // Atualiza o usuário na lista
    const index = users.value.findIndex(u => u.id === editingUser.value.id)
    if (index !== -1) {
      users.value[index] = { ...users.value[index], ...updatedUser }
    }
    
    updateMessage.value = { text: 'Usuário atualizado com sucesso!', type: 'success' }
    
    // Fechar o modal após atualização bem-sucedida
    setTimeout(() => {
      closeModal()
    }, 1500)
    
  } catch (error) {
    console.error("Erro ao atualizar usuário:", error);
    updateMessage.value = { text: 'Erro ao atualizar usuário.', type: 'error' }
  } finally {
    isLoading.value = false
  }
}

// Fecha os modais ao pressionar ESC
function handleKeyDown(e) {
  if (e.key === 'Escape') {
    if (showModal.value) {
      closeModal()
    }
    if (showDeleteModal.value) {
      closeDeleteModal()
    }
  }
}

onMounted(() => {
  listUsers()
  window.addEventListener('keydown', handleKeyDown)
})
</script>

<template>
  <div class="container">
    <div class="header">
      <h1 class="heading">Usuários</h1>
      
      <div class="search-container">
        <SearchIcon size="16" class="search-icon" />
        <input 
          type="text" 
          v-model="searchQuery" 
          placeholder="Buscar usuários..." 
          class="search-input"
        />
      </div>
    </div>
    
    <div class="empty-state" v-if="filteredUsers.length === 0">
      <p>Nenhum usuário encontrado.</p>
    </div>
    
    <div class="grid" v-else>
      <div class="card" v-for="user in filteredUsers" :key="user.id">
        <div class="content">
          <h3 class="name">{{ user.name }}</h3>
          <p class="email">{{ user.email }}</p>
          <p class="id">ID: {{ user.id }}</p>
          <p class="admin-status">
            <span :class="['badge', user.isAdmin ? 'admin' : 'user']">
              {{ user.isAdmin ? 'Admin' : 'Usuário' }}
            </span>
          </p>
        </div>
        
        <div class="actions">
          <button class="action-btn edit" @click="openEditModal(user)" aria-label="Editar usuário">
            <PencilIcon size="18" />
          </button>
          
          <button class="action-btn delete" @click="openDeleteModal(user)" aria-label="Deletar usuário">
            <Trash2Icon size="18" />
          </button>
        </div>
      </div>
    </div>
    
    <!-- Modal de edição -->
    <div class="modal-overlay" v-if="showModal" @click="closeModal"></div>
    
    <div class="modal" v-if="showModal" :class="{ 'modal-active': showModal }">
      <div class="modal-header">
        <h2 class="modal-title">Editar Usuário</h2>
        <button class="close-btn" @click="closeModal" aria-label="Fechar modal">
          <XIcon size="20" />
        </button>
      </div>
      
      <div class="modal-body">
        <div class="alert" v-if="updateMessage.text" :class="updateMessage.type">
          {{ updateMessage.text }}
        </div>
        
        <form @submit.prevent="updateUser">
          <div class="field">
            <label for="edit-name">Nome</label>
            <input
              type="text"
              id="edit-name"
              v-model="editForm.name"
              required
              placeholder="Nome do usuário"
            />
          </div>
          
          <div class="field">
            <label for="edit-email">E-mail</label>
            <input
              type="email"
              id="edit-email"
              v-model="editForm.email"
              required
              placeholder="E-mail do usuário"
            />
          </div>
          
          <div class="field">
            <label for="edit-admin">Administrador</label>
            <select
              id="edit-admin"
              v-model="editForm.isAdmin"
              required
            >
              <option :value="true">Sim</option>
              <option :value="false">Não</option>
            </select>
          </div>
          
          <div class="modal-actions">
            <button type="button" class="cancel-btn" @click="closeModal" :disabled="isLoading">
              Cancelar
            </button>
            <button type="submit" class="save-btn" :disabled="isLoading">
              {{ isLoading ? 'Salvando...' : 'Salvar' }}
            </button>
          </div>
        </form>
      </div>
    </div>
    
    <!-- Modal de confirmação de deleção -->
    <div class="modal-overlay" v-if="showDeleteModal" @click="closeDeleteModal"></div>
    
    <div class="modal delete-modal" v-if="showDeleteModal" :class="{ 'modal-active': showDeleteModal }">
      <div class="modal-header delete-header">
        <div class="delete-title-wrapper">
          <AlertTriangleIcon size="20" class="delete-icon" />
          <h2 class="modal-title">Confirmar Exclusão</h2>
        </div>
        <button class="close-btn" @click="closeDeleteModal" aria-label="Fechar modal">
          <XIcon size="20" />
        </button>
      </div>
      
      <div class="modal-body">
        <div class="alert" v-if="updateMessage.text" :class="updateMessage.type">
          {{ updateMessage.text }}
        </div>
        
        <div class="delete-confirmation" v-if="deletingUser">
          <p>Tem certeza que deseja excluir o usuário <strong>{{ deletingUser.name }}</strong>?</p>
          <p class="delete-warning">Esta ação não pode ser desfeita.</p>
        </div>
        
        <div class="modal-actions">
          <button type="button" class="cancel-btn" @click="closeDeleteModal" :disabled="isLoading">
            Cancelar
          </button>
          <button type="button" class="delete-btn" @click="deleteUser" :disabled="isLoading">
            {{ isLoading ? 'Excluindo...' : 'Excluir' }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
  font-family: system-ui, -apple-system, sans-serif;
}

.header {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

@media (min-width: 640px) {
  .header {
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
  }
}

.heading {
  font-size: 1.5rem;
  font-weight: 500;
  color: #333;
  letter-spacing: -0.01em;
  margin: 0;
}

.search-container {
  position: relative;
  max-width: 320px;
  width: 100%;
}

.search-icon {
  position: absolute;
  top: 50%;
  left: 0.75rem;
  transform: translateY(-50%);
  color: #888;
}

.search-input {
  width: 100%;
  padding: 0.625rem 0.625rem 0.625rem 2.25rem;
  font-size: 0.875rem;
  border-radius: 6px;
  border: 1px solid #e5e7eb;
  background-color: #fafafa;
  transition: all 0.2s ease;
}

.search-input:focus {
  outline: none;
  border-color: #aaa;
  background-color: white;
  box-shadow: 0 0 0 2px rgba(0, 0, 0, 0.03);
}

.search-input::placeholder {
  color: #aaa;
}

.empty-state {
  text-align: center;
  padding: 2rem;
  background-color: #fafafa;
  border-radius: 8px;
  color: #666;
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1rem;
}

.card {
  background: white;
  border-radius: 8px;
  padding: 1.25rem;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
  transition: all 0.2s ease;
}

.card:hover {
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
}

.content {
  flex: 1;
}

.name {
  font-size: 1rem;
  font-weight: 500;
  color: #111;
  margin: 0 0 0.25rem;
}

.email {
  font-size: 0.875rem;
  color: #555;
  margin: 0 0 0.5rem;
}

.id {
  font-size: 0.75rem;
  color: #888;
  margin: 0 0 0.5rem;
}

.admin-status {
  margin: 0;
}

.badge {
  display: inline-block;
  font-size: 0.75rem;
  font-weight: 500;
  padding: 0.125rem 0.5rem;
  border-radius: 12px;
}

.badge.admin {
  background-color: rgba(59, 130, 246, 0.1);
  color: #3b82f6;
}

.badge.user {
  background-color: rgba(107, 114, 128, 0.1);
  color: #6b7280;
}

.actions {
  display: flex;
  gap: 0.5rem;
}

.action-btn {
  background: transparent;
  border: none;
  cursor: pointer;
  color: #aaa;
  padding: 0.25rem;
  border-radius: 4px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
}

.edit:hover {
  color: #3b82f6;
  background: rgba(59, 130, 246, 0.05);
}

.delete:hover {
  color: #ef4444;
  background: rgba(239, 68, 68, 0.05);
}

/* Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.4);
  z-index: 50;
  backdrop-filter: blur(2px);
}

.modal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 90%;
  max-width: 480px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  z-index: 100;
  opacity: 0;
  transition: opacity 0.2s ease, transform 0.2s ease;
  transform: translate(-50%, -48%);
}

.modal-active {
  opacity: 1;
  transform: translate(-50%, -50%);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  border-bottom: 1px solid #f1f1f1;
}

.delete-header {
  border-bottom-color: #fee2e2;
}

.delete-title-wrapper {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.delete-icon {
  color: #ef4444;
}

.modal-title {
  font-size: 1.125rem;
  font-weight: 500;
  color: #333;
  margin: 0;
}

.close-btn {
  background: transparent;
  border: none;
  color: #666;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  padding: 0.25rem;
  border-radius: 4px;
  transition: all 0.2s ease;
}

.close-btn:hover {
  background-color: #f3f4f6;
  color: #333;
}

.modal-body {
  padding: 1.25rem;
}

.delete-confirmation {
  text-align: center;
  margin-bottom: 1rem;
}

.delete-warning {
  font-size: 0.875rem;
  color: #888;
  margin-top: 0.5rem;
}

.field {
  margin-bottom: 1.25rem;
}

select {
  width: 100%;
  padding: 0.625rem;
  font-size: 0.875rem;
  border-radius: 6px;
  border: 1px solid #e5e7eb;
  background-color: #fafafa;
  color: #333;
  transition: all 0.2s ease;
  appearance: auto;
}

select:focus {
  outline: none;
  border-color: #aaa;
  background-color: white;
  box-shadow: 0 0 0 2px rgba(0, 0, 0, 0.03);
}

label {
  display: block;
  font-size: 0.875rem;
  font-weight: 500;
  color: #555;
  margin-bottom: 0.375rem;
}

input {
  width: 95%;
  padding: 0.625rem;
  font-size: 0.875rem;
  border-radius: 6px;
  border: 1px solid #e5e7eb;
  background-color: #fafafa;
  color: #333;
  transition: all 0.2s ease;
}

input:focus {
  outline: none;
  border-color: #aaa;
  background-color: white;
  box-shadow: 0 0 0 2px rgba(0, 0, 0, 0.03);
}

input::placeholder {
  color: #aaa;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
  margin-top: 1.5rem;
}

.cancel-btn {
  padding: 0.625rem 1rem;
  font-size: 0.875rem;
  font-weight: 500;
  color: #666;
  background-color: #f5f5f5;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.cancel-btn:hover {
  background-color: #e5e5e5;
}

.save-btn {
  padding: 0.625rem 1rem;
  font-size: 0.875rem;
  font-weight: 500;
  color: white;
  background-color: #333;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.save-btn:hover {
  background-color: #000;
}

.delete-btn {
  padding: 0.625rem 1rem;
  font-size: 0.875rem;
  font-weight: 500;
  color: white;
  background-color: #ef4444;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.delete-btn:hover {
  background-color: #dc2626;
}

.save-btn:disabled, 
.cancel-btn:disabled,
.delete-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.alert {
  padding: 0.75rem;
  border-radius: 6px;
  margin-bottom: 1rem;
  font-size: 0.875rem;
}

.alert.success {
  background-color: rgba(16, 185, 129, 0.1);
  color: #10b981;
}

.alert.error {
  background-color: rgba(239, 68, 68, 0.1);
  color: #ef4444;
}
</style>