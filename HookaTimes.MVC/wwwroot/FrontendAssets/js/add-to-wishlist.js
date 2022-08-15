

document.addEventListener('DOMContentLoaded', (event) => {

    const addToWishlistBtns = document.querySelectorAll(".add-to-wishlist-btn");

    function handleAddToWishlist(e) {

        let btn = e.currentTarget;
        e.preventDefault()
        let productId = btn.dataset.productid

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
    addToWishlistBtns.forEach((btn) => {
        console.log(btn)
        btn.addEventListener('click', handleAddToWishlist)
    })
});