import { updateCart } from "../js/update-cart-dropdown.js"

document.addEventListener("DOMContentLoaded", e => {
    const removeItemBtns = document.querySelectorAll(".remove-item-btn")
    const updateCartBtn = document.querySelector(".cart__update-button")
    const quantityInputs = document.querySelectorAll(".input-number__input")
    const cartContainer = document.querySelector("#cart-block")


    function getCartItems() {
        const cartItems = document.querySelectorAll(".cart-table__item")


        let itemsArray = []
        cartItems.forEach(el => {

            let itemId = el.dataset.productid
            console.log("item id", itemId)
            let qty = el.querySelector(".input-number__input")
            let item = {
                id: itemId,
                quantity: qty.value
            }

            itemsArray.push(item)
        })
        return itemsArray
    }

    function removeItemFromCart(e) {
        let pressedBtn = e.currentTarget
        let itemId = pressedBtn.dataset.productid
        if (itemId) {

            let formdata = new FormData()
            formdata.append("productId", itemId)
            let row = pressedBtn.closest("tr")
            if (row) {
                row.remove()
            }
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
            $.ajax({
                type: 'Delete',
                async: true,
                processData: false,
                contentType: false,
                data: formdata,
                url: `/Cart/RemoveItemFromCart`,
                success: function (result) {
                    if (result.statusCode == 200) {
                        updateCart(false)

                  
                    } else {
                     
                    }

                },
                fail: function (err) {

                    console.log(err)
                }
            })
        }
    }

    function handleUpdateCart(e) {
        e.preventDefault()
        const cartItems = document.querySelectorAll(".cart-table__item")


        let itemsArray = getCartItems()

        if (itemsArray) {
            let data = new FormData()
            for (var i = 0; i < itemsArray.length; i++) {
                data.append("[" + i + "].Id", itemsArray[i].id);
                data.append("[" + i + "].Quantity", itemsArray[i].quantity);
            }
            //console.log("itemsss",itemsArray)
            //itemsArray = JSON.stringify({ 'items': itemsArray });

            $.ajax({
                type: 'POST',
                async: true,
                processData: false,
                contentType: false,
                data: data,
                url: `/Cart/UpdateCart`,
                success: function (result) {
                    if (result.statusCode == 201) {
                        updateCart()
                        calculateNewCartTotal()
                        Swal.fire({
                            icon: 'success',
                            title: 'Success',
                            text: result.message
                        })

                    } else {
                 
                    }

                },
                fail: function (err) {
                 
                    console.log(err)
                }
            })
        }

    }

    function calculateNewCartTotal() {
        let cartItems = document.querySelectorAll(".cart-table__item")
        var totalCount = 0.00
        if (!cartItems) {
            return
        }
        cartItems.forEach(item => {
            let itemQty = parseFloat(item.querySelector(".input-number__input").value)
            let itemPrice = parseFloat(item.querySelector(".cart-item-price").textContent)
            let newItemPrice = parseFloat(itemQty * itemPrice)
            item.querySelector(".cart-item-total").textContent = newItemPrice + " " + "AED"
            totalCount += newItemPrice

        })
        document.querySelector("#total-cart-price").textContent = totalCount + " " + "AED"
    }

    removeItemBtns.forEach(btn => {
        btn.addEventListener("click", removeItemFromCart)
    })

    updateCartBtn.addEventListener("click", handleUpdateCart)

    quantityInputs.forEach(input => {
        input.addEventListener("change", calculateNewCartTotal)
    })

})


