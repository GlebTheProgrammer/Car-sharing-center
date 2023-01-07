var map;
function initMap(vehicles) {
    var data = JSON.parse('@Html.Raw(Model)');
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 53.90588468960213, lng: 27.555191727187893 },
        zoom: 11
    });

    var model = '@Html.Raw(Json.Serialize(Model/*.VehiclesLocation*/))';
    var data = JSON.parse(model);

    var vehiclesData = JSON.parse('@Html.Raw(Json.Serialize(Model/*.Vehicles*/))');

    const infos = [];

    for (let i = 0; i < data.length; i++) {
        var contentString = '<div>' +
            '<h5 class="card-title">' + vehiclesData[i]['name'] + '</h5>' +
            '<img src="/vehicleImages/' + vehiclesData[i]['image'] + '" style="max-width: 150px;">' +
            '</div>';

        infos.push(contentString);
    }

    var infoWindow = new google.maps.InfoWindow({
        disableAutoPan: true
    });

    for (let i = 0; i < data.length; i++) {
        const marker = new google.maps.Marker(
            {
                position: new google.maps.LatLng(data[i][0], data[i][1]),
                map: map,
                title: vehiclesData[i]['name'],
                icon: {
                    url: "/Icons/vehicleIcon.png",
                    scaledSize: new google.maps.Size(40, 40)
                },
                animation: google.maps.Animation.DROP
            });

        google.maps.event.addListener(marker, 'mouseover', function () {
            infoWindow.setContent(infos[i]),
                infoWindow.open(map, marker)
        });
        google.maps.event.addListener(marker, 'mouseout', function () {
            infoWindow.close()
        });

    }
}