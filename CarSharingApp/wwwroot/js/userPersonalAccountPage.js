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
                url: "/UserPersonalAccount/DeleteVehicle/",
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

// Function for showing alert when user try to finish an existing order
function ShowFinishOrderConfirmationAlert(orderId) {
    Swal.fire({
        title: 'Are you sure you want to finish order?',
        text: "All unused time you have now will disapperar!",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#4FA64F',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: "/UserPersonalAccount/FinishOrder/",
                data: {
                    orderId: orderId
                },
                method: "POST"
            });

            localStorage.setItem("HasFinishedOrder", "True");

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

    if (localStorage.getItem("HasFinishedOrder") === "True") {

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

        localStorage.removeItem("HasFinishedOrder");

        return;
    }
});

