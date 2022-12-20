// Function for showing alert when user want to delete vehicle
function ShowDeleteVehicleConfirmationAlert(deleteVehicleId) {
    Swal.fire({
        title: 'Are you sure you want to delete this vehicle?',
        text: "You won't be able to cancel this action!",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#4FA64F',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: "/ClientPersonalAccount/DeleteVehicle/",
                data: {
                    vehicleId: deleteVehicleId
                },
                method: "POST"
            });

            localStorage.setItem("HasDeletedVehicle", "True");

            // Sometimes window location reload works, sometimes not, so I've putted both to be sure that page will be reloaded successfully
            window.location.reload(true);
            document.location.reload(true);
        }
    });
}

// Event listener for showing successful message after vehicle was deleted
window.addEventListener("DOMContentLoaded", () => {

    if (localStorage.getItem("HasDeletedVehicle") === "True") {

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
            title: 'Vehicle was deleted successfully'
        });

        localStorage.removeItem("HasDeletedVehicle");

        return;
    }
});

// Function for checking the submit form (that all the ratings were set, if user submits )
function SubmitFinishOrderForm(e, submitRating) {
    e.preventDefault();

    if (submitRating === "True") {
        var errorsCounter = 0;

        if (document.getElementById("TotalConditionStars").textContent === "0") {
            document.getElementById("ConditionErrorSpan").setAttribute("class", "text-danger text-center");
            errorsCounter = errorsCounter + 1;
        }
        if (document.getElementById("TotalFuelConsumptionStars").textContent === "0") {
            document.getElementById("FuelConsumptionErrorSpan").setAttribute("class", "text-danger text-center");
            errorsCounter = errorsCounter + 1;
        }
        if (document.getElementById("TotalEasyToDriveStars").textContent === "0") {
            document.getElementById("EasyToDriveErrorSpan").setAttribute("class", "text-danger text-center");
            errorsCounter = errorsCounter + 1;
        }
        if (document.getElementById("TotalFamilyFriendlyStars").textContent === "0") {
            document.getElementById("FamilyFriendlyErrorSpan").setAttribute("class", "text-danger text-center");
            errorsCounter = errorsCounter + 1;
        }
        if (document.getElementById("TotalSUVStars").textContent === "0") {
            document.getElementById("SUVErrorSpan").setAttribute("class", "text-danger text-center");
            errorsCounter = errorsCounter + 1;
        }

        if (errorsCounter === 0) {
            document.getElementById("hasSubmittedRating").checked = true;
            document.getElementById("hasSubmittedRating").value = true;
            document.getElementById("FinishOrderForm").submit();
        }
    }
    else {
        document.getElementById("hasSubmittedRating").checked = false;
        document.getElementById("hasSubmittedRating").value = false;
        document.getElementById("FinishOrderForm").submit();
    }
}

// Function to hide all error spans connected with rating 
function SetTotalStarsValueAndHideErrorSpan(componentId, starsValue) {

    var errorSpan = document.getElementById(componentId + "ErrorSpan");
    errorSpan.setAttribute("class", "visually-hidden");

    var totalAmountSpan = document.getElementById("Total" + componentId + "Stars");
    totalAmountSpan.textContent = starsValue;

}

