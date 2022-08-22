const formContainer = document.querySelector(".checkout-form-groups-container")
const addressDropdown = document.querySelector("#checkout-address")

$('#checkout-address').on('change', function (e) {
    let selectedVal = e.target.value
    if (selectedVal == 0) {
        formContainer.style.display = "block"
    } else {
        formContainer.style.display = "none"
    }
});
