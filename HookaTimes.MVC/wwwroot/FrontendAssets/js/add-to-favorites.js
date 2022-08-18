import { addToFav } from "../js/add-to-favorites-functionality.js"

const addToFavBtn = document.querySelector(".add-to-favorite-btn")




$(document).on("click", ".add-to-favorite-btn", function (e) {
   
    let pressedBtn = this
    console.log(pressedBtn)
    let placeId = $(this).attr("data-placeid")
    if (placeId) {
        let formdata = new FormData()
        formdata.append("placeId", placeId)
        let heartSvg = pressedBtn.querySelector('.favorite-icon')
        let favSvg = pressedBtn.querySelector("#fav-icon")
        let row = pressedBtn.closest('tr')

        if (heartSvg) {
            if (heartSvg.style.fill == 'red')
                heartSvg.style.fill = 'none'
            else heartSvg.style.fill = 'red'
        }

        if (favSvg) {
            var isFav = favSvg.getAttribute('data-isfav') === "True" || favSvg.getAttribute('data-isfav') == "true" ;
            console.log("initial", isFav)
            isFav = !isFav
            favSvg.setAttribute('data-isfav',isFav)
            changeFavIconSvg(isFav,favSvg)
        }

        if (row) {
            row.remove()
        }

        addToFav(formdata)
    }
});

function changeFavIconSvg(isFav, favSvg) {
    console.log("inside fn",isFav)
    if (isFav) {
        favSvg.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" fill="#fdcf51" width="16" height="16" viewBox="0 0 20 20" aria-labelledby="icon-svg-title- icon-svg-desc-" role="img" class="sc-rbbb40-0 kMNrPk"><title>bookmark-fill</title><path d="M15.020 0.9h-10.020c-1.060 0-1.92 0.84-1.92 1.9v0 16.42c0 0.28 0.16 0.5 0.36 0.62v0c0.12 0.060 0.24 0.1 0.38 0.1s0.24-0.040 0.36-0.1v0l5.82-3.52 5.82 3.52c0.1 0.060 0.24 0.1 0.38 0.1v0c0 0 0 0 0 0 0.12 0 0.24-0.040 0.34-0.1v0c0.22-0.12 0.36-0.34 0.36-0.62v-16.46c-0.020-1.040-0.86-1.86-1.88-1.86v0z"></path></svg>'
    } else {
        favSvg.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg"
                    fill = "#fdcf51"
                    width = "16" height = "16"
                    viewBox = "0 0 20 20" aria - labelledby="icon-svg-title- icon-svg-desc-"
                    role = "img" class="sc-rbbb40-0 kMNrPk" >
                                    <title>favorite add</title>
                                    <path d="M12.38 7.8h-1.66v-1.68c0-0.26-0.22-0.46-0.48-0.46v0h-0.48c-0.26 0-0.48 0.2-0.48 0.46v0 1.68h-1.66c-0.26 0-0.48 0.2-0.48 0.48v0 0.46c0 0.28 0.22 0.48 0.48 0.48v0h1.66v1.68c0 0.26 0.22 0.46 0.48 0.46v0h0.48c0.26 0 0.48-0.2 0.48-0.46v0-1.68h1.66c0.26 0 0.48-0.2 0.48-0.48v0-0.46c0-0.28-0.22-0.48-0.48-0.48v0zM15.020 0.9h-10.020c-1.060 0-1.92 0.84-1.92 1.9v0 16.42c0 0.28 0.16 0.5 0.36 0.62v0c0.12 0.060 0.24 0.1 0.38 0.1s0.24-0.040 0.36-0.1v0l5.82-3.52 5.82 3.52c0.1 0.060 0.24 0.1 0.38 0.1v0c0 0 0 0 0 0 0.12 0 0.24-0.040 0.34-0.1v0c0.22-0.12 0.36-0.34 0.36-0.62v-16.46c-0.020-1.040-0.86-1.86-1.88-1.86v0zM15.48 17.96l-5.1-3.080c-0.12-0.060-0.24-0.1-0.38-0.1s-0.26 0.040-0.38 0.1v0l-5.1 3.080v-15.24c0.040-0.22 0.22-0.4 0.46-0.4 0 0 0 0 0.020 0v0h10.020c0 0 0 0 0 0 0.24 0 0.44 0.2 0.46 0.44v0z"></path>
                                </svg > `
    }
}