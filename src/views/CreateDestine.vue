<script setup>
async function handleCreateDestine() {
  const name = document.getElementById("name").value;
  const description = document.getElementById("description").value;
  const imageUrl = document.getElementById("url").value;
  const country = document.getElementById("country").value;
  const state = document.getElementById("state").value;
  const city = document.getElementById("city").value;
  const address = document.getElementById("address").value;
  const latitude = document.getElementById("latitude").value;
  const longitude = document.getElementById("longitude").value;

  try {
    const response = await fetch(
      "https://localhost:7073/api/destines/createdestine",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify({
          name,
          description,
          imageUrl,
          country,
          state,
          city,
          address,
          latitude,
          longitude,
        }),
      }
    );

    if (!response.ok) {
      throw new Error("Erro no servidor: " + response.status);
    }

    alert("Destino criado com sucesso!", "success");

    // Limpa os campos de usuário
    const name = (document.getElementById("name").value = "");
    const description = (document.getElementById("description").value = "");
    const imageUrl = (document.getElementById("url").value = "");
    const country = (document.getElementById("country").value = "");
    const state = (document.getElementById("state").value = "");
    const city = (document.getElementById("city").value = "");
    const address = (document.getElementById("address").value = "");
    const latitude = (document.getElementById("latitude").value = "");
    const longitude = (document.getElementById("longitude").value = "");
  } catch (error) {
    console.error("Erro de rede ou servidor:", error);
    alert("Erro ao criar destino!", "danger", error);
  }
}
</script>

<template>
  <div class="container">
    <div class="form-card">
      <h1 class="heading">Novo destino</h1>

      <form @submit.prevent="handleCreateDestine">
        <div class="field">
          <label for="name">Nome destino</label>
          <input type="text" id="name" required placeholder="Nome do destino" />
        </div>

        <div class="field">
          <label for="email">Descrição</label>
          <input
            type="text"
            id="description"
            required
            placeholder="Descrição do destino"
          />
        </div>

        <div class="field">
          <label for="url">Url Imagem Destino</label>
          <input
            type="url"
            id="url"
            required
            placeholder="Url da imagem do destino"
          />
        </div>

        <div class="field">
          <label for="country">País</label>
          <input type="text" id="country" required placeholder="País" />
        </div>

        <div class="field">
          <label for="state">Estado</label>
          <input type="text" id="state" required placeholder="Estado" />
        </div>

        <div class="field">
          <label for="city">Cidade</label>
          <input type="text" id="city" required placeholder="Cidade" />
        </div>

        <div class="field">
          <label for="address">Endereço</label>
          <input type="text" id="address" required placeholder="Endereço" />
        </div>

        <div class="field">
          <label for="">Latitude</label>
          <input type="number" id="latitude" required placeholder="Latitude" />
        </div>

        <div class="field">
          <label for="">Longitude</label>
          <input
            type="number"
            id="longitude"
            required
            placeholder="Longitude"
          />
        </div>

        <button type="submit" class="submit">Criar destino</button>
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
