document.addEventListener('DOMContentLoaded', (event) => {

    const setStatusbtn = document.querySelectorAll(".setStatusbtn");

    setStatusbtn.forEach(btn => {
        btn.addEventListener('click', handleSetStatus)
    })

    console.log(setStatusbtn)


    function handleSetStatus(e) {
        let btn = e.currentTarget;
        let statusId = btn.dataset.statusid;
        let invitationId = btn.dataset.invitationid;
        let btnvalue = btn.innerHTML
        console.log("StatusId=", statusId, "invitationId=", invitationId,"BtnValue", btnvalue)


        let data = new FormData()
        data.append("StatusId", statusId)
        data.append("invitationId", invitationId)
        btn.innerHTML = '<i class="fa fa-spinner fa-spin"></i>loading'
        setStatusbtn.forEach(btn => {
            btn.disabled = true;
        });

    $.ajax({
        type: 'Post',
        async: true,
        processData: false,
        contentType: false,
        data: data,
        url: `/Account/SetInvitationStatusMVC`,
        success: function (result) {
            btn.innerHTML = btnvalue
            setStatusbtn.disabled = false;
            if (result.statusCode == 201) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: result.data.message,
                })

                location.reload();
            } else {
                addToCartBtn.innerHTML = btnvalue
                addToCartBtn.disabled = false;
                Swal.fire({
                    icon: 'error',
                    title: 'Fail',
                    text: result.errorMessage
                })

                location.reload();

            }

        },
        fail: function (err) {

            console.log(err)
        }
    })

    }//handleSetStatus


})
