// cart.js

function updateCartCount(cartCount) {
    const cartItemCount = document.getElementById('cartItemCount');

    if (cartCount > 0) {
        cartItemCount.textContent = cartCount;
        cartItemCount.style.display = 'inline-block'; // Mostrar el círculo rojo
    } else {
        cartItemCount.style.display = 'none'; // Ocultar si no hay productos
    }
}

// Simulación de la cantidad de productos en el carrito
let cartCount = 3; // Aquí puedes obtener la cantidad desde tu servidor o localStorage
updateCartCount(cartCount);
