
document.addEventListener('DOMContentLoaded', (event) => {
    //$(document).on("click", "#logoutbtn", function (e) {
    //    e.preventDefault()
    //    const form = document.querySelector('#logoutForm');
    //    /*     const btn = document.querySelector('#logoutbtn');*/
    //    //console.log("button", btn)
    //    console.log(form)
    //    if (form) {

    //        form.submit();

    //    }


    //})

    //    $('#customSwitch1').click(function(e){
    //        alert((this).attr('checked'))
    //    });
    //})


    $('input#customSwitch1').change(function () {


        $.ajax({
            type: 'Put',
            async: true,
            processData: false,
            contentType: false,
            url: `/Account/IsAvailableToggle`,
        })



    })
});

