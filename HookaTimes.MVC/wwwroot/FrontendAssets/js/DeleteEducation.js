document.addEventListener('DOMContentLoaded', (event) => {

    $(document).on("click", ".deletebtn", function (e) {

        $(this).parent().css("display", "none");
        //alert($(this).parent().attr('value'));
        let educationId = $(this).parent().attr('value');
        let fromdata = new FormData();
        fromdata.append("EducationId", educationId);

        $.ajax({
            type: 'Delete',
            async: true,
            processData: false,
            contentType: false,
            data: fromdata,
            url: `/Account/DeleteEducation`,
            success: function (result) {
               
                if (result.statusCode == 200) {
                    notyf.success({ message: "Your Education has been Deleted" }, 6)
                    //location.reload();
                } else {
                   
                    notyf.error({ message: "Your Education was not Deleted" }, 6)

                }

            },
            fail: function (err) {
                //addToCartBtn.innerHTML = 'Add To Cart'
                //addToCartBtn.disabled = false;
                console.log(err)
              
                notyf.error({ message: "Your Education was not Saved" },6)
            }
        })


    });








    $(document).on("click", ".deleteExpBtn", function (e) {

        $(this).parent().css("display", "none");
        //alert($(this).parent().attr('value'));
        let experienceId = $(this).parent().attr('value');
        let fromdata = new FormData();
        fromdata.append("ExperienceId", experienceId);

        $.ajax({
            type: 'Delete',
            async: true,
            processData: false,
            contentType: false,
            data: fromdata,
            url: `/Account/DeleteExperience`,
            success: function (result) {

                if (result.statusCode == 200) {
                    notyf.success({ message: "Your Experience has been Deleted" }, 6)
                    //location.reload();
                } else {

                    notyf.error({ message: "Your Experience was not Deleted" }, 6)

                }

            },
            fail: function (err) {
                //addToCartBtn.innerHTML = 'Add To Cart'
                //addToCartBtn.disabled = false;
                console.log(err)

                notyf.error({ message: "Your Experience was not Saved" },6)
            }
        })


    });





});