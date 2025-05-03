// A função showAlert está agora fora do bloco try-catch
function showAlert(message, type = "success") {
    const alertBox = document.getElementById("alertBox");
    alertBox.className = "alert alert-" + type;
    alertBox.innerText = message;
    alertBox.classList.remove("d-none");

    setTimeout(() => {
        alertBox.classList.add("d-none");
    }, 4000);
}

document.getElementById("createUserForm").addEventListener("submit", async function(e) {
    e.preventDefault();

    const name = document.getElementById("name").value.trim();
    const email = document.getElementById("email").value.trim();
    const password = document.getElementById("password").value;
    const isAdminValue = document.getElementById("isAdmin").value;

    if (!email.includes('@') || password.length < 8) {
        showAlert('Por favor, insira um e-mail válido e uma senha com pelo menos 8 caracteres.', "danger");
        return;
    }

    // Verifica se o valor de isAdmin é válido
    if (isAdminValue !== "true" && isAdminValue !== "false") {
        showAlert('Por favor, selecione se o usuário será um administrador.', "danger");
        return;
    }

    // Converte a string para booleano
    const isAdmin = isAdminValue === "true";

    try {
        const response = await fetch('https://localhost:7073/api/users/createuser', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ name, email, password, isAdmin })
        });

        if (!response.ok) {
            throw new Error('Erro no servidor: ' + response.status);
        }

        const data = await response.json();
        showAlert("Usuário criado com sucesso!", "success");
        console.log(data);

        // Limpar os campos do formulário
        document.getElementById("name").value = "";
        document.getElementById("email").value = "";
        document.getElementById("password").value = "";
        document.getElementById("isAdmin").value = "";

    } catch (error) {
        console.error('Erro de rede ou servidor:', error);
        showAlert("Erro ao criar usuário!", "danger");
    }
});
