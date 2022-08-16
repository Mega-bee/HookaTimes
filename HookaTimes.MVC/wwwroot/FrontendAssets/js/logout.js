


document.addEventListener('DOMContentLoaded', (event) => {
    const form = document.querySelector('#logoutForm');
    const btn = document.querySelector('#logoutbtn');
    

    btn.addEventListener('click', (event) => {
        event.preventDefault();
        form.submit();
    });

 
})

