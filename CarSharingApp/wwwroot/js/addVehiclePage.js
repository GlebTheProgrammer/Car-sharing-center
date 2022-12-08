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

// Function for hiding span error messages
function HideErrorSpan(componentId) {
    var component = document.getElementById(componentId);
    component.textContent = "";
}

// Function for selecting user image and displaying it
function onFileSelected(event) {
    var file = document.getElementById("ImageFileInput");
    if (file.files.length == 0) {
        return;
    }

    var selectedFile = event.target.files[0];
    var reader = new FileReader();

    var imgtag = document.getElementById("carImage");
    imgtag.title = selectedFile.name;

    reader.onload = function (event) {

        const imageInputElement = document.getElementById("imageInput");
        imageInputElement.value = event.target.result;

        const noImageElement = document.getElementById("noCarImage");
        noImageElement.style.display = "none";

        imgtag.src = event.target.result;
    };

    reader.readAsDataURL(selectedFile);
}

// Section for working with googleMaps starts here
var markersArray = [];

// Function for setting up a marker on the map
function setUpTheMarker() {

    var address = document.getElementById('carAddress').value;

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

            document.getElementById('latit').value = results[0].geometry.location.lat();
            document.getElementById('longit').value = results[0].geometry.location.lng();

            map.setCenter({ lat: results[0].geometry.location.lat(), lng: results[0].geometry.location.lng() });


            // Save lat and long into the local storage to draw marker after page refreshes
            localStorage.setItem("marker-lat", results[0].geometry.location.lat());
            localStorage.setItem("marker-long", results[0].geometry.location.lng());

        } else {
            alert('Address was not found. Response: ' + status);
        }
    });

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
            map: map
        });

        document.getElementById('latit').value = latit;
        document.getElementById('longit').value = long;
    }
});
// Section for working with googleMaps ends here