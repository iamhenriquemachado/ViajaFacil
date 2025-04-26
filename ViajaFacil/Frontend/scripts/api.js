document.getElementById("loginForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const email = document.getElementById("email").value.trim();
    const password = document.getElementById("password").value.trim();

    if (!email.includes('@') || password.length < 8) {
        alert('Please enter a valid email and password.');
        return;
    }

    try {
        const response = await fetch('https://localhost:7073/api/users/authentication', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include', 
            body: JSON.stringify({ email, password })
        });

        if (response.ok) {
            const data = await response.json();
            if (data.isAdmin === true) {
                window.location.href = '../pages/admin-dashboard.html';
            } else {
                window.location.href = '../pages/user-dashboard.html';
            }
        } else if (response.status === 401) {
            alert('Invalid email or password.');
        } else {
            alert('Login failed. Please try again.');
        }

    } catch (error) {
        console.error('Network or server error:', error);
        alert('Unable to connect to the server. Please try again later.');
    }
});
