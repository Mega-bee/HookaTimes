const addToFavBtn = document.querySelector(".add-to-favorite-btn")

document.addEventListener("click", e => {
    let pressedBtn = e.target
    if (pressedBtn.classList.contains("add-to-favorite-btn")) {
        let placeId = pressedBtn.dataset.placeid
        if (placeId) {
            let formdata = new FormData()
            formdata.append("placeId", placeId)
            let heartSvg = pressedBtn.querySelector('.favorite-icon')
            if (heartSvg.style.fill == 'red')
                heartSvg.style.fill = 'none'
            else heartSvg.style.fill = 'red'

            $.ajax({
                type: 'Post',
                async: true,
                processData: false,
                contentType: false,
                data: formdata,
                url: `/Places/AddToFavorites`,
                success: function (result) {

                  

                },
                fail: function (err) {

                   console.log(err)
                }
            })
        }
    }
})