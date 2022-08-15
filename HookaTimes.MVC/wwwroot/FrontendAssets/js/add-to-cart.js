const addToCartBtns = document.querySelectorAll(".add-to-cart-btn");

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