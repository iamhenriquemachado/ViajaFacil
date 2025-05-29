<script setup>
import { ref, onMounted, onUnmounted, computed } from 'vue'
import { Trash2Icon, PencilIcon, SearchIcon, XIcon, AlertTriangleIcon } from 'lucide-vue-next'

const reservations = ref([])
const searchQuery = ref('')
const showModal = ref(false)
const showDeleteModal = ref(false) 
const editingReservation = ref(null)
const deletingReservation = ref(null)
const isLoading = ref(false)
const updateMessage = ref({ text: '', type: '' })

// Form data para edição
const editForm = ref({
  userId: '',
  destinationId: '',
  reservationDate: '', 
  status: ''
})

// Reservas filtradas com base na pesquisa
const filteredReserves = computed(() => {
  const query = searchQuery.value.toLowerCase().trim()
  if (!query) return reservations.value
  
  return reservations.value.filter(reserve => 
    reserve.userId?.toString().toLowerCase().includes(query) || 
    reserve.destinationId?.toString().toLowerCase().includes(query) ||
    reserve.id?.toString().includes(query) ||
    reserve.reservationDate?.toLowerCase().includes(query)
  )
})

async function listReservations() {
  try {
    const response = await fetch("https://localhost:7073/api/reserves/allreserves", {
      method: "GET",
      credentials: "include"
    });
    if (!response.ok) {
      throw new Error("Erro no servidor: " + response.status);
    }
    const data = await response.json();
    reservations.value = data;
  } catch (error) {
    console.error("Erro de rede ou servidor:", error);
  }
}



// Função para abrir o modal de confirmação de deleção
function openDeleteModal(reserve) {
  deletingReservation.value = reserve
  showDeleteModal.value = true
  updateMessage.value = { text: '', type: '' }
}

// Função para fechar o modal de confirmação de deleção
function closeDeleteModal() {
  showDeleteModal.value = false
  updateMessage.value = { text: '', type: '' }
  setTimeout(() => {
    deletingReservation.value = null
  }, 300)
}

function openEditModal(reserve) {
  editingReservation.value = reserve
  editForm.value.userId = reserve.userId
  editForm.value.destinationId = reserve.destinationId
  editForm.value.reservationDate = reserve.reservationDate
  editForm.value.status = reserve.status.toString()
  showModal.value = true
  updateMessage.value = { text: '', type: '' }
}

function closeModal() {
  showModal.value = false
  updateMessage.value = { text: '', type: '' }
  setTimeout(() => {
    editingReservation.value = null
  }, 300)
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
  listReservations()
  window.addEventListener('keydown', handleKeyDown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeyDown)
})
</script>

<template>
  <div class="container">
    <div class="header">
      <h1 class="heading">Gerenciar Reservas</h1>
      
      <div class="search-container">
        <SearchIcon size="16" class="search-icon" />
        <input 
          type="text" 
          v-model="searchQuery" 
          placeholder="Buscar reservas..." 
          class="search-input"
        />
      </div>
    </div>
    
    <div class="empty-state" v-if="filteredReserves.length === 0">
      <p>Nenhuma reserva encontrada.</p>
    </div>
    
    <div class="grid" v-else>
      <div class="card" v-for="reserve in filteredReserves" :key="reserve.id || reserve.userId">
        <div class="content">
          <h3 class="userId">Usuário: {{ reserve.userId }}</h3>
          <p class="destinationId">Destino: {{ reserve.destinationId }}</p>
          <p class="reservationDate">Data: {{ new Date(reserve.reservationDate).toLocaleDateString('pt-BR') }}</p>
          <p class="status">
            Status: 
            <span class="badge" :class="reserve.status ? 'confirmed' : 'pending'">
              {{ reserve.status ? 'Confirmado' : 'Pendente' }}
            </span>
          </p>
        </div>
        
        <div class="actions">
          <button class="action-btn edit" @click="openEditModal(reserve)" aria-label="Editar reserva">
            <PencilIcon size="18" />
          </button>
          
          <button class="action-btn delete" @click="openDeleteModal(reserve)" aria-label="Deletar reserva">
            <Trash2Icon size="18" />
          </button>
        </div>
      </div>
    </div>
    
    <!-- Modal de edição -->
    <div class="modal-overlay" v-if="showModal" @click="closeModal"></div>
    
    <div class="modal" v-if="showModal" :class="{ 'modal-active': showModal }">
      <div class="modal-header">
        <h2 class="modal-title">Editar Reserva</h2>
        <button class="close-btn" @click="closeModal" aria-label="Fechar modal">
          <XIcon size="20" />
        </button>
      </div>
      
      <div class="modal-body">
        <div class="alert" v-if="updateMessage.text" :class="updateMessage.type">
          {{ updateMessage.text }}
        </div>
        
        <form @submit.prevent="updateReservation">
          <div class="field">
            <label for="edit-user-id">ID Usuário:</label>
            <input
              type="text"
              id="edit-user-id"
              v-model="editForm.userId"
              required
              placeholder="ID do Usuário"
            />
          </div>
          
          <div class="field">
            <label for="edit-destination-id">ID Destino:</label>
            <input
              type="number"
              id="edit-destination-id"
              v-model="editForm.destinationId"
              required
              placeholder="ID do Destino"
            />
          </div>

          <div class="field">
            <label for="edit-date-reserve">Data da Reserva:</label>
            <input
              type="date"
              id="edit-date-reserve"
              v-model="editForm.reservationDate"
              required
              placeholder="Data da Reserva"
            />
          </div>
          
          <div class="field">
            <label for="edit-status">Status:</label>
            <select
              id="edit-status"
              v-model="editForm.status"
              required
            >
              <option value="true">Confirmado</option>
              <option value="false">Pendente</option>
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
        
        <div class="delete-confirmation" v-if="deletingReservation">
          <p>Tem certeza que deseja excluir a reserva do destino <strong>{{ deletingReservation.destinationId }}</strong>?</p>
          <p class="delete-warning">Esta ação não pode ser desfeita.</p>
        </div>
        
        <div class="modal-actions">
          <button type="button" class="cancel-btn" @click="closeDeleteModal" :disabled="isLoading">
            Cancelar
          </button>
          <button type="button" class="delete-btn" @click="deleteReservation" :disabled="isLoading">
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
  pointer-events: none;
}

.search-input {
  width: 100%;
  padding: 0.625rem 0.625rem 0.625rem 2.25rem;
  font-size: 0.875rem;
  border-radius: 6px;
  border: 1px solid #e5e7eb;
  background-color: #fafafa;
  transition: all 0.2s ease;
  box-sizing: border-box;
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
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
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
  border: 1px solid #f0f0f0;
}

.card:hover {
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
}

.content {
  flex: 1;
}

.userId {
  font-size: 1rem;
  font-weight: 500;
  color: #111;
  margin: 0 0 0.5rem;
}

.destinationId, .reservationDate, .status {
  font-size: 0.875rem;
  color: #555;
  margin: 0 0 0.5rem;
}

.badge {
  display: inline-block;
  font-size: 0.75rem;
  font-weight: 500;
  padding: 0.125rem 0.5rem;
  border-radius: 12px;
  margin-left: 0.25rem;
}

.badge.confirmed {
  background-color: rgba(34, 197, 94, 0.1);
  color: #22c55e;
}

.badge.pending {
  background-color: rgba(251, 191, 36, 0.1);
  color: #f59e0b;
}

.actions {
  display: flex;
  gap: 0.5rem;
  margin-left: 1rem;
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

.action-btn.edit:hover {
  color: #3b82f6;
  background: rgba(59, 130, 246, 0.05);
}

.action-btn.delete:hover {
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
  transform: translate(-50%, -48%);
  width: 90%;
  max-width: 480px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  z-index: 100;
  opacity: 0;
  transition: opacity 0.2s ease, transform 0.2s ease;
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

.field:last-of-type {
  margin-bottom: 0;
}

label {
  display: block;
  font-size: 0.875rem;
  font-weight: 500;
  color: #555;
  margin-bottom: 0.375rem;
}

input, select {
  width: 100%;
  padding: 0.625rem;
  font-size: 0.875rem;
  border-radius: 6px;
  border: 1px solid #e5e7eb;
  background-color: #fafafa;
  color: #333;
  transition: all 0.2s ease;
  box-sizing: border-box;
}

input:focus, select:focus {
  outline: none;
  border-color: #aaa;
  background-color: white;
  box-shadow: 0 0 0 2px rgba(0, 0, 0, 0.03);
}

input::placeholder {
  color: #aaa;
}

select {
  appearance: auto;
  cursor: pointer;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
  margin-top: 1.5rem;
}

.cancel-btn, .save-btn, .delete-btn {
  padding: 0.625rem 1rem;
  font-size: 0.875rem;
  font-weight: 500;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.cancel-btn {
  color: #666;
  background-color: #f5f5f5;
}

.cancel-btn:hover {
  background-color: #e5e5e5;
}

.save-btn {
  color: white;
  background-color: #333;
}

.save-btn:hover {
  background-color: #000;
}

.delete-btn {
  color: white;
  background-color: #ef4444;
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
  background-color: rgba(34, 197, 94, 0.1);
  color: #22c55e;
  border: 1px solid rgba(34, 197, 94, 0.2);
}

.alert.error {
  background-color: rgba(239, 68, 68, 0.1);
  color: #ef4444;
  border: 1px solid rgba(239, 68, 68, 0.2);
}
</style>