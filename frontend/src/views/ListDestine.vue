<script setup>
import { ref, onMounted, computed, onUnmounted } from 'vue'
import { Trash2Icon, PencilIcon, SearchIcon, XIcon, AlertTriangleIcon } from 'lucide-vue-next'

const destines = ref([])
const searchQuery = ref('')
const showModal = ref(false)
const showDeleteModal = ref(false) 
const editingDestine = ref(null)
const deletingDestine = ref(null)
const isLoading = ref(false)
const updateMessage = ref({ text: '', type: '' })

const editForm = ref({
  name: '',
  description: '',
  imageUrl: '',
  country: '',
  state: '', 
  city: '', 
  address: '',
  latitude: '',
  longitude: ''
})

// Função para limpar o formulário
function clearForm() {
  editForm.value = {
    name: '',
    description: '',
    imageUrl: '',
    country: '',
    state: '', 
    city: '', 
    address: '',
    latitude: '',
    longitude: ''
  }
}

const filtereddestines = computed(() => {
  const query = searchQuery.value.toLowerCase().trim()
  if (!query) return destines.value
  
  return destines.value.filter(user => 
    user.name.toLowerCase().includes(query) || 
    user.description.toLowerCase().includes(query) ||
    user.country.toString().includes(query)
  )
})

async function listDestines() {
  try {
    const response = await fetch("https://localhost:7073/api/destines/listdestines", {
      method: "GET",
      credentials: "include"
    });
    if (!response.ok) {
      throw new Error("Erro no servidor: " + response.status);
    }
    const data = await response.json();
    destines.value = data
    
  } catch (error) {
    console.error("Erro de rede ou servidor:", error);
  }
}

// Função para abrir o modal de confirmação de deleção
function openDeleteModal(destine) {
  deletingDestine.value = destine
  showDeleteModal.value = true
  // Limpar qualquer mensagem anterior ao abrir o modal
  updateMessage.value = { text: '', type: '' }
}

// Função para fechar o modal de confirmação de deleção
function closeDeleteModal() {
  showDeleteModal.value = false
  updateMessage.value = { text: '', type: '' }
  setTimeout(() => {
    deletingDestine.value = null
  }, 300)
}

async function deleteDestine() {
  if (!deletingDestine.value) return
 
  isLoading.value = true
 
  try {
    const response = await fetch(`https://localhost:7073/api/destines/deletedestine/${deletingDestine.value.id}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
    });
   
    if (!response.ok) {
      throw new Error("Erro no servidor: " + response.status);
    }
   
    const index = destines.value.findIndex(u => u.id === deletingDestine.value.id)
    if (index !== -1) {
      destines.value.splice(index, 1);
    }
   
    updateMessage.value = { text: 'Destino deletado com sucesso!', type: 'success' }
   
    // Fechar o modal após deleção bem-sucedida
    setTimeout(() => {
      closeDeleteModal()
    }, 1500)
   
  } catch (error) {
    console.error("Erro ao deletar destino:", error);
    updateMessage.value = { text: 'Erro ao deletar destino.', type: 'error' }
  } finally {
    isLoading.value = false
  }
}

function openEditModal(destine) {
  editingDestine.value = destine 
  editForm.value.name = destine.name || ''
  editForm.value.description = destine.description || ''
  editForm.value.imageUrl = destine.imageUrl || ''
  editForm.value.country = destine.country || ''
  editForm.value.state = destine.state || ''
  editForm.value.city = destine.city || ''
  editForm.value.address = destine.address || ''
  editForm.value.latitude = destine.latitude ? destine.latitude.toString() : ''
  editForm.value.longitude = destine.longitude ? destine.longitude.toString() : ''
  showModal.value = true
  // Limpar mensagens anteriores
  updateMessage.value = { text: '', type: '' }
}

function closeModal() {
  showModal.value = false
  updateMessage.value = { text: '', type: '' }
  setTimeout(() => {
    editingDestine.value = null
    clearForm() // Limpar o formulário ao fechar
  }, 300)
}

async function updateDestine() {
  if (!editingDestine.value) {
    console.error('Nenhum destino selecionado para edição')
    updateMessage.value = { text: 'Erro: Nenhum destino selecionado.', type: 'error' }
    return
  }
  
  isLoading.value = true
  
  try {
    const requestBody = {
      name: editForm.value.name,
      description: editForm.value.description,
      imageUrl: editForm.value.imageUrl,
      country: editForm.value.country, 
      state: editForm.value.state, 
      city: editForm.value.city,
      address: editForm.value.address, 
      latitude: editForm.value.latitude ? parseFloat(editForm.value.latitude) : 0,
      longitude: editForm.value.longitude ? parseFloat(editForm.value.longitude) : 0
    }

    console.log('Enviando dados:', requestBody) // Debug

    const response = await fetch(`https://localhost:7073/api/destines/updatedestine/${editingDestine.value.id}`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include", 
      body: JSON.stringify(requestBody),
    });
    
    if (!response.ok) {
      const errorData = await response.text();
      throw new Error(`Erro no servidor: ${response.status} - ${errorData}`);
    }
    
    // Verificar se a resposta tem conteúdo JSON
    let updatedDestine = null
    const contentType = response.headers.get('content-type')
    if (contentType && contentType.includes('application/json')) {
      updatedDestine = await response.json()
    }
    
    // Atualiza o destino na lista
    const index = destines.value.findIndex(d => d.id === editingDestine.value.id)
    if (index !== -1) {
      // Se a API retornou dados atualizados, usa eles, senão usa os dados do formulário
      const dataToUpdate = updatedDestine || requestBody
      destines.value[index] = { 
        ...destines.value[index], 
        ...dataToUpdate,
        id: editingDestine.value.id // Garantir que o ID permaneça
      }
    }
    
    console.log('Destino atualizado com sucesso') // Debug
    updateMessage.value = { text: 'Destino atualizado com sucesso!', type: 'success' }
    
    // Fechar o modal após atualização bem-sucedida
    setTimeout(() => {
      closeModal()
    }, 2000) // Aumentei o tempo para 2 segundos para dar tempo de ver a mensagem
    
  } catch (error) {
    console.error("Erro ao atualizar destino:", error);
    updateMessage.value = { text: `Erro ao atualizar destino: ${error.message}`, type: 'error' }
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
  listDestines()
  window.addEventListener('keydown', handleKeyDown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeyDown)
})
</script>

<template>
  <div class="container">
    <div class="header">
      <h1 class="heading">Destinos</h1>
      
      <div class="search-container">
        <SearchIcon size="16" class="search-icon" />
        <input 
          type="text" 
          v-model="searchQuery" 
          placeholder="Buscar destinos..." 
          class="search-input"
        />
      </div>
    </div>
    
    <div class="empty-state" v-if="filtereddestines.length === 0">
      <p>Nenhum destino encontrado.</p>
    </div>
    
    <div class="grid" v-else>
      <div class="card" v-for="destine in filtereddestines" :key="destine.id">
        <div class="content">
          <h3 class="name">{{ destine.name }}</h3>
          <p class="id">ID: {{ destine.id }}</p>
          <p class="description">{{ destine.description }}</p>
          <p class="country">País: {{ destine.country }}</p>
        </div>
        
        <div class="actions">
          <button class="action-btn edit" @click="openEditModal(destine)" aria-label="Editar destino">
            <PencilIcon size="18" />
          </button>
          
          <button class="action-btn delete" @click="openDeleteModal(destine)" aria-label="Deletar destino">
            <Trash2Icon size="18" />
          </button>
        </div>
      </div>
    </div>
    
    <!-- Modal de edição -->
    <div class="modal-overlay" v-if="showModal" @click="closeModal"></div>
    
    <div class="modal" v-if="showModal" :class="{ 'modal-active': showModal }">
      <div class="modal-header">
        <h2 class="modal-title">Editar Destino</h2>
        <button class="close-btn" @click="closeModal" aria-label="Fechar modal">
          <XIcon size="20" />
        </button>
      </div>
      
      <div class="modal-body">
        <div class="alert" v-if="updateMessage.text" :class="updateMessage.type">
          {{ updateMessage.text }}
        </div>
        
        <form @submit.prevent="updateDestine">
          <div class="field">
            <label for="edit-name">Nome</label>
            <input
              type="text"
              id="edit-name"
              v-model="editForm.name"
              required
              placeholder="Nome do destino"
            />
          </div>
          
          <div class="field">
            <label for="edit-description">Descrição</label>
            <input
              type="text"
              id="edit-description"
              v-model="editForm.description"
              required
              placeholder="Descrição"
            />
          </div>

          <div class="field">
            <label for="edit-imageUrl">Url Imagem</label>
            <input
              type="text"
              id="edit-imageUrl"
              v-model="editForm.imageUrl"
              required
              placeholder="Url Imagem"
            />
          </div>

          <div class="field">
            <label for="edit-country">País</label>
            <input
              type="text"
              id="edit-country"
              v-model="editForm.country"
              required
              placeholder="País"
            />
          </div>

          <div class="field">
            <label for="edit-state">Estado</label>
            <input
              type="text"
              id="edit-state"
              v-model="editForm.state"
              required
              placeholder="Estado"
            />
          </div>

          <div class="field">
            <label for="edit-city">Cidade</label>
            <input
              type="text"
              id="edit-city"
              v-model="editForm.city"
              required
              placeholder="Cidade"
            />
          </div>

          <div class="field">
            <label for="edit-address">Endereço</label>
            <input
              type="text"
              id="edit-address"
              v-model="editForm.address"
              required
              placeholder="Endereço"
            />
          </div>

          <div class="field">
            <label for="edit-latitude">Latitude</label>
            <input
              type="number"
              step="any"
              id="edit-latitude"
              v-model="editForm.latitude"
              required
              placeholder="Latitude"
            />
          </div>

          <div class="field">
            <label for="edit-longitude">Longitude</label>
            <input
              type="number"
              step="any"
              id="edit-longitude"
              v-model="editForm.longitude"
              required
              placeholder="Longitude"
            />
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
        
        <div class="delete-confirmation" v-if="deletingDestine">
          <p>Tem certeza que deseja excluir o destino <strong>{{ deletingDestine.name }}</strong>?</p>
          <p class="delete-warning">Esta ação não pode ser desfeita.</p>
        </div>
        
        <div class="modal-actions">
          <button type="button" class="cancel-btn" @click="closeDeleteModal" :disabled="isLoading">
            Cancelar
          </button>
          <button type="button" class="delete-btn" @click="deleteDestine" :disabled="isLoading">
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

.description {
  font-size: 0.875rem;
  color: #555;
  margin: 0 0 0.5rem;
}

.country {
  font-size: 0.875rem;
  color: #555;
  margin: 0 0 0.5rem;
}

.id {
  font-size: 0.75rem;
  color: #888;
  margin: 0 0 0.5rem;
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
  max-height: 80vh;
  overflow-y: auto;
  overflow-x: hidden;
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