// Function to show message about successful logout 
function ShowSuccessfulMessageAfterLogout() {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'success',
        title: 'Logged out successfully.'
    });
}

//Function to show message about error when tried to get current user location
function ShowErrorMessageAfterGeolocationServiceFailed(errorMessage) {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: errorMessage,
        confirmButtonColor: '#257cfd',
        timer: 10000,
        timerProgressBar: true
    })
}

//Function to show message about error occured
function ShowErrorMessage(errorMessage) {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: errorMessage,
        confirmButtonColor: '#257cfd',
        timer: 10000,
        timerProgressBar: true
    })
}

// Function to show message about successful filters adding 
function ShowSuccessfulMessageAfterAppliedFilters() {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 2000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'success',
        title: 'Filters were applied successfully.'
    });
}

// Function to show message about successful vehicle deletion
function ShowSuccessfulMessageAfterVehicleWasDeleted() {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'success',
        title: 'Vehicle was deleted successfully.'
    });
}

// Function to show message about successful vehicle deletion
function ShowSuccessfulMessageAfterRentalWasFinished() {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'success',
        title: 'Rental was finished successfully.'
    });
}

// Function to show message about successful rental deletion
function ShowSuccessfulMessageAfterRentalWasDeleted() {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'success',
        title: 'Rental information was deleted successfully.'
    });
}

// Function to show user an alert press logout button
function ShowLogoutConfirmationAlert() {
    Swal.fire({
        title: 'Are you sure?',
        text: "You will need to Sign In again!",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#4FA64F',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = '/signIn/logout'
        }
    });
}

// Function to show message about successful authorization 
function ShowSuccessfulMessageAfterLogIn() {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'success',
        title: 'Signed in successfully.'
    });
}

// Function to show message about successful account data changing
function ShowSuccessfulMessageAfterUserHasChangedAccountData() {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'success',
        title: 'Account data was changed successfully.'
    });
}

// Function to show message about successful vehicle data changing
function ShowSuccessfulMessageAfterUserHasChangedVehicleData() {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'success',
        title: 'Vehicle data was changed successfully.'
    });
}

// Function to show message about successful user password changing
function ShowSuccessfulMessageAfterUserHasChangedPassword() {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'success',
        title: 'Password was changed successfully.'
    });
}

// Function to show message about successful vehicle addition
function ShowSuccessfulCarSharingMessage() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-secondary'
        }
    })

    swalWithBootstrapButtons.fire({
        title: 'Successfuly shared!',
        text: "Vehicle was successfully sent to our administrators for review. Check vehicle status on Dashboard page right now!",
        icon: 'success',
        showCancelButton: true,
        confirmButtonColor: '#007aff',
        confirmButtonText: 'Check',
        cancelButtonText: 'Later',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = '/dashboard/';
        }
    })
}

// Function to show message about uncompleted order payment process
function ShowPurchaseCancelledMessage() {
    Swal.fire({
        title: "Payment has been canceled!",
        text: "The order has not been completed. If you haven't canceled your order, please try again...",
        icon: 'error',
        confirmButtonColor: '#007aff'
    });
}

// Function to show message about completed order payment process
function ShowPurchaseCompletedMessage() {

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-secondary'
        }
    })

    swalWithBootstrapButtons.fire({
        title: 'Congratulations!',
        text: "You have successfully completed payment process. Check new order in your Account.",
        icon: 'success',
        showCancelButton: true,
        confirmButtonColor: '#007aff',
        confirmButtonText: 'Check',
        cancelButtonText: 'Later',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = '/dashboard/';
        }
    })
}

// Function to show message about successful rental finish
function ShowSuccessfulOrderFinishedMessage() {

    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 1500,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'success',
        title: 'Order was finished successfully.'
    });
}

// Function for showing message after successful registration
function ThrowRegistrationSuccessMessage() {
    Swal.fire({
        title: "Success!",
        text: "You have registered successfully. Please, Sign In with your credentials.",
        icon: 'success',
        confirmButtonColor: '#007aff'
    });
}

// Function for showing an error when sign in failed
function ThrowSignInErrorMessage() {
    Swal.fire({
        title: "Authorization Failed!",
        text: "You have provided wrong credentials. Check and try again.",
        icon: 'error',
        confirmButtonColor: '#007aff'
    });
}

//Function to show message about Unauthorized 401 error
function ShowUnauthorizedErrorMessage() {

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-secondary'
        }
    })

    swalWithBootstrapButtons.fire({
        title: '401 Unauthorized',
        text: "We couldn't validate your credentials. Please sign in and then try again.",
        icon: 'error',
        showCancelButton: true,
        confirmButtonColor: '#007aff',
        confirmButtonText: 'Sign in',
        cancelButtonText: 'Later',
        timer: 10000,
        timerProgressBar: true,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = '/signIn/';
        }
    })
}

//Function to show message about Unauthorized 401 error
function ShowForbiddenErrorMessage() {

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-secondary'
        }
    })

    swalWithBootstrapButtons.fire({
        title: '401 Unauthorized',
        text: "We couldn't validate your credentials. Please sign in and then try again.",
        icon: 'error',
        showCancelButton: true,
        confirmButtonColor: '#007aff',
        confirmButtonText: 'Sign in',
        cancelButtonText: 'Later',
        timer: 10000,
        timerProgressBar: true,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = '/signIn/';
        }
    })
}

//Function to show message about Forbidden 403 error
function ShowForbiddenErrorMessage() {

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-secondary'
        }
    })

    swalWithBootstrapButtons.fire({
        title: '403 Forbidden',
        text: "Sorry but you don't have permission to access this area.",
        icon: 'warning',
        confirmButtonColor: '#007aff',
        timer: 10000,
        timerProgressBar: true
    })
}

//Function to show message about NotFound 404 error
function ShowNotFoundErrorMessage() {

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-secondary'
        }
    })

    swalWithBootstrapButtons.fire({
        title: '404 Not Found',
        text: "Sorry but there is no information about what you are looking for.",
        icon: 'question',
        confirmButtonColor: '#007aff',
        timer: 10000,
        timerProgressBar: true
    })
}

//Function to show message about Internal Server Error 500 and Service Unavailable 503 error
function ShowInternalServerErrorMessage() {

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-secondary'
        }
    })

    swalWithBootstrapButtons.fire({
        title: '500 Server Error',
        text: "Something has happened on the server side or it might be overloaded. Please try againg later.",
        icon: 'error',
        confirmButtonColor: '#007aff',
        timer: 10000,
        timerProgressBar: true
    })
}