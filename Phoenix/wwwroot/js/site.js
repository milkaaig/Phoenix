document.addEventListener("DOMContentLoaded", function () {
    const themeToggle = document.getElementById('themeToggle');
    const htmlElement = document.documentElement;

    // Check for saved theme or system preference
    const savedTheme = localStorage.getItem('theme');
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    const initialTheme = savedTheme || (prefersDark ? 'dark' : 'light');

    // Apply the initial theme
    htmlElement.setAttribute('data-theme', initialTheme);
    if (themeToggle) themeToggle.checked = initialTheme === 'dark';

    // Toggle theme on switch change
    if (themeToggle) {
        themeToggle.addEventListener('change', function () {
            const newTheme = this.checked ? 'dark' : 'light';
            htmlElement.setAttribute('data-theme', newTheme);
            localStorage.setItem('theme', newTheme);
        });
    }

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

    
    document.querySelectorAll('.carousel-img').forEach(function(img) {
        const title = img.getAttribute('data-title') || '';
        const category = img.getAttribute('data-category') || '';
        const card = img.closest('.carousel-card');
        let width = 900, height = 337; // 16:6 aspect ratio (900/150 = 6, 900/337 ≈ 2.67)
        if (card) {
            width = Math.max(300, Math.round(card.offsetWidth || 900));
            // Use the same aspect ratio as CSS
            height = Math.round(width * 6 / 16);
        }
        const seed = encodeURIComponent((title + category).replace(/\s+/g, ''));
        const url = `https://picsum.photos/seed/${seed}/${width}/${height}`;
        img.src = url;
    });
});