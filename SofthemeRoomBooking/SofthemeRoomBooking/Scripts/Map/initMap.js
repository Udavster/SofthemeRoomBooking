function initMap() {
    var url = '../../Content/Images/Map/';
    var point = {lat: 50.42894726, lng: 30.51822513};
    var map = new google.maps.Map(document.getElementById('map'), {
        center: point,
        zoom: 17,
        scrollwheel: false
    });

    var contentString = '<div id="content">'+
        '<div class="map__name">Softheme</div>'+
        '<div class="map__adress">г. Киев, ул. Деловая, 5а</div>'+
        '<div class="map__web"><span class="map__sitelogo">softheme.com</span></div>'+
        '<div class="map__web"><span class="map__emaillogo">info@softheme.com</span></div>'+
        '</div>';

    var infowindow = new google.maps.InfoWindow({
        content: contentString
    });

    var marker = new google.maps.Marker({
        map: map,
        position: point,
        icon : url+'icon-map.png',
        title: 'Softheme'
    });

    marker.addListener('click', function() {
        infowindow.open(map, marker);
    });
}
