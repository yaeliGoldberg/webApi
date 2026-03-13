/*function login() {


    const name = document.getElementById("username").value;
    const id = document.getElementById("userid").value;
  

    fetch('http://localhost:5242/user/Login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Name: name, Id: id })
    })
    .then(response => response.json())
    .then(data => {
        localStorage.setItem("token",  data.token);
        alert("Login successful!");
        window.location.href = "index.html"; // הפנייה לדף הראשי
    })
    .catch(err => alert("Login failed: " + err));
}

*/// login.js
/*document.addEventListener("DOMContentLoaded", () => {
    // אם כבר יש token, נשלח מיד לדף הראשי
    const token = localStorage.getItem("token");
    if (token) {
        window.location.href = "index.html";
    }
});*/

function login() {
    console.log("login.js loaded");
    const name = document.getElementById("username").value.trim();
    const id = document.getElementById("userid").value.trim();

    if (!name || !id) {
        alert("אנא הכנס שם ומזהה!");
        return;
    }

    fetch('http://localhost:5242/user/Login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Name: name, Id: id })
    })
        .then(response => {
            if (!response.ok) throw new Error("Login failed: " + response.status);
            return response.json();
        })
        .then(data => {
            localStorage.setItem("token", data.token);
            alert("Login successful!");
            window.location.href = "index.html"; // הפנייה לדף הראשי
        })
        .catch(err => alert(err));
}