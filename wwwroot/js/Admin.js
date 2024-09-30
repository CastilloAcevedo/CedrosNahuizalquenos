// cart.js

function updateCartCount(cartCount) {
    const cartItemCount = document.getElementById('cartItemCount');
    cartCount++;
    if (cartCount > 0) {
        cartItemCount.textContent = cartCount;
        cartItemCount.style.display = 'inline-block'; // Mostrar el círculo rojo
    } else {
        cartItemCount.style.display = 'none'; // Ocultar si no hay productos
    }
}

