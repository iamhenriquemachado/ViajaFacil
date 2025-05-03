async function getUsers() {
    try {
        const response = await fetch("https://localhost:7073/api/users/listusers", {
            method: "GET",
            credentials: "include" // envia o cookie JWT
        });

        if (!response.ok) {
            throw new Error("Erro no servidor: " + response.status);
        }

        const data = await response.json();
        console.log(data);
        return data;

    } catch (error) {
        console.error("Erro de rede ou servidor:", error);
    }
}
