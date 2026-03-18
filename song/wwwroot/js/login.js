function login() {
    const name = document.getElementById("username").value.trim();
    const id = parseInt(document.getElementById("userid").value, 10);

    if (!name || Number.isNaN(id)) {
        alert("Please enter a valid name and numeric ID.");
        return;
    }

    // Use the correct controller endpoint that validates against the stored users list.
    fetch('/Login/Login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Name: name, Id: id })
    })
        .then(response => {
            if (!response.ok) {
                console.error('Login failed status:', response.status, response.statusText);
                return response.text().then(t => {
                    throw new Error(t || "Login failed");
                });
            }
            return response.json();
        })
        .then(data => {
            if (!data?.token) {
                throw new Error("Missing token in response");
            }

            localStorage.setItem("token", data.token);
            alert("Login successful!");
            window.location.href = "index.html"; // הפנייה לדף הראשי
        })
        .catch(err => alert("Login failed: " + err.message));
}



