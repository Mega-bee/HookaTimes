import { updateCart } from "../js/update-cart-dropdown.js"

$(document).on("click", ".dropcart__product-remove", function (e) {
 
    let itemId = $(this).attr("data-productid")
    if (itemId) {

            let row = $(this).closest(".dropcart__product")
            if (row) {
                row.remove()
            }
        let formdata = new FormData()
        formdata.append("productId",itemId)
            $.ajax({
                type: 'Delete',
                async: true,
                processData: false,
                contentType: false,
                data: formdata,
                url: `/Cart/RemoveItemFromCart`,
                success: function (result) {
                    console.log(result)
                    if (result.statusCode == 200) {
                        updateCart(false)
                      
                    }

                },
                fail: function (err) {

                    console.log(err)
                }
            })
        }
    
})