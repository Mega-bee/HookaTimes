export function calculateNewCartTotal() {
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