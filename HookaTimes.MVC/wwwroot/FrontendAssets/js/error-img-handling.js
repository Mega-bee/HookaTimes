$('img').on('error', function () {
    console.log("error")
    $(this).attr('src','https://source.unsplash.com/random/?random,foodstore')
})