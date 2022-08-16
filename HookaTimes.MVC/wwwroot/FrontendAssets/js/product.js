
import { addToCart } from "../js/add-to-cart-functionality"

const productItems = document.querySelectorAll(".product-item")
const addToCartBtn = document.querySelector("#add-to-cart-btn")
const quantityEl = document.querySelector("#product-quantity")
const productFrom = document.querySelector("#product-fom")
const productDescriptionElement = document.querySelector(".product__description")
const productPriceElement = document.querySelector(".product__prices")
const addToWishlistBtn = document.querySelector(".add-to-wishlist-btn")

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
    productPriceElement.textContent = price + " " + "AED"
}

function handleAddToCart(e) {
    e.preventDefault()
    let qty = quantityEl.value;
    let productId = document.querySelector(".active-item").dataset.productid

    let formdata = new FormData() 
    formdata.append("productId",productId)
    formdata.append("quantity", qty)
    addToCartBtn.innerHTML = '<i class="fa fa-spinner fa-spin"></i> Add To Cart'
    addToCartBtn.disabled = true;
    addToCart(formdata)
    //$.ajax({
    //    type: 'Post',
    //    async: true,
    //    processData: false,
    //    contentType: false,
    //    data: formdata,
    //    url: `/Cart/AddToCart`,
    //    success: function (result) {
    //        addToCartBtn.innerHTML = 'Add To Cart'
    //        addToCartBtn.disabled = false;
    //        if (result.statusCode == 201) {
    //            updateCart()
    //            Swal.fire({
    //                icon: 'success',
    //                title: 'Success',
    //                text: result.data.message,
    //            })
    //        } else {
    //            Swal.fire({
    //                icon: 'error',
    //                title: 'Fail',
    //                text: result.errorMessage
    //            })
    //        }
           
    //    },
    //    fail: function (err) {
    //        addToCartBtn.innerHTML = 'Add To Cart'
    //        addToCartBtn.disabled = false;
    //        console.log(err)
    //    }
    //})

}

function handleAddToWishlist(e) {
    let btn = e.currentTarget
    let qty = quantityEl.value;
    let productId = document.querySelector(".active-item").dataset.productid

    let formdata = new FormData()
    formdata.append("productId", productId)
    let heartSvg = btn.querySelector('.wishlist-icon')
    if (heartSvg.style.fill == 'red')
        heartSvg.style.fill = 'none'
    else heartSvg.style.fill = 'red'
    $.ajax({
        type: 'Post',
        async: true,
        processData: false,
        contentType: false,
        data: formdata,
        url: `/Wishlist/AddToWishlist`,
        success: function (result) {
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
            console.log(err)
        }
    })

}

if (productItems.length) {
    productItems.forEach(el => {
        el.addEventListener('click',handleSelectItem)
    })
}



productFrom.addEventListener("submit", handleAddToCart)
addToWishlistBtn.addEventListener("click",handleAddToWishlist)

