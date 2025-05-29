<script setup>
async function handleCreateUser() {
  const name = document.getElementById("name").value.trim();
  const email = document.getElementById("email").value.trim();
  const password = document.getElementById("password").value;
  const admin = document.getElementById("admin").value;
  
  if (!email.includes("@") || password.length < 8) {
    alert(
      "Por favor, insira um e-mail válido e uma senha com pelo menos 8 caracteres.",
      "danger"
    );
    return;
  }
  
  if (admin !== "true" && admin !== "false") {
    alert("Por favor, selecione se o usuário será um administrador.", "danger");
    return;
  }
  
  const isAdmin = admin === "true";
  
  try {
    const response = await fetch(
      "https://localhost:7073/api/users/createuser",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ name, email, password, isAdmin }),
      }
    );
    
    if (!response.ok) {
      throw new Error("Erro no servidor: " + response.status);
    }
    
    const data = await response.json();
    alert("Usuário criado com sucesso!", "success");
    
    // Limpar os campos do formulário
    document.getElementById("name").value = "";
    document.getElementById("email").value = "";
    document.getElementById("password").value = "";
    document.getElementById("admin").value = "";
  } catch (error) {
    console.error("Erro de rede ou servidor:", error);
    alert("Erro ao criar usuário!", "danger");
  }
}
</script>

<template>
  <div class="container">
    <div class="form-card">
      <h1 class="heading">Novo usuário</h1>
      
      <form @submit.prevent="handleCreateUser">
        <div class="field">
          <label for="name">Nome</label>
          <input type="text" id="name" required placeholder="Digite o nome completo" />
        </div>
        
        <div class="field">
          <label for="email">E-mail</label>
          <input type="email" id="email" required placeholder="Digite o e-mail" />
        </div>
        
        <div class="field">
          <label for="password">Senha</label>
          <input type="password" id="password" required placeholder="Mínimo de 8 caracteres" />
        </div>
        
        <div class="field">
          <label for="admin">Administrador</label>
          <select id="admin" required>
            <option disabled selected value="">Selecione uma opção</option>
            <option value="true">Sim</option>
            <option value="false">Não</option>
          </select>
        </div>
        
        <button type="submit" class="submit">Criar usuário</button>
      </form>
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

.form-card {
  background: white;
  border-radius: 8px;
  padding: 2rem;
  width: 100%;
  max-width: 480px;
  margin: 0 auto;
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
}

.heading {
  font-size: 1.5rem;
  font-weight: 500;
  color: #333;
  margin-bottom: 1.5rem;
  letter-spacing: -0.01em;
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

input,
select {
  width: 100%;
  padding: 0.625rem;
  font-size: 0.875rem;
  border-radius: 6px;
  border: 1px solid #e5e7eb;
  background-color: #fafafa;
  color: #333;
  transition: all 0.2s ease;
}

input:focus,
select:focus {
  outline: none;
  border-color: #aaa;
  background-color: white;
  box-shadow: 0 0 0 2px rgba(0, 0, 0, 0.03);
}

input::placeholder {
  color: #aaa;
}

.submit {
  width: 100%;
  padding: 0.625rem 1.25rem;
  background-color: #333;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 0.875rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  margin-top: 0.5rem;
}

.submit:hover {
  background-color: #000;
}

.submit:active {
  transform: translateY(1px);
}
</style>