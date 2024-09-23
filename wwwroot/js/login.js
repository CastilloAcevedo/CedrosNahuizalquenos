// wwwroot/js/login.js

const togglePassword = document.querySelector('#togglePassword');
const password = document.querySelector('#password');
const toggleIcon = document.querySelector('#toggleIcon');
const loginForm = document.querySelector('#loginForm');
const emailError = document.querySelector('#emailError');
const passwordError = document.querySelector('#passwordError');

togglePassword.addEventListener('click', function () {
    const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
    password.setAttribute('type', type);
    const imgSrc = type === 'password' ? '/img/ojo-cerrado.png' : '/img/ojo.png';
    toggleIcon.setAttribute('src', imgSrc);
});

loginForm.addEventListener('submit', function (e) {
    let valid = true;

    // Resetear los mensajes de error
    emailError.style.display = 'none';
    passwordError.style.display = 'none';

    // Validar email
    if (!email.value) {
        emailError.style.display = 'block';
        valid = false;
    }

    // Validar contraseña
    if (!password.value) {
        passwordError.style.display = 'block';
        valid = false;
    }

    // Si no es válido, evitar el envío del formulario
    if (!valid) {
        e.preventDefault();
    }
});
