console.log("user.js loaded!");
const uri = '/User';
let users = [];

function getToken() {
    return localStorage.getItem("token");
}

function getAuthHeaders() {
    return {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + getToken()
    };
}

function getUserRole() {
    // Decode JWT token to get role
    const token = getToken();
    if (!token) return null;
    
    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        return payload.role || null;
    } catch (e) {
        return null;
    }
}

function getItems() {
    fetch(uri, {
        headers: getAuthHeaders()
    })
        .then(response => {
            if (!response.ok) {
                if (response.status === 401) {
                    alert("הפעה פגת - אנא התחבר מחדש");
                    window.location.href = "login.html";
                    return Promise.reject("Unauthorized");
                }
                if (response.status === 403) {
                    alert("אין לך הרשאה לצפות ברשימת משתמשים");
                    return Promise.reject("Forbidden");
                }
                throw new Error(`HTTP Error: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            if (data && Array.isArray(data)) {
                _displayItems(data);
            } else {
                console.warn("לא קיבלנו מערך משתמשים");
                _displayItems([]);
            }
        })
        .catch(error => {
            console.error('Unable to get items.', error);
            alert("שגיאה בטעינת המשתמשים: " + error.message);
        });
}

function addItem() {
    const userRole = getUserRole();
    if (userRole !== 'admin') {
        alert("אין לך הרשאה להוסיף משתמשים חדשים");
        return;
    }

    const nameInput = document.getElementById('add-name');
    const ageInput = document.getElementById('add-age');
    const roleSelect = document.getElementById('add-role');

    if (!nameInput.value.trim() || !ageInput.value.trim()) {
        alert("אנא מלא את כל השדות");
        return;
    }

    const item = {
        name: nameInput.value.trim(),
        age: parseInt(ageInput.value.trim(), 10),
        role: roleSelect ? roleSelect.value : 'user'
    };

    fetch(uri, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(item)
        })
        .then(response => {
            if (!response.ok) {
                if (response.status === 401) {
                    alert("הפעה פגת - אנא התחבר מחדש");
                    window.location.href = "login.html";
                    return Promise.reject("Unauthorized");
                }
                if (response.status === 403) {
                    alert("אין לך הרשאה להוסיף משתמשים");
                    return Promise.reject("Forbidden");
                }
                throw new Error(`HTTP Error: ${response.status}`);
            }
            return response.json();
        })
        .then(() => {
            alert("המשתמש נוסף בהצלחה!");
            getItems();
            nameInput.value = '';
            ageInput.value = '';
            if (roleSelect) roleSelect.value = 'user';
        })
        .catch(error => {
            console.error('Unable to add item.', error);
            alert("שגיאה בהוספת המשתמש: " + error.message);
        });
}

function deleteItem(id) {
    if (!confirm("האם אתה בטוח שברצונך למחוק משתמש זה?")) {
        return;
    }

    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: getAuthHeaders()
    })
        .then(response => {
            if (!response.ok) {
                if (response.status === 401) {
                    alert("הפעה פגת - אנא התחבר מחדש");
                    window.location.href = "login.html";
                    return Promise.reject("Unauthorized");
                }
                if (response.status === 403) {
                    alert("אין לך הרשאה למחוק משתמש זה");
                    return Promise.reject("Forbidden");
                }
                throw new Error(`HTTP Error: ${response.status}`);
            }
            return response.text();
        })
        .then(() => {
            alert("המשתמש נמחק בהצלחה!");
            getItems();
        })
        .catch(error => {
            console.error('Unable to delete item.', error);
            alert("שגיאה במחיקת המשתמש: " + error.message);
        });
}

function displayEditForm(id) {
    const item = users.find(x => x.id === id);
    if (!item) return;

    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-age').value = item.age;
    if (document.getElementById('edit-role')) {
        document.getElementById('edit-role').value = item.role || 'user';
    }
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = parseInt(document.getElementById('edit-id').value, 10);
    
    if (!document.getElementById('edit-name').value.trim() || !document.getElementById('edit-age').value.trim()) {
        alert("אנא מלא את כל השדות");
        return false;
    }

    const item = {
        id: itemId,
        name: document.getElementById('edit-name').value.trim(),
        age: parseInt(document.getElementById('edit-age').value.trim(), 10),
        role: document.getElementById('edit-role') ? document.getElementById('edit-role').value : 'user'
    };

    fetch(`${uri}/${itemId}`, {
            method: 'PUT',
            headers: getAuthHeaders(),
            body: JSON.stringify(item)
        })
        .then(response => {
            if (!response.ok) {
                if (response.status === 401) {
                    alert("הפעה פגת - אנא התחבר מחדש");
                    window.location.href = "login.html";
                    return Promise.reject("Unauthorized");
                }
                if (response.status === 403) {
                    alert("אין לך הרשאה לערוך משתמש זה");
                    return Promise.reject("Forbidden");
                }
                throw new Error(`HTTP Error: ${response.status}`);
            }
            return response.text();
        })
        .then(() => {
            alert("המשתמש עודכן בהצלחה!");
            getItems();
            closeInput();
        })
        .catch(error => {
            console.error('Unable to update item.', error);
            alert("שגיאה בעדכון המשתמש: " + error.message);
        });

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(count) {
    const name = (count === 1) ? 'user' : 'users';
    document.getElementById('counter').innerText = `${count} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('user');
    tBody.innerHTML = '';
    _displayCount(data.length);

    const userRole = getUserRole();
    const isAdmin = userRole === 'admin';

    data.forEach(item => {
        const tr = tBody.insertRow();

        const td1 = tr.insertCell(0);
        td1.textContent = item.name;

        const td2 = tr.insertCell(1);
        td2.textContent = item.age;

        const td2b = tr.insertCell(2);
        td2b.textContent = item.role || 'user';

        const td3 = tr.insertCell(3);
        if (isAdmin) {
            const editBtn = document.createElement('button');
            editBtn.textContent = 'עריכה';
            editBtn.addEventListener('click', () => displayEditForm(item.id));
            td3.appendChild(editBtn);
        }

        const td4 = tr.insertCell(4);
        if (isAdmin) {
            const delBtn = document.createElement('button');
            delBtn.textContent = 'מחיקה';
            delBtn.addEventListener('click', () => deleteItem(item.id));
            td4.appendChild(delBtn);
        }
    });

    users = data;
}

// Load items initially when page loads
getItems();