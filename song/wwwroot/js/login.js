function login() {
    const name = document.getElementById("username").value;
    const id = document.getElementById("userid").value;


    fetch('/Login/Login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Name: name, Id: id })
    })
        // .then(response => response.json())
        .then(response => response.text()) // 👈 במקום json
        .then(data => {
            console.log("SERVER RESPONSE:", data);
        })
        .then(data => {
            console.log("RESPONSE:", data);
            localStorage.setItem("token", data.token);
            alert("Login successful!");
            window.location.href = "index.html"; // הפנייה לדף הראשי
        })
        .catch(err => alert("Login failed: " + err));
}


