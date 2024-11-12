function cardSelected() {
    const card = document.getElementsById('card');
    console.log("try select card");

    card.style.transform = 'translateY(-10px)';
    card.style.position = 'fixed';

}