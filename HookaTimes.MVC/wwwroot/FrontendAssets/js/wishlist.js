const removeItemBtns = document.querySelectorAll(".remove-item-btn")
let wishlistCount = document.querySelector(".wishlist-indicator-value") 
const wishlistContainer = document.querySelector("#wishlist-block")

function removeItemFromWishlist(e) {

    let pressedBtn = e.currentTarget
    console.log(pressedBtn)
    let itemId = pressedBtn.dataset.productid

    if (itemId) {

        let formdata = new FormData()
        formdata.append("productId", itemId)
        let row = pressedBtn.closest("tr")
        if (row) {
            row.remove()
        }
        let allRows = document.querySelectorAll(".wishlist__row")
        if (allRows.length - 1 == 0) {
            wishlistContainer.innerHTML = `<div class="not-found">
                <div class="not-found__content">
                    <h1 class="not-found__title">Your wishlist is empty!</h1>
                    <p class="not-found__text">We can't seem to find anything in your wishlist</p>
                    <a href="/Home/HookaProducts" class="btn btn-light">Continue Shopping</a>
                </div>
            </div>`
        }
        $.ajax({
            type: 'Delete',
            async: true,
            processData: false,
            contentType: false,
            data: formdata,
            url: `/Wishlist/RemoveItemFromWishlist`,
            success: function (result) {
                if (result.statusCode == 200) {
                    wishlistCount.textContent = parseInt(wishlistCount.textContent) - 1
                    
                  
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


removeItemBtns.forEach(btn => {
    btn.addEventListener("click", removeItemFromWishlist)
})