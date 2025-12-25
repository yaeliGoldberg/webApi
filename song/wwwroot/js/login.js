function login() {
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

// בדיקה אם יש טוקן בכל דף אחר
if(!localStorage.getItem("token")){
    window.location.href = "login.html";
}

