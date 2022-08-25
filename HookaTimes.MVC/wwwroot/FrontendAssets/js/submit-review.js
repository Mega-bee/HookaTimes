let submitBtn = document.querySelector('.submit-review-btn')
let form = document.querySelector('.reviews-view__form')

function appendNewReview(review) {
    console.log(review)
    let originalNode = document.querySelector(".reviews-list__item")
    let clonedReview = originalNode.cloneNode(true)
    let img = clonedReview.querySelector(".review__avatar-img")
    let name = clonedReview.querySelector(".review__author")
    let reviewText = clonedReview.querySelector(".review__text")
    let
    img.src = review.image

  

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
                }, 3000)

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

form.addEventListener('submit',handleFormSubmit)