/*console.log("site.js loaded!");
const uri = '/Sing';
let music = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const nameInput = document.getElementById('add-name');
    const singerInput = document.getElementById('add-singer');

    const item = {
        name: nameInput.value.trim(),
        singer: singerInput.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
    .then(response => response.json())
    .then(() => {
        getItems();
        nameInput.value = '';
        singerInput.value = '';
    })
    .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, { method: 'DELETE' })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = music.find(x => x.id === id);
    if (!item) return;

    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-singer').value = item.singer;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = parseInt(document.getElementById('edit-id').value, 10);
    const item = {
        id: itemId,
        name: document.getElementById('edit-name').value.trim(),
        singer: document.getElementById('edit-singer').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
    .then(() => getItems())
    .catch(error => console.error('Unable to update item.', error));

    closeInput();
    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(count) {
    const name = (count === 1) ? 'song' : 'songs';
    document.getElementById('counter').innerText = `${count} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('musics');
    tBody.innerHTML = '';
    _displayCount(data.length);

    data.forEach(item => {
        const tr = tBody.insertRow();

        const td1 = tr.insertCell(0);
        td1.textContent = item.name;

        const td2 = tr.insertCell(1);
        td2.textContent = item.singer;

        const td3 = tr.insertCell(2);
        const editBtn = document.createElement('button');
        editBtn.textContent = 'Edit';
        editBtn.addEventListener('click', () => displayEditForm(item.id));
        td3.appendChild(editBtn);

        const td4 = tr.insertCell(3);
        const delBtn = document.createElement('button');
        delBtn.textContent = 'Delete';
        delBtn.addEventListener('click', () => deleteItem(item.id));
        td4.appendChild(delBtn);
    });

    music = data;
}

// Load items initially
getItems();
*/


console.log("site.js loaded!");

const uri = 'https://localhost:7154/Sing';
let music = [];

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

function getItems() {
    fetch(uri, {
        headers: {
            'Authorization': 'Bearer ' + getToken()
        }
    })
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const nameInput = document.getElementById('add-name');
    const singerInput = document.getElementById('add-singer');

    const item = {
        name: nameInput.value.trim(),
        singer: singerInput.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            nameInput.value = '';
            singerInput.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': 'Bearer ' + getToken()
        }
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = music.find(x => x.id === id);
    if (!item) return;

    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-singer').value = item.singer;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = parseInt(document.getElementById('edit-id').value, 10);

    const item = {
        id: itemId,
        name: document.getElementById('edit-name').value.trim(),
        singer: document.getElementById('edit-singer').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: getAuthHeaders(),
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();
    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(count) {
    const name = (count === 1) ? 'song' : 'songs';
    document.getElementById('counter').innerText = `${count} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('musics');
    tBody.innerHTML = '';
    _displayCount(data.length);

    data.forEach(item => {
        const tr = tBody.insertRow();

        const td1 = tr.insertCell(0);
        td1.textContent = item.name;

        const td2 = tr.insertCell(1);
        td2.textContent = item.singer;

        const td3 = tr.insertCell(2);
        const editBtn = document.createElement('button');
        editBtn.textContent = 'Edit';
        editBtn.addEventListener('click', () => displayEditForm(item.id));
        td3.appendChild(editBtn);

        const td4 = tr.insertCell(3);
        const delBtn = document.createElement('button');
        delBtn.textContent = 'Delete';
        delBtn.addEventListener('click', () => deleteItem(item.id));
        td4.appendChild(delBtn);
    });

    music = data;
}

//getItems();

