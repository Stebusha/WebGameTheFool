window.moveCardToCenter = function () {
    const card = document.getElementById('card');
    if (card) {
        const windowWidth = window.innerWidth;
        const windiwHeight = window.innerHeight;

        const centerX = (windowWidth - card.offsetWidth) / 2;
        const centerY = (windowHeight - card.offsetHeight) / 2;

        card.style.left = centerX + 'px';
        card.style.top = centerY + 'px';
    }
}