// Function for hiding span error messages
function HideErrorSpan(componentId) {
    var component = document.getElementById(componentId);
    component.textContent = "";
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

// Function for showing message after successful registration
function ThrowRegistrationSuccessMessage() {
    Swal.fire({
        title: "Success!",
        text: "You have registered successfully. Please, Sign In with your credentials.",
        icon: 'success',
        confirmButtonColor: '#007aff'
    });
}