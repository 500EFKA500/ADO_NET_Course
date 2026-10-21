async function request(url, options) {
    const res = await fetch(url, options);
    if (!res.ok) {
        let message = 'Ошибка запроса: ' + res.status;
        try { const body = await res.json(); message = body.error || message; } catch {}
        throw new Error(message);
    }
    return res.json();
}
function showError(error) { document.getElementById('gr').textContent = error.message; }
function renderRows(id, rows, fields) {
    const tbody = document.getElementById(id);
    tbody.replaceChildren();
    for (const row of rows) {
        const tr = document.createElement('tr');
        for (const field of fields) {
            const td = document.createElement('td');
            td.textContent = row[field];
            tr.appendChild(td);
        }
        tbody.appendChild(tr);
    }
}
async function loadStudents() {
    try {
        const students = await request('/students/all');
        ClearPage();
        document.getElementById('studentsTable').style.display = 'table';
        renderRows('studentBody', students, ['studentId', 'firstName', 'lastName', 'age']);
    } catch (error) { showError(error); }
}
async function loadGroups() {
    try {
        const groups = await request('/groups/all');
        ClearPage();
        document.getElementById('groupsTable').style.display = 'table';
        renderRows('groupBody', groups, ['groupId', 'groupName']);
    } catch (error) { showError(error); }
}
async function addStudent(event) {
    event.preventDefault();
    const button = document.querySelector('button[type="submit"]');
    button.disabled = true;
    try {
        const student = {
            firstName: document.getElementById('firstNameStudent').value.trim(),
            lastName: document.getElementById('lastNameStudent').value.trim(),
            age: Number(document.getElementById('ageStudent').value),
            groupId: Number(document.getElementById('groupId').value)
        };
        const saved = await request('/students/add', {
            method: 'POST', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(student)
        });
        document.getElementById('studentForm').reset();
        await loadStudents();
        document.getElementById('gr').textContent = 'Студент добавлен. ID: ' + saved.studentId;
    } catch (error) { showError(error); }
    finally { button.disabled = false; }
}
function ClearPage() {
    document.getElementById('studentsTable').style.display = 'none';
    document.getElementById('groupsTable').style.display = 'none';
    document.getElementById('gr').textContent = '';
}
async function initialize() {
    try {
        const groups = await request('/groups/all');
        const select = document.getElementById('groupId');
        for (const group of groups) {
            const option = document.createElement('option');
            option.value = group.groupId;
            option.textContent = group.groupName;
            select.appendChild(option);
        }
        await loadStudents();
    } catch (error) { showError(error); }
}
document.getElementById('studentForm').addEventListener('submit', addStudent);
initialize();
