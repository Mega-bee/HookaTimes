let submitBtn = document.querySelector('.submit-review-btn')
let form = document.querySelector('.reviews-view__form')

function appendNewReview(review) {

    let container = document.querySelector(".reviews-list__content")
    let originalNode = document.querySelector(".reviews-list__item")
    let clonedReview = originalNode.cloneNode(true)
    let img = clonedReview.querySelector(".review__avatar-img")
    let name = clonedReview.querySelector(".review__author")
    let reviewText = clonedReview.querySelector(".review__text")
    let reviewDate = clonedReview.querySelector(".review__date")
    let reviewContainer = clonedReview.querySelector(".rating__body")
    clonedReview.style.display = 'block'
    img.src = review.image
    name.textContent = review.name
    reviewText.textContent = review.description
    let createdDate = new Date(review.createdDate).toLocaleString()
    reviewDate.textContent = createdDate
    let rating = ` <svg class="rating__star rating__star--active"
                                                                     width="13px"
                                                                     height="12px">
                                                                            <g class="rating__fill">
                                                                                <use xlink:href="/FrontendAssets/images/sprite.svg#star-normal"></use>
                                                                            </g>
                                                                            <g class="rating__stroke">
                                                                                <use xlink:href="/FrontendAssets/images/sprite.svg#star-normal-stroke"></use>
                                                                            </g>
                                                                        </svg>

                                                                        <div class="rating__star rating__star--only-edge rating__star--active">
                                                                            <div class="rating__fill">
                                                                                <div class="fake-svg-icon"></div>
                                                                            </div>
                                                                            <div class="rating__stroke">
                                                                                <div class="fake-svg-icon"></div>
                                                                            </div>
                                                                        </div>`
    for (var i = 0; i < review.rating; i++) {
        reviewContainer.innerHTML += rating
    }
    container.appendChild(clonedReview)
}

function handleFormSubmit(e) {
    e.preventDefault()
    let descInput = document.querySelector('#review-text')
    let ratingInput = document.querySelector('#review-stars')
    let validationErrorMsgEl = document.querySelector('#validation-msg')
    if (!descInput.value) {

        validationErrorMsgEl.textContent = 'Please add description';
        return
    }

    submitBtn.innerHTML = '<i class="fa fa-spinner fa-spin"></i> Post Your Review'
    submitBtn.disabled = true
    validationErrorMsgEl.textContent = ''
    let fromdata = new FormData()
    let placeId = form.dataset.placeid
    fromdata.append("rating", ratingInput.value)
    fromdata.append("description", descInput.value)

    $.ajax({
        type: 'Post',
        async: true,
        processData: false,
        contentType: false,
        data: fromdata,
        url: `/Places/AddReview/${placeId}`,
        success: function (result) {
            descInput.value = ''
            if (result.statusCode == 200) {
                submitBtn.innerHTML = '<i class="fas fa-check"></i>'
                setTimeout(() => {
                    submitBtn.innerHTML = 'Post Your Review'
                    submitBtn.disabled = false
                    //window.location.reload()
                }, 2000)

                notyf.success({ message: "Your review has been submitted" })
                appendNewReview(result.data.data)

            } else {
                submitBtn.innerHTML = 'Post Your Review'
                submitBtn.disabled = false
            }

        },
        complete: function (xhr, textStatus) {

            if (xhr.status == 401) {


                notyf.error({ message: "Your are not logged in, please login" })
            }
        },
        fail: function (err) {
            submitBtn.innerHTML = 'Post Your Review'
            submitBtn.disabled = false
            console.log(err)
        }
    })

}

form.addEventListener('submit', handleFormSubmit)