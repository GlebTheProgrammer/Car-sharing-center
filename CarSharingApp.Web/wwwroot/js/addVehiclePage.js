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

// Function for selecting user image and displaying it
function onFileSelected(event) {
    var file = document.getElementById("ImageFileInputErrorValidationCustom");
    if (file.files.length == 0) {
        return;
    }

    var selectedFile = event.target.files[0];
    var reader = new FileReader();

    var imgtag = document.getElementById("carImage");
    imgtag.title = selectedFile.name;

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
}

function UnloadImage() {
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

        document.getElementById('latit').value = latit;
        document.getElementById('longit').value = long;
    }
});
// Section for working with googleMaps ends here