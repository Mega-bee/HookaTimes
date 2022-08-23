const form = document.querySelector("#contact-form")
const submitBtn = document.querySelector("#submit-btn")


function resetBtn() {
    submitBtn.innerHTML = "Send Message";
    submitBtn.disabled = false;
}

function sendMessage(data,e) {
    $.ajax({
        type: 'Post',
        async: true,
        processData: false,
        contentType: false,
        data: data,
        url: `/ContactUs/SendContactUsMessage`,
        success: function (result) {
            e.target.reset()
          resetBtn()
            if (result.statusCode == 200) {
                notyf.success({ message: result.data.message }, 6)
            } else {
              
                notyf.error({ message: "Failed to send message. Please try again" }, 6)

            }

        },
        fail: function (err) {
            //addToCartBtn.innerHTML = 'Add To Cart'
            //addToCartBtn.disabled = false;
            console.log(err)
            resetBtn()
            notyf.error({ message: "Failed to send message. Please try again" })
        }
    })
}

function submitForm(e) {
    e.preventDefault()
    var $inputs = $('#contact-form :input');
    let formdata = new FormData()
    //let placeId = form.dataset.placeid
    var hasError = false;
    submitBtn.innerHTML = '<i class="fa fa-spinner fa-spin"></i>Send Message'
    submitBtn.disabled = true;
    $inputs.each(function (index) {
        var val = $(this).val();
        //if ($(this).prop('required')) {
        //    if (val == "" || val == null) {
        //        $(this).addClass("error");
        //        hasError = true;
                
        //        return;
        //    } else {

                formdata.append($(this).attr("name"), val);
        //    }
        //}
  
    });

    if (hasError == true) {
        var target = $('.error');
        if (target.length) {
            $('html,body').animate({
                scrollTop: target.offset().top - 160
            }, 1000);
            resetBtn()
            return false;
        }
        return;
    }

    sendMessage(formdata,e)
}

form.addEventListener("submit",submitForm)

