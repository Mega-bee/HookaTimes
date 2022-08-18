

document.addEventListener('DOMContentLoaded', (event) => {

    const addToWishlistBtns = document.querySelectorAll(".add-to-wishlist-btn");
    let wishlistCount = document.querySelector(".wishlist-indicator-value") 

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
                    wishlistCount.textContent = parseInt(wishlistCount.textContent) + 1
                    notyf.success({ message: "Product added to wishlist" })
                } else if (result.statusCode == 204) {
                    wishlistCount.textContent = parseInt(wishlistCount.textContent) - 1
                    notyf.success({ message: "Product removed from wishlist" })
                } else {

              
                }

            },
            fail: function (err) {

                console.log(err)
            }
        })

    }
    addToWishlistBtns.forEach((btn) => {
        btn.addEventListener('click', handleAddToWishlist)
    })
});