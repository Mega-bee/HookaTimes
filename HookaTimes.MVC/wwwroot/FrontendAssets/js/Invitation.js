
document.addEventListener('DOMContentLoaded', (event) => {
    


    //btn.addEventListener('click', (event) => {
    //    form.submit();
    //})

    //$(document).on("click", ".add-to-favorite-btn", function (e) { }

    $(document).on("submit", "#inviteForm", function (e) { 
        e.preventDefault();

        const form = this;
        const btn = form.querySelector('#submitBtn');
        const closrbtn = document.querySelector('.quickview__close')
        const buddyIdElement = document.querySelector('#buddyId');
        let BuddyId = buddyIdElement.getAttribute('data-buddyid')
        e.preventDefault();

        $('input').removeClass('error');
        $('select').removeClass('error');
        $('textarea').removeClass('error');

        btn.innerHTML = '<i class="fa fa-spinner fa-spin"></i>loading'
        btn.disabled = true;

        var hasError = false;

        var $inputs = $('#inviteForm :input');
        let fromdata = new FormData()
        //let placeId = form.dataset.placeid

        $inputs.each(function (index) {
            var val = $(this).val();
            if ($(this).prop('required')) {
                if (val == "" || val == null) {
                    $(this).addClass("error");
                    hasError = true;
                    return;
                } else {

                    fromdata.append($(this).attr("name"), val);
                }
            }
            //} else {
            //    fromdata.append($(this).attr("name"), val)

            //}
        });
        fromdata.append("ToBuddyId", BuddyId)

        for (var pair of fromdata.entries()) {
            console.log(pair[0] + ', ' + pair[1]);
        }

        
        if (hasError == true) {
            var target = $('.error');
            if (target.length) {
                $('html,body').animate({
                    scrollTop: target.offset().top - 160
                }, 1000);
                return false;
            }
            return;
        }


        $.ajax({
            type: 'Post',
            async: true,
            processData: false,
            contentType: false,
            data: fromdata,
            url: `/Buddy/InviteBuddy`,
            success: function (result) {
                btn.innerHTML = "Submit";
                btn.disabled = false;
                if (result.statusCode == 201) {
                    notyf.success({ message: "Your Invitation has been Sent" }, 6)
                    closrbtn.click();
                } else {
                    btn.innerHTML = "Submit";
                    btn.disabled = false;
                    notyf.error({ message: "Your Invitation not logged in, please login" },6)

                }

            },
            fail: function (err) {
                //addToCartBtn.innerHTML = 'Add To Cart'
                //addToCartBtn.disabled = false;
                console.log(err)
                btn.innerHTML = "Submit";
                btn.disabled = false;
                notyf.error({ message: "Your Invitation not logged in, please login" })
            }
        })
  

   
    })



    $(document).on('click', 'input', function () {
        $(this).removeClass('error');

    });
    $(document).on('click', 'select', function () {
        $(this).removeClass('error');

    });
    $(document).on('click', 'textarea', function () {
        $(this).removeClass('error');

    });

})









