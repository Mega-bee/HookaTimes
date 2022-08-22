const filterBtn = document.querySelector('#filter-btn');
const resetFilterButton = document.querySelector("#reset-filter-btn")
const placesContainer = document.querySelector("#items-container")
var cuisineQuery = ""
var sortByValue = 0

function submitFilter(e) {
    console.log("submittttttttttttttttttttttttttttttttttt")
    var cuisines = []
    cuisineQuery = ""
    sortByValue = 0
    const cuisinesCheckboxes = document.querySelectorAll('input[name=cuisine]:checked');
    const sortBy = document.querySelector('input[name="sortBy"]:checked');
    if (sortBy) {
        sortByValue = sortBy.value
    }
    if (cuisinesCheckboxes.length) {
       cuisines =  Array.from(cuisinesCheckboxes).map(checkbox => checkbox.value)
    }
    console.log(cuisinesCheckboxes)
    console.log(cuisines)

  
   
    if (cuisines.length) {
        cuisines.forEach(cuisine => {
            cuisineQuery+= '&cuisines=' + cuisine
        })
    }

    filterPlaces()

    var url = '@Url.Action("HookaPlacesSearch", "Home")';
}

function filterPlaces() {
    $.ajax({
        type: 'GET',
        async: true,
        //data: data,
        url: `/Home/HookaPlacesSearch?${cuisineQuery}&sortBy=${sortByValue}`,
        success: function (result) {
            placesContainer.innerHTML = result;
        },
        fail: function (err) {
            console.log(err)
        }
    })
    //}).done(function (result) {
    //    console.log(result)

    //});
}


function resetFilters(e) {
    const checkedCheckboxes = document.querySelectorAll('input[name=cuisine]:checked');
    const checkedRadioButton = document.querySelector('input[name="sortBy"]:checked');
    if (checkedCheckboxes.length) {
        checkedCheckboxes.forEach(checkbox => {
            checkbox.checked = false
        })
    }
    if (checkedRadioButton) {
        checkedRadioButton.checked = false
    }
    cuisineQuery = ""
    sortByValue = 0
    filterPlaces()
}

filterBtn.addEventListener('click', submitFilter)
resetFilterButton.addEventListener('click',resetFilters)

