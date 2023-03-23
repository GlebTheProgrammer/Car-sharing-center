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
function HideInputField(target, form) {
    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
    } else {
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

        event.preventDefault();
    }
    form.classList.add('was-validated');
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
    var streetAddress = document.getElementById('StreetAddressInput').value;
    var aptSuiteEtc = document.getElementById('AptSuiteEtcInput').value;

    geocoder = new google.maps.Geocoder();

    geocoder.geocode({
        address: streetAddress + " " + aptSuiteEtc
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

            document.getElementById('ModelLocationLatitude').value = results[0].geometry.location.lat();
            document.getElementById('ModelLocationLongitude').value = results[0].geometry.location.lng();

            map.setCenter({ lat: results[0].geometry.location.lat(), lng: results[0].geometry.location.lng() });

            HideLocationInputs();

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
            map: map,
            icon: {
                url: "/Icons/vehicleIcon.png",
                scaledSize: new google.maps.Size(40, 40)
            }
        });

    markersArray.push(marker);

    map.setCenter({ lat: latSerialized, lng: longSerialized });

    CancelAndHideInput("StreetAddress");
    CancelAndHideInput("AptSuiteEtc");
    CancelAndHideInput("City");
    CancelAndHideInput("Country");

    var changeBtn = document.getElementById("ChangeAddressBtn");
    var cancelChangesBtn = document.getElementById("CancelAddressChangesBtn");
    var searchAddressBtn = document.getElementById("SearchAddressBtn");

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
    var text = document.getElementById(target + "Text");
    var input = document.getElementById(target + "Input");
    var section = document.getElementById(target + "Section");

    section.setAttribute("class", "col-6");
    input.value = text.textContent;
}

function HideAddressInputField(target) {

    var text = document.getElementById(target + "Text");
    var input = document.getElementById(target + "Input");
    var section = document.getElementById(target + "Section");

    text.textContent = input.value;

    SetModelValue(target);

    section.setAttribute("class", "col-6 visually-hidden");
}
// Section for working with googleMaps ends here