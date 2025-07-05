document.addEventListener('DOMContentLoaded', function () {
    var searchBox = document.getElementById('searchBox');
    if (!searchBox) return;

    searchBox.addEventListener('input', function () {
        var query = this.value.toLowerCase();

        // Filter carousel items
        var carouselItems = document.querySelectorAll('.post-carousel-item');
        var firstVisible = null;
        carouselItems.forEach(function (item, idx) {
            var text = (item.dataset.title + ' ' + item.dataset.category + ' ' + item.dataset.author).toLowerCase();
            if (text.includes(query)) {
                item.style.display = '';
                if (firstVisible === null) firstVisible = idx;
            } else {
                item.style.display = 'none';
            }
            item.classList.remove('active');
        });
        // Set first visible as active
        if (firstVisible !== null && carouselItems[firstVisible]) {
            carouselItems[firstVisible].classList.add('active');
        }

        // Filter cards
        var cards = document.querySelectorAll('.post-card');
        cards.forEach(function (card) {
            var text = (card.dataset.title + ' ' + card.dataset.category + ' ' + card.dataset.author).toLowerCase();
            card.style.display = text.includes(query) ? '' : 'none';
        });
    });
});