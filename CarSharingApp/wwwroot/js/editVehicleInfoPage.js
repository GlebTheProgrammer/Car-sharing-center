// Function for showing the alert when user clicks on Cancel Changes button
function ShowCancelChangesConfirmationAlert() {
    Swal.fire({
        title: 'Are you sure?',
        text: "All new information will be removed!",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#4FA64F',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.reload(true);
        }
    });
}

// Function for checking textarea input to prevent multiple enters
function CheckTextAreaInput(e) {
    if (e.keyCode == 13 && !e.shiftKey) {
        e.preventDefault();
    }
}

// Function for showing input fields and hiding text content
function ShowInputField(target) {

    var text = document.getElementById(target + "Text");
    var changeBtn = document.getElementById("Change" + target + "Btn");
    var input = document.getElementById(target + "Input");
    var saveBtn = document.getElementById("Save" + target + "Btn");

    text.setAttribute("class", "visually-hidden");
    changeBtn.setAttribute("class", "visually-hidden");

    input.setAttribute("class", "form-control");
    input.value = text.textContent;
    saveBtn.setAttribute("class", "btn btn-outline-success");
}

// Function for hiding input fields and showing text content
function HideInputField(target) {

    var text = document.getElementById(target + "Text");
    var changeBtn = document.getElementById("Change" + target + "Btn");
    var input = document.getElementById(target + "Input");
    var saveBtn = document.getElementById("Save" + target + "Btn");

    text.textContent = input.value;
    SetModelValue(target);

    input.setAttribute("class", "visually-hidden");
    saveBtn.setAttribute("class", "visually-hidden");

    text.setAttribute("class", "text-muted mb-0");
    changeBtn.setAttribute("class", "btn btn-outline-warning");
}

// Function for refreshing form model property input value
function SetModelValue(valueName) {

    var input = document.getElementById(valueName + "Input");
    var modelAttribute = document.getElementById("Model" + valueName);

    modelAttribute.value = input.value;
}

// Function for hiding error span messages
function HideErrorSpan(componentId) {
    var component = document.getElementById(componentId);
    component.setAttribute("class", "visually-hidden");
}

// Section for working with googleMaps starts here
var markersArray = [];

function setUpTheMarker() {
    var address = document.getElementById('AddressInput').value;

    if (address === "") {
        var addressErrorSpan = document.getElementById("AddressError");
        addressErrorSpan.textContent = "Address can't be empty";
        addressErrorSpan.setAttribute("class", "text-danger text-center d-flex justify-content-center");
        return;
    }

    geocoder = new google.maps.Geocoder();

    geocoder.geocode({
        address: address
    }, (results, status) => {
        if (status == google.maps.GeocoderStatus.OK) {

            clearAllMarkers();
            var marker = new google.maps.Marker(
                {
                    position: new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng()),
                    map: map
                });

            markersArray.push(marker);

            document.getElementById('ModelLocationLatitude').value = results[0].geometry.location.lat();
            document.getElementById('ModelLocationLongitude').value = results[0].geometry.location.lng();

            map.setCenter({ lat: results[0].geometry.location.lat(), lng: results[0].geometry.location.lng() });

            HideAddressInputField("Address");

        } else {
            alert('Address was not found. Response: ' + status);
        }
    });

}

function CancelMapChanges(target) {
    clearAllMarkers();

    var latitude = document.getElementById("ModelLocationLatitude").value;
    var longitude = document.getElementById("ModelLocationLongitude").value;

    var latSerialized = JSON.parse(latitude);
    var longSerialized = JSON.parse(longitude);

    var marker = new google.maps.Marker(
        {
            position: new google.maps.LatLng(latSerialized, longSerialized),
            map: map
        });

    markersArray.push(marker);

    map.setCenter({ lat: latSerialized, lng: longSerialized });

    var changeBtn = document.getElementById("Change" + target + "Btn");
    var input = document.getElementById(target + "Input");
    var cancelChangesBtn = document.getElementById("Cancel" + target + "ChangesBtn");
    var searchAddressBtn = document.getElementById("Search" + target + "Btn");

    input.setAttribute("class", "visually-hidden");
    cancelChangesBtn.setAttribute("class", "visually-hidden");
    searchAddressBtn.setAttribute("class", "visually-hidden");
    changeBtn.setAttribute("class", "btn btn-outline-warning");
}

function clearAllMarkers() {
    for (var i = 0; i < markersArray.length; i++) {
        markersArray[i].setMap(null);
    }
    markersArray.length = 0;
}

function ShowAddressInputField(target) {

    clearAllMarkers();
    var text = document.getElementById(target + "Text");
    var changeBtn = document.getElementById("Change" + target + "Btn");
    var input = document.getElementById(target + "Input");
    var cancelChangesBtn = document.getElementById("Cancel" + target + "ChangesBtn");
    var searchAddressBtn = document.getElementById("Search" + target + "Btn");

    changeBtn.setAttribute("class", "visually-hidden");

    input.setAttribute("class", "form-control");
    input.value = text.textContent;
    cancelChangesBtn.setAttribute("class", "btn btn-outline-danger");
    searchAddressBtn.setAttribute("class", "btn btn-outline-primary ml-2");
}

function HideAddressInputField(target) {

    var text = document.getElementById(target + "Text");
    var changeBtn = document.getElementById("Change" + target + "Btn");
    var input = document.getElementById(target + "Input");
    var cancelChangesBtn = document.getElementById("Cancel" + target + "ChangesBtn");
    var searchAddressBtn = document.getElementById("Search" + target + "Btn");

    text.textContent = input.value;
    SetModelValue(target);

    input.setAttribute("class", "visually-hidden");
    cancelChangesBtn.setAttribute("class", "visually-hidden");
    searchAddressBtn.setAttribute("class", "visually-hidden");

    changeBtn.setAttribute("class", "btn btn-outline-warning");
}
// Section for working with googleMaps ends here