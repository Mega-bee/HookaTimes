import { updateCart } from "../js/update-cart-dropdown.js"

export function addToCart(data) {
    $.ajax({
        type: 'Post',
        async: true,
        processData: false,
        contentType: false,
        data: data,
        url: `/Cart/AddToCart`,
        success: function (result) {
            if (result.statusCode == 201) {


                updateCart()
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: result.data.message,
                })
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Fail',
                    text: result.errorMessage
                })
            }

        },
        fail: function (err) {

            console.log(err)
        }
    })
}