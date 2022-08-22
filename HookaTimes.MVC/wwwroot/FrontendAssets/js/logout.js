


document.addEventListener('DOMContentLoaded', (event) => {
    $(document).on("click", "#logoutbtn", function (e) {
        e.preventDefault()
        const form = document.querySelector('#logoutForm');
   /*     const btn = document.querySelector('#logoutbtn');*/
        //console.log("button", btn)
        console.log(form)
        if (form) {

                form.submit();

        }


    })
})

