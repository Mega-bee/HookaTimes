const filterBtn = document.querySelector('#filter-btn');
const resetFilterButton = document.querySelector("#reset-filter-btn")
const placesContainer = document.querySelector("#items-container")
let sortByValue = 0
let filterByValue = 0

function submitFilter(e) {
    const filterBy = document.querySelector('input[name=filterBy]:checked');
    const sortBy = document.querySelector('input[name="sortBy"]:checked');
    if (sortBy) {
        sortByValue = sortBy.value
    }
    if (filterBy) {
        filterByValue = filterBy.value
    }

    filterBuddies()
}

function filterBuddies() {
    $.ajax({
        type: 'GET',
        async: true,
        //data: data,
        url: `/Home/HookaBuddiesSearch?sortBy=${sortByValue}&filterBy=${filterByValue}`,
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
    const checkedCheckboxes = document.querySelectorAll('input[name=filterBy]:checked');
    const checkedRadioButton = document.querySelector('input[name="sortBy"]:checked');
    if (checkedCheckboxes.length) {
        checkedCheckboxes.forEach(checkbox => {
            checkbox.checked = false
        })
    }
    if (checkedRadioButton) {
        checkedRadioButton.checked = false
    }
    filterByValue = 0
    sortByValue = 0
    filterBuddies()
}

filterBtn.addEventListener('click', submitFilter)
resetFilterButton.addEventListener('click', resetFilters)

