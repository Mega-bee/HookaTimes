export function updateCart(toggleCart = true) {
    $.ajax({
        type: 'GET',
        async: true,
        //data: data,
        url: `/Cart/GetCartDropdown`,
        success: function (result) {
            let container = document.querySelector('.dropcart__body')
            let itemCountContainer = document.querySelector(".cart-indicator-value")
            let itemCountContainerMobile = document.querySelector(".cart-indicator-value-mobile")
            container.innerHTML = result;
            let elementsCount = document.querySelectorAll('.dropcart__product').length
            if (itemCountContainer) {
                itemCountContainer.textContent = elementsCount - 1
            }
            
            if (itemCountContainerMobile) {
                itemCountContainerMobile.textContent = elementsCount - 1
            }
          
            let navPanel = document.querySelector(".nav-panel");
            navPanel.classList.add("nav-panel--stuck", "nav-panel--show")
            let cartDropdownContainer = document.querySelector('#navbar-cart-container')

            if (toggleCart) {
                let cartDropdownIcon = document.querySelector("#navbar-cart-container .indicator__button")
                if (!cartDropdownContainer.classList.contains("indicator--open")) {
                    cartDropdownIcon.click()
                }
            }
        
          
        },
        fail: function (err) {
            console.log(err)
        }
    })

}