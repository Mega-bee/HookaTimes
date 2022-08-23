
document.addEventListener('DOMContentLoaded', (event) => {
    const form = document.querySelector('#experienceForm');
    const btn = document.querySelector('#submit-Education');
  


    //btn.addEventListener('click', (e) => {
    //    console.log("submitting form")
    //    e.preventDefault();
    //    form.submit();
    //})

    //$(document).on("click", ".add-to-favorite-btn", function (e) { }

    $(document).on("submit", "#experienceForm", function (e) { 
       
        e.preventDefault();
       

        const form = document.querySelector('#educationForm');
        const btn = document.querySelector('#submit-Education');

        $('input').removeClass('error');
        $('select').removeClass('error');
        $('textarea').removeClass('error');

        btn.innerHTML = '<i class="fa fa-spinner fa-spin"></i>loading'
        btn.disabled = true;

        var hasError = false;

        var $inputs = $('#experienceForm :input');
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
            url: `/Account/AddExperience`,
            success: function (result) {
                btn.innerHTML = "Save";
                btn.disabled = false;
                if (result.statusCode == 201) {
                    notyf.success({ message: "Your Experience has been Saved" }, 6)
                    location.reload();
                } else {
                    btn.innerHTML = "Save";
                    btn.disabled = false;
                    notyf.error({ message: "Your Experience was not Saved" },6)

                }

            },
            fail: function (err) {
                //addToCartBtn.innerHTML = 'Add To Cart'
                //addToCartBtn.disabled = false;
                console.log(err)
                btn.innerHTML = "Save";
                btn.disabled = false;
                notyf.error({ message: "Your Experience was not Saved" })
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









