document.addEventListener('DOMContentLoaded', (e) => {

    $(document).on('click', '.account-menu__form-forgot-link', function () {
        e.preventDefault();
        $(this).attr("disabled", true)
        let btn = $('.account-menu__form-forgot-link');
        if ($('#header-signin-email').val()) {

            

            let email = $('#header-signin-email').val();
           
            let fromdata = new FormData()
            fromdata.append("identifier", email)
            alert(email)
            $.ajax({
                type: 'Post',
                async: true,
                processData: false,
                contentType: false,
                data: fromdata,
                url: `/Account/ForgetPassword`,
                success: function (result) {
                   
                    btn.attr("disabled", false);
                    if (result.statusCode == 200) {
                        notyf.success({ message: result.data.message }, 6)
                        closrbtn.click();
                    } else {
                        btn.attr("disabled", false);

                        notyf.error({ message: result.errorMessage }, 6)

                    }

                },
                fail: function (err) {
                    //addToCartBtn.innerHTML = 'Add To Cart'
                    //addToCartBtn.disabled = false;
                    console.log(err)
                    btn.innerHTML = "Submit";
                    btn.disabled = false;
                    notyf.error({ message: "Something Went Wrong" })
                }
            })
        }
        else {
            btn.attr("disabled", false);
            //notyf.open({ type: 'warning', message: "Please fill the email filed", duration: 6 });
            //notyf.custom({ message: "Please fill the email filed", duration: 6, className:"whitesmoke", "fa fa-gear"});
            //toastNotifyCustom("Please fill the email filed", 6, "whitesmoke", "fa fa-gear")

            //notyf.error({ message: "FUck you bitch ntek 7oto la hal email l zaber" },6)

            notyf.open({
                type: 'warning', message: "Please insert your email", duration: 4000,
            });

        }
        
       
    });


});