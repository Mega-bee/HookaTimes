import { updateCart } from "../js/update-cart-dropdown.js"
import { calculateNewCartTotal } from "../js/calculate-cart-total.js"
const cartContainer = document.querySelector("#cart-block")

$(document).on("click", ".dropcart__product-remove", function (e) {
 
    let itemId = $(this).attr("data-productid")
    if (itemId) {

            let row = $(this).closest(".dropcart__product")
            if (row) {
                row.remove()
        }

        let currUrl = window.location.pathname.split("/");
        let controller = currUrl[1]
        if (controller == "Cart") {
            let row = document.querySelector(`tr[data-productid='${itemId}']`)
            if (row) {
                row.remove()
            }
            calculateNewCartTotal()
            let allRows = document.querySelectorAll(".cart-table__item")
            if (allRows.length == 0) {
                cartContainer.innerHTML = `  <div class="not-found">
                <div class="not-found__content">
                    <h1 class="not-found__title">Your cart is empty</h1>
                    <p class="not-found__text">We can't seem to find anything in your cart</p>
                    <a href="/Home/HookaProducts" class="btn btn-light">Continue Shopping</a>
                </div>
            </div>`
            }
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