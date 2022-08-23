$(document).on("click", ".share-btn", function (e) {
    let domain = window.location.origin
    let btn = this
    console.log(btn)
    let parent = btn.parentElement.parentElement
    console.log(parent)
    let sub = parent.querySelector(".product-image__body").href
    let link = sub
    console.log(link)
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(link).select();
    document.execCommand("copy");
    $temp.remove();
    notyf.success({ message:"Link copied to clipboard" })
})