
document.addEventListener('DOMContentLoaded', (event) => {
    const form = document.querySelector('#inviteForm');
    const btn = document.querySelector('#submitBtn');
    const buddyIdElement = document.querySelector('#buddyId');
    let BuddyId = buddyIdElement.getAttribute('data-buddyid')


    //btn.addEventListener('click', (event) => {
    //})


    form.addEventListener('submit', (event) => {
        event.preventDefault();

        $('input').removeClass('error');
        $('select').removeClass('error');
        $('textarea').removeClass('error');

        btn.innerHTML = '<i class="fa fa-spinner fa-spin"></i>loading'
        btn.disabled = true;

        var hasError = false;

        var $inputs = $('#exampleValidation :input');
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

                    fromdata.append($(this).attr("name"), val)
                }
            } else {
                fromdata.append($(this).attr("name"), val)
            }
        });
        fromdata.append("ToBuddyId", BuddyId)

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

                } else {
                    notyf.error({ message: "Your Invitation not logged in, please login" })

                }

            },
            fail: function (err) {
                //addToCartBtn.innerHTML = 'Add To Cart'
                //addToCartBtn.disabled = false;
                //console.log(err)
                btn.innerHTML = "Submit";
                btn.disabled = false;
                notyf.error({ message: "Your Invitation not logged in, please login" })
            }
        })
    });

   




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









