import { updateCart } from "../js/update-cart-dropdown.js"

export function addToCart(data,btn = null) {
    $.ajax({
        type: 'Post',
        async: true,
        processData: false,
        contentType: false,
        data: data,
        url: `/Cart/AddToCart`,
        success: function (result) {
            if (result.statusCode == 201) {


                updateCart()
                console.log(btn)
                if (btn) {
                    btn.innerHTML = 'Add To Cart'
                    btn.disabled = false;
                }
            } else {
          
                if (btn) {
                    btn.innerHTML = 'Add To Cart'
                    btn.disabled = false;
                }
            }

        },
        fail: function (err) {

            console.log(err)
        }
    })
}