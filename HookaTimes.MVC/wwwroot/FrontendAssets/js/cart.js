﻿const removeItemBtns = document.querySelectorAll(".remove-item-btn")

function removeItemFromCart(e) {
    let pressedBtn = e.target
    let itemId = pressedBtn.dataset.productid
    console.log("desiodjsm")
    if (itemId) {
   
        let formdata = new FormData()
        formdata.append("productId", itemId)
        console.log("ajaxxxxx")
        $.ajax({
            type: 'Delete',
            async: true,
            processData: false,
            contentType: false,
            data: formdata,
            url: `/Cart/RemoveItemFromCart`,
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
    btn.addEventListener("click", removeItemFromCart)
})