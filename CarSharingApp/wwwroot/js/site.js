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
        title: 'Logged out successfully'
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
            window.location.href = '/SignIn/Logout'
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
        title: 'Signed in successfully'
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
        title: 'Account data was changed successfully'
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
        title: 'Vehicle data was changed successfully'
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
        title: 'Password was changed successfully'
    });
}

// Function to show message about unauthorized access attempt
function ShowSuccessfulCarSharingMessage() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-secondary'
        }
    })

    swalWithBootstrapButtons.fire({
        title: 'Successfuly shared!',
        text: "Vehicle was added to your personal account page. Go there and activate it right now!",
        icon: 'success',
        showCancelButton: true,
        confirmButtonColor: '#007aff',
        confirmButtonText: 'Activate',
        cancelButtonText: 'Later',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = '/ClientPersonalAccount/';
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
    Swal.fire({
        title: "Congratulations!",
        text: "You have successfully completed payment process. Check new order in your Account.",
        icon: 'success',
        confirmButtonColor: '#007aff'
    });
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
        title: 'Order was finished successfully'
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
