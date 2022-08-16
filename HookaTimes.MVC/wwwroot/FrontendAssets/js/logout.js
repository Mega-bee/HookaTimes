


document.addEventListener('DOMContentLoaded', (event) => {
    const form = document.querySelector('#logoutForm');
    const btn = document.querySelector('#logoutbtn');
    //const btnLogin = document.querySelector('#btnLogin');
    //const inpEmail = document.querySelector('#header-signin-email');
    //const inpPass = document.querySelector('#header-signin-password');

    //let inpEmailValue = inpEmail.value;
    //let inpPassValue = inpPass.value;

    btn.addEventListener('click', (event) => {
        event.preventDefault();
        form.submit();
    });

    //btnLogin.addEventListener('click', handleLogin);


    //function handleLogin() {

    //    let data = new FormData()
    //    data.append("Email", inpEmailValue);
    //    data.append("Password", inpPassValue);

    
    }
})

