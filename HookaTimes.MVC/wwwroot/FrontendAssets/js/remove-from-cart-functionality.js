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
            console.log(result)
            if (result.statusCode == 200) {
                updateCart()
                let row = pressedBtn.closest("tr")
                if (row) {
                    row.remove()
                }
            } else {
              
            }

        },
        fail: function (err) {

            console.log(err)
        }
    })
}