export function addToFav(formdata) {
    $.ajax({
        type: 'Post',
        async: true,
        processData: false,
        contentType: false,
        data: formdata,
        url: `/Places/AddToFavorites`,
        success: function (result) {
            console.log(result)
            if (result.statusCode == 200 || result.statusCode == 201) {
                notyf.success({ message: result.data.message })
            } else {
                notyf.error({ message: result.errorMessage })

            }
          

        },
        complete: function (xhr, textStatus) {
 
            if (xhr.status == 401) {
              
                notyf.error({ message: "Your are not logged in, please login" })
            }
        } ,
        fail: function (err) {
            notyf.error({ message: "Failed to add place to favorites" })

            console.log(err)
        }
    })
}

