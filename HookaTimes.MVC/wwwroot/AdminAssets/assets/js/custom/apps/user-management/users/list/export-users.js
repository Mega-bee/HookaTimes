"use strict";

// Class definition
var KTModalExportUsers = function () {
    var table = document.getElementById('kt_table_users');
    // Shared variables
    //const element = document.getElementById('kt_modal_export_users');
    //const form = element.querySelector('#kt_modal_export_users_form');
    //const modal = new bootstrap.Modal(element);

    // Init form inputs
    //var initForm = function () {

        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        //var validator = FormValidation.formValidation(
        //    form,
        //    {
        //        fields: {
        //            'format': {
        //                validators: {
        //                    notEmpty: {
        //                        message: 'File format is required'
        //                    }
        //                }
        //            },
        //        },
        //        plugins: {
        //            trigger: new FormValidation.plugins.Trigger(),
        //            bootstrap: new FormValidation.plugins.Bootstrap5({
        //                rowSelector: '.fv-row',
        //                eleInvalidClass: '',
        //                eleValidClass: ''
        //            })
        //        }
        //    }
        //);

        //// Submit button handler
        //const submitButton = element.querySelector('[data-kt-users-modal-action="submit"]');
        //submitButton.addEventListener('click', function (e) {
        //    e.preventDefault();

        //    // Validate form before submit
        //    if (validator) {
        //        validator.validate().then(function (status) {
        //            console.log('validated!');

        //            if (status == 'Valid') {
        //                submitButton.setAttribute('data-kt-indicator', 'on');

        //                // Disable submit button whilst loading
        //                submitButton.disabled = true;

        //                setTimeout(function () {
        //                    submitButton.removeAttribute('data-kt-indicator');

        //                    Swal.fire({
        //                        text: "User list has been successfully exported!",
        //                        icon: "success",
        //                        buttonsStyling: false,
        //                        confirmButtonText: "Ok, got it!",
        //                        customClass: {
        //                            confirmButton: "btn btn-primary"
        //                        }
        //                    }).then(function (result) {
        //                        if (result.isConfirmed) {
        //                            modal.hide();

        //                            // Enable submit button after loading
        //                            submitButton.disabled = false;
        //                        }
        //                    });

        //                    //form.submit(); // Submit form
        //                }, 2000);
        //            } else {
        //                Swal.fire({
        //                    text: "Sorry, looks like there are some errors detected, please try again.",
        //                    icon: "error",
        //                    buttonsStyling: false,
        //                    confirmButtonText: "Ok, got it!",
        //                    customClass: {
        //                        confirmButton: "btn btn-primary"
        //                    }
        //                });
        //            }
        //        });
        //    }
        //});

        //// Cancel button handler
        //const cancelButton = element.querySelector('[data-kt-users-modal-action="cancel"]');
        //cancelButton.addEventListener('click', function (e) {
        //    e.preventDefault();

        //    Swal.fire({
        //        text: "Are you sure you would like to cancel?",
        //        icon: "warning",
        //        showCancelButton: true,
        //        buttonsStyling: false,
        //        confirmButtonText: "Yes, cancel it!",
        //        cancelButtonText: "No, return",
        //        customClass: {
        //            confirmButton: "btn btn-primary",
        //            cancelButton: "btn btn-active-light"
        //        }
        //    }).then(function (result) {
        //        if (result.value) {
        //            form.reset(); // Reset form	
        //            modal.hide(); // Hide modal				
        //        } else if (result.dismiss === 'cancel') {
        //            Swal.fire({
        //                text: "Your form has not been cancelled!.",
        //                icon: "error",
        //                buttonsStyling: false,
        //                confirmButtonText: "Ok, got it!",
        //                customClass: {
        //                    confirmButton: "btn btn-primary",
        //                }
        //            });
        //        }
        //    });
        //});

        //// Close button handler
        //const closeButton = element.querySelector('[data-kt-users-modal-action="close"]');
        //closeButton.addEventListener('click', function (e) {
        //    e.preventDefault();

        //    Swal.fire({
        //        text: "Are you sure you would like to cancel?",
        //        icon: "warning",
        //        showCancelButton: true,
        //        buttonsStyling: false,
        //        confirmButtonText: "Yes, cancel it!",
        //        cancelButtonText: "No, return",
        //        customClass: {
        //            confirmButton: "btn btn-primary",
        //            cancelButton: "btn btn-active-light"
        //        }
        //    }).then(function (result) {
        //        if (result.value) {
        //            form.reset(); // Reset form	
        //            modal.hide(); // Hide modal				
        //        } else if (result.dismiss === 'cancel') {
        //            Swal.fire({
        //                text: "Your form has not been cancelled!.",
        //                icon: "error",
        //                buttonsStyling: false,
        //                confirmButtonText: "Ok, got it!",
        //                customClass: {
        //                    confirmButton: "btn btn-primary",
        //                }
        //            });
        //        }
        //    });
        //});

    // Export Buttons
    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = 'Customer Orders Report';
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }
    

    return {
        // Public functions
        init: function () {
            //initForm();
            exportButtons();

        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTModalExportUsers.init();
});