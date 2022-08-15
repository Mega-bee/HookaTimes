const productItems = document.querySelectorAll(".product-item")
const addToCartBtn = document.querySelector("#add-to-cart-btn")
const quantityEl = document.querySelector("#product-quantity")
const productFrom = document.querySelector("#product-fom")
const productDescriptionElement = document.querySelector(".product__description")
const productPriceElement = document.querySelector(".product__prices")

function handleSelectItem(e) {
    for (const item of document.querySelectorAll(".active-item")) {
        if (item.classList.contains("active-item")) item.classList.remove("active-item");
        
    }
    event.currentTarget.classList.add("active-item");
    refreshProduct(event.currentTarget)
}

function refreshProduct(product) {
    let price = product.dataset.price;
    let description = product.dataset.productdesc
    productDescriptionElement.textContent = description
    productPriceElement.textContent = price
}

function handleAddToCart(e) {
    e.preventDefault()
    let qty = quantityEl.value;
    console.log("hellooooo")
    console.log("producttttt",document.querySelector(".active-item"))
    let productId = document.querySelector(".active-item").dataset.productid

    let formdata = new FormData() 
    formdata.append("productId",productId)
    formdata.append("quantity", qty)
    addToCartBtn.innerHTML = '<i class="fa fa-spinner fa-spin"></i> Add To Cart'
    addToCartBtn.disabled = true;
    $.ajax({
        type: 'Post',
        async: true,
        processData: false,
        contentType: false,
        data: formdata,
        url: `/Cart/AddToCart`,
        success: function (result) {
            addToCartBtn.innerHTML = 'Add To Cart'
            addToCartBtn.disabled = false;
            if (result.statusCode == 201) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: result.data.message,
                })
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Fail',
                    text: result.errorMessage
                })
            }
           
        },
        fail: function (err) {
            addToCartBtn.innerHTML = 'Add To Cart'
            addToCartBtn.disabled = false;
            console.log(err)
        }
    })

}

if (productItems.length) {
    productItems.forEach(el => {
        el.addEventListener('click',handleSelectItem)
    })
}



productFrom.addEventListener("submit",handleAddToCart)

