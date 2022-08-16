import { updateCart } from "../js/update-cart-dropdown.js"

document.addEventListener("click", e => {
   
    let btn = e.target
    if (btn.classList.contains("dropcart__product-remove")) {
        let itemId = btn.dataset.productid
        console.log("itemIddddddddd",itemId)
        if (itemId) {
            let row = btn.closest(".dropcart__product")
            if (row) {
                row.remove()
            }
            let formdata = new FormData()
            formdata.append("productId", itemId)
            $.ajax({
                type: 'Delete',
                async: true,
                processData: false,
                contentType: false,
                data: formdata,
                url: `/Cart/RemoveItemFromCart`,
                success: function (result) {
                    if (result.statusCode == 200) {
                        updateCart()
                      
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
    }
})