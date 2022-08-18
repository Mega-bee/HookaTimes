export function addToFav(formdata) {
    $.ajax({
        type: 'Post',
        async: true,
        processData: false,
        contentType: false,
        data: formdata,
        url: `/Places/AddToFavorites`,
        success: function (result) {
    
         

        },
        complete: function (xhr, textStatus) {
 
            if (xhr.status == 401) {
              
                window.location.href = '/Account/Login'
            }
        } ,
        fail: function (err) {

            console.log(err)
        }
    })
}

