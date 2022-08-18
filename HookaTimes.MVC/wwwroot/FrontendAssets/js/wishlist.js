const removeItemBtns = document.querySelectorAll(".remove-item-btn")

function removeItemFromWishlist(e) {

    let pressedBtn = e.currentTarget
    console.log(pressedBtn)
    let itemId = pressedBtn.dataset.productid

    if (itemId) {

        let formdata = new FormData()
        formdata.append("productId", itemId)
        $.ajax({
            type: 'Delete',
            async: true,
            processData: false,
            contentType: false,
            data: formdata,
            url: `/Wishlist/RemoveItemFromWishlist`,
            success: function (result) {
                if (result.statusCode == 200) {
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
}


removeItemBtns.forEach(btn => {
    btn.addEventListener("click", removeItemFromWishlist)
})