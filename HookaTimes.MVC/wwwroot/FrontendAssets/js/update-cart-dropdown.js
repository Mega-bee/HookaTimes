export function updateCart() {
    $.ajax({
        type: 'GET',
        async: true,
        //data: data,
        url: `/Cart/GetCartDropdown`,
        success: function (result) {
            let container = document.querySelector('.dropcart__body')
            let itemCountContainer = document.querySelector(".cart-indicator-value")
            container.innerHTML = result;
            let elementsCount = document.querySelectorAll('.dropcart__product').length
          
            itemCountContainer.textContent = elementsCount - 1
        },
        fail: function (err) {
            console.log(err)
        }
    })

}