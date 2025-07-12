document.addEventListener("DOMContentLoaded", function () {
    // Randomize border color for post cards
    const cards = document.querySelectorAll('.card.h-100');
    cards.forEach(card => {
        const color = `hsl(${Math.floor(Math.random() * 360)}, 70%, 60%)`;
        card.style.border = `1px solid ${color}`;
    });

    // Randomize border color for carousel cards
    const carouselCards = document.querySelectorAll('.carousel-card');
    carouselCards.forEach(card => {
        const color = `hsl(${Math.floor(Math.random() * 360)}, 70%, 60%)`;
        card.style.border = `1px solid ${color}`;
    });
});
