import { updateCart } from "../js/update-cart-dropdown.js"

const addToCartBtns = document.querySelectorAll(".add-to-cart-btn");
const dropdownCartItemsContainer = document.querySelector(".dropcart__products-list")
const itemsCount = document.querySelector(".cart-indicator-value")




//function updateCart() {
//    $.ajax({
//        type: 'GET',
//        async: true,
//        //data: data,
//        url: `/Cart/GetCartDropdown`,
//        success: function (result) {
//            let container = document.querySelector('.dropcart__body')
//            container.innerHTML = result;
          
//        },
//        fail: function (err) {
//            console.log(err)
//        }
//    })

//}


function handleAddToCart(e) {
  
    let btn = e.currentTarget;
    e.preventDefault()
    let productId = btn.dataset.productid

    let formdata = new FormData()
    formdata.append("productId", productId)
    formdata.append("quantity", 1)



    $.ajax({
        type: 'Post',
        async: true,
        processData: false,
        contentType: false,
        data: formdata,
        url: `/Cart/AddToCart`,
        success: function (result) {
            if (result.statusCode == 201) {
                
               
                updateCart()
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
addToCartBtns.forEach(btn => {
    btn.addEventListener('click',handleAddToCart)
})