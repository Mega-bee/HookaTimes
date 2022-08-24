$(document).on("click", ".share-btn", function (e) {
    e.preventDefault()
    let btn = this
    console.log(btn)
    var sub = ""
    var link = ""
    if (btn.tagName.toLowerCase() === 'a') {
        e.preventDefault()
        sub = btn.href; //this is the url where the anchor tag points to.
    } else {
        let parent = btn.parentElement.parentElement
         sub = parent.querySelector(".product-image__body").href
    }
    
   
     link = sub
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(link).select();
    document.execCommand("copy");
    $temp.remove();
    notyf.success({ message:"Link copied to clipboard" })
})