// Function for checking textarea input to prevent multiple enters
function CheckTextAreaInput(e) {
    if (e.keyCode == 13 && !e.shiftKey) {
        e.preventDefault();
    }
}

// EventListener for removing stored lat and long from local storage
addEventListener("beforeunload", () => {
    localStorage.removeItem("marker-lat");
    localStorage.removeItem("marker-long");
});

// Image analizer section starts here

function check(headers) {
    return (buffers, options = {
        offset: 0
    }) => headers.every((header, index) => header === buffers[options.offset + index]);
}

function readBuffer(file, start = 0, end = 2) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => {
            resolve(reader.result);
        }
            ;
        reader.onerror = reject;
        reader.readAsArrayBuffer(file.slice(start, end));
    }
    );
}

const isPNG = check([0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a]);
const isJPEG = check([0xff, 0xd8, 0xff]);

async function handleChange(event) {

    var fileName = document.getElementById("ImageFileInputErrorValidationCustom");

    var selectedFile = event.target.files[0];
    var selectedFileName = selectedFile.name;
    var selectedFileExtension = selectedFileName.split('.')[selectedFileName.split('.').length - 1].toLowerCase();

    const pngImageBuffer = await readBuffer(selectedFile, 0, 8);
    const pngUint8Array = new Uint8Array(pngImageBuffer);
    const jpegImageBuffer = await readBuffer(selectedFile, 0, 3);
    const jpegUint8Array = new Uint8Array(jpegImageBuffer);

    // Check file first bytes to check if its a real png/jpeg image 
    if (!(isPNG(pngUint8Array) || isJPEG(jpegUint8Array))) {
        UnloadImage(true, `The type of selected file is not PNG or JPEG.`);
        return;
    }

    // Check for the file extension
    if (selectedFileExtension.length > 5) {
        UnloadImage(true, `Image extension is wrong. Please try again.`);
        return;
    }

    // Check file name length
    if (selectedFileName.length - selectedFileExtension.length > 30) {
        UnloadImage(true, `Image name can't be more then 30 characters long. Please, try again.`);
        return;
    }

    // Check file size
    var iConvert = (selectedFile.size / 1048576).toFixed(2); // size in mb
    if (iConvert > 30) {
        UnloadImage(true, `Image size can't be more then 30 MB. Please, try again.`);
        return;
    }

    var reader = new FileReader();

    var imgtag = document.getElementById("carImage");
    imgtag.title = selectedFileName;

    reader.onload = function (event) {

        const imageInputElement = document.getElementById("imageInputErrorValidationCustom");
        imageInputElement.value = event.target.result;

        const noImageElement = document.getElementById("noCarImage");
        noImageElement.style.display = "none";

        const uploadButtonElement = document.getElementById("unloadImageButton");
        uploadButtonElement.removeAttribute("disabled");

        imgtag.src = event.target.result;
    };

    reader.readAsDataURL(selectedFile);

    ShowSuccessImageUploadMessage(`Image was uploaded successfully.`)
}

function getEverythingBack() {
    var imgtag = document.getElementById("carImage");
    imgtag.title = "";
    const imageInputElement = document.getElementById("imageInputErrorValidationCustom");
    imageInputElement.value = "";
    const noImageElement = document.getElementById("noCarImage");
    noImageElement.style.display = "";
    const uploadButtonElement = document.getElementById("unloadImageButton");
    uploadButtonElement.setAttribute("disabled", "true");
    imgtag.src = "";
}

// Function to show message about error in image upload 
function ShowErrorImageUploadMessage(message) {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 5000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'error',
        title: message
    });
}

// Function to show message about error in image upload 
function ShowSuccessImageUploadMessage(message) {
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
        title: message
    });
}

// Image analizer section ends here



function UnloadImage(isError, message) {
    var form = document.getElementById("createNewVehicleForm");
    form.reset();

    var imgtag = document.getElementById("carImage");
    imgtag.title = "";

    const imageInputElement = document.getElementById("imageInputErrorValidationCustom");
    imageInputElement.value = "";

    const noImageElement = document.getElementById("noCarImage");
    noImageElement.removeAttribute("style");

    const uploadButtonElement = document.getElementById("unloadImageButton");
    uploadButtonElement.setAttribute("disabled", "");

    imgtag.removeAttribute("src");

    if (isError) {
        ShowErrorImageUploadMessage(message);
    } else {
        ShowSuccessImageUploadMessage(message);
    }
}

// Section for working with googleMaps starts here
var markersArray = [];

// Function for setting up a marker on the map
function setUpTheMarker() {

    let vehicleStreetAddress = document.getElementById('streetAddressErrorValidationCustom');
    let vehicleAptSuiteEtc = document.getElementById('aptSuiteEtcErrorValidationCustom');
    let vehileCountry = document.getElementById('countryErrorValidationCustom');
    let vehicleCity = document.getElementById('cityErrorValidationCustom');

    let areValidatedFields = true;

    if (!vehicleStreetAddress.checkValidity()) {
        vehicleStreetAddress.setAttribute("class", "form-control is-invalid");
        areValidatedFields = false;
    }
    if (!vehicleAptSuiteEtc.checkValidity()) {
        vehicleAptSuiteEtc.setAttribute("class", "form-control is-invalid");
        areValidatedFields = false;
    }
    if (!vehileCountry.checkValidity()) {
        vehileCountry.setAttribute("class", "form-control is-invalid");
        areValidatedFields = false;
    }
    if (!vehicleCity.checkValidity()) {
        vehicleCity.setAttribute("class", "form-control is-invalid");
        areValidatedFields = false;
    }

    if (!areValidatedFields) {
        return;
    }

    let vehicleAddress = vehicleStreetAddress.value.concat(" ", vehicleAptSuiteEtc.value);

    geocoder = new google.maps.Geocoder();

    geocoder.geocode({
        address: vehicleAddress
    }, (results, status) => {
        if (status == google.maps.GeocoderStatus.OK) {

            clearAllMarkers();
            var marker = new google.maps.Marker(
                {
                    position: new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng()),
                    map: map,
                    icon: {
                        url: "/Icons/vehicleIcon.png",
                        scaledSize: new google.maps.Size(40, 40)
                    }
                });

            markersArray.push(marker);

            document.getElementById('latitudeErrorValidationCustom').value = results[0].geometry.location.lat();
            document.getElementById('longitudeErrorValidationCustom').value = results[0].geometry.location.lng();

            map.setCenter({ lat: results[0].geometry.location.lat(), lng: results[0].geometry.location.lng() });


            // Save lat and long into the local storage to draw marker after page refreshes
            localStorage.setItem("marker-lat", results[0].geometry.location.lat());
            localStorage.setItem("marker-long", results[0].geometry.location.lng());

            vehicleStreetAddress.setAttribute("readonly", "readonly");
            vehicleStreetAddress.setAttribute("class", "form-control is-valid");
            vehicleAptSuiteEtc.setAttribute("readonly", "readonly");
            vehicleAptSuiteEtc.setAttribute("class", "form-control is-valid");
            vehicleCity.setAttribute("readonly", "readonly");
            vehicleCity.setAttribute("class", "form-control is-valid");
            vehileCountry.setAttribute("readonly", "readonly");
            vehileCountry.setAttribute("class", "form-control is-valid");

            document.getElementById("resetMarkerBtn").setAttribute("class", "btn btn-outline-warning mt-3");
            document.getElementById("searchVehicleBtn").setAttribute("class", "btn btn-outline-primary mt-3 visually-hidden");

        } else {
            alert('Address was not found. Response: ' + status);
        }
    });
}

function resetMarker() {
    let vehicleStreetAddress = document.getElementById('streetAddressErrorValidationCustom');
    let vehicleAptSuiteEtc = document.getElementById('aptSuiteEtcErrorValidationCustom');
    let vehileCountry = document.getElementById('countryErrorValidationCustom');
    let vehicleCity = document.getElementById('cityErrorValidationCustom');

    vehicleStreetAddress.removeAttribute("readonly");
    vehicleAptSuiteEtc.removeAttribute("readonly");
    vehileCountry.removeAttribute("readonly");
    vehicleCity.removeAttribute("readonly");

    document.getElementById("resetMarkerBtn").setAttribute("class", "btn btn-outline-warning mt-3 visually-hidden");
    document.getElementById("searchVehicleBtn").setAttribute("class", "btn btn-outline-primary mt-3");

    localStorage.removeItem("marker-lat");
    localStorage.removeItem("marker-long");

    document.getElementById('latitudeErrorValidationCustom').value = "";
    document.getElementById('longitudeErrorValidationCustom').value = "";

    clearAllMarkers();
}

// Function for deleting all markers from the array
function clearAllMarkers() {
    for (var i = 0; i < markersArray.length; i++) {
        markersArray[i].setMap(null);
    }
    markersArray.length = 0;
}

// Add event listener for setting up a marker if there is lat and long stored in local storage
window.addEventListener("load", () => {

    if (localStorage.hasOwnProperty("marker-lat") && localStorage.hasOwnProperty("marker-long")) {

        var latit = JSON.parse(localStorage.getItem("marker-lat"));
        var long = JSON.parse(localStorage.getItem("marker-long"));

        var marker = new google.maps.Marker({
            position: {
                lat: latit,
                lng: long
            },
            map: map,
            icon: {
                url: "/Icons/vehicleIcon.png",
                scaledSize: new google.maps.Size(40, 40)
            }
        });

        document.getElementById('latitudeErrorValidationCustom').value = latit;
        document.getElementById('longitudeErrorValidationCustom').value = long;
    }
});
// Section for working with googleMaps ends here

(() => {
    'use strict'

    const forms = document.querySelectorAll('.needs-validation')

    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
            }

            form.classList.add('was-validated')
        }, false)
    })
})()

document.addEventListener("DOMContentLoaded", () => {
    const serverErrorSpans = document.getElementById('serverErrors').querySelectorAll("span");

    Array.from(serverErrorSpans).forEach(errorSpan => {
        if (errorSpan.textContent !== "") {
            document.getElementById(errorSpan.id + "Validation").textContent = errorSpan.textContent;
            document.getElementById(errorSpan.id + "ValidationCustom").setAttribute("class", "form-control is-invalid")
        }
    });
});

function SetInputAsRequireValidation(inputElement) {
    inputElement.setAttribute("class", "form-control");
}

function CheckFormForInvalidFields() {
    const forms = document.getElementById("submitForm").querySelectorAll('input');

    Array.from(forms).forEach(form => {
        if (form.getAttribute("class") !== null) {
            if (form.getAttribute("class").includes("is-invalid")) {
                form.setAttribute("class", "form-control");
            }
        }
    });
}

const vehicleCategories = [];

function FilterCategories(checkboxElement, category) {
    if (checkboxElement.checked) {
        vehicleCategories.push(category);
    } else {
        var index = vehicleCategories.indexOf(category);
        vehicleCategories.splice(index, 1);
    }
}