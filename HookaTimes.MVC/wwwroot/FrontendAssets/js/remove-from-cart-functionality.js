import { updateCart } from "../js/update-cart-dropdown.js"

export function removeFromCart(data) {
    $.ajax({
        type: 'Delete',
        async: true,
        processData: false,
        contentType: false,
        data: data,
        url: `/Cart/RemoveItemFromCart`,
        success: function (result) {
            if (result.statusCode == 200) {
                updateCart()
                let row = pressedBtn.closest("tr")
                if (row) {
                    row.remove()
                }
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