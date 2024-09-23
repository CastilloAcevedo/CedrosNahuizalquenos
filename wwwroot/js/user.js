function toggleSecurityCodeField() {
    var role = document.getElementById('Rol').value;
    var securityCodeDiv = document.getElementById('securityCodeDiv');
    securityCodeDiv.style.display = (role === 'Administrador') ? 'block' : 'none';
}

function togglePasswordVisibility() {
    var passwordInput = document.getElementById('passwordInput');
    var toggleIcon = document.getElementById('togglePasswordIcon');
    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        toggleIcon.src = '/img/ojo.png'; // Cambia a la imagen de contraseña visible
    } else {
        passwordInput.type = 'password';
        toggleIcon.src = '/img/ojo-cerrado.png'; // Cambia a la imagen de contraseña oculta
    }
}