﻿
function xy_to_lon(x, y) {
    var lon = 0.6563 * x + 15.962;
    if (isEven(y)) {
        lon = lon - 0.6563 / 2.0;
    }
    return lon;
}
function xy_to_lat(x, y) {
    var lat = -0.5935 * y - 15.446;
    return lat;
}
function latlon_to_x(lat, lon) {
    if (isEven(latlon_to_y(lat, lon))) {
        lon = lon + 0.6563 / 2.0;
    }
    var x = (lon - 15.962) / 0.6563;
    return x;
}
function latlon_to_y(lat, lon) {
    var y = (lat + 15.446) / (-0.5935);
    return y;
}

/***  little hack starts here ***/
L.Map = L.Map.extend({
    openPopup: function (popup) {
        //        this.closePopup();  // just comment this
        this._popup = popup;

        return this.addLayer(popup).fire('popupopen', {
            popup: this._popup
        });
    }
}); /***  end of hack ***/


var map = L.map('map', {
    crs: L.CRS.Simple,
    center: [xy_to_lat(model_center_x, model_center_y), xy_to_lon(model_center_x, model_center_y)],
    zoom: model_center_zoom,
});

var topo = L.tileLayer('https://web196.secure-secure.co.uk/baloogancampaign.com/render3/{z}/{x}/{y}.png', {
    attribution: "<a href='http://baloogan.com'>Written by Baloogan</a> (<a href='https://twitter.com/baloogancamp'>@BalooganCamp</a>) leafletjs " + model_side + " " + model_date + ' Map:WitP-AE Topo Map Project',
    continuousWorld: true,
    id: 'map-render3',
    maxZoom: 6,
    minZoom: 3,
    noWrap: true,
});
var yamato = L.tileLayer('https://web196.secure-secure.co.uk/baloogancampaign.com/render4/{z}/{x}/{y}.png', {
    attribution: "<a href='http://baloogan.com'>Written by Baloogan</a> (<a href='https://twitter.com/baloogancamp'>@BalooganCamp</a>) leafletjs " + model_side + " " + model_date + ' Map:Yamato Damashii',
    continuousWorld: true,
    id: 'map-render4',
    maxZoom: 6,
    minZoom: 3,
    noWrap: true,
}).addTo(map);
map.zoomControl.setPosition('topright');
map.setMaxBounds([[-150, 0], [0, 190]]);
L.Circle = L.Circle.extend({
    projectLatlngs: function () {
        //this._point = this._map.latLngToLayerPoint(this._latlng);
        //this._radius = this._mRadius;
        var lngRadius = this._mRadius,
            latlng2 = new L.LatLng(this._latlng.lat, this._latlng.lng - lngRadius),
            point2 = this._map.latLngToLayerPoint(latlng2);

        this._point = this._map.latLngToLayerPoint(this._latlng);
        this._radius = Math.max(Math.round(this._point.x - point2.x), 1);
    },

    getBounds: function () {
        var radius = this._mRadius,
            latlng = this._latlng,
            sw = new L.LatLng(latlng.lat - radius, latlng.lng - radius),
            ne = new L.LatLng(latlng.lat + radius, latlng.lng + radius);

        return new L.LatLngBounds(sw, ne);
    }
});



var airbaseIcon_a = L.icon({
    iconUrl: '/Content/map_icons/AB' + '' + '.png',
});
var navalbaseIcon_a = L.icon({
    iconUrl: '/Content/map_icons/NB' + '' + '.png',
});
var taskforceIcon_a = L.icon({
    iconUrl: '/Content/map_icons/TF' + '' + '.png',
});
var lcuIcon_a = L.icon({
    iconUrl: '/Content/map_icons/LCU' + '' + '.png',
});

var airbaseIcon_j = L.icon({
    iconUrl: '/Content/map_icons/AB' + 'j' + '.png',
});
var navalbaseIcon_j = L.icon({
    iconUrl: '/Content/map_icons/NB' + 'j' + '.png',
});
var taskforceIcon_j = L.icon({
    iconUrl: '/Content/map_icons/TF' + 'j' + '.png',
});
var lcuIcon_j = L.icon({
    iconUrl: '/Content/map_icons/LCU' + 'j' + '.png',
});

var baseIcon_a = L.icon({
    iconUrl: '/Content/map_icons/USBase.png',
});
var baseIcon_j = L.icon({
    iconUrl: '/Content/map_icons/JABase.png',
});
var reportIcon = L.icon({
    iconUrl: '/Content/map_icons/Report.png',
});




function final() {

    var baseMaps = {
        "WitP-AE Topo Map Project": topo,
        "Yamato Damashii": yamato
    };

    var overlayMaps = {
        "Insignificant 'Dot' Bases": L.layerGroup(group_dot_bases)
    };
    L.control.layers(baseMaps, overlayMaps).addTo(map);
}