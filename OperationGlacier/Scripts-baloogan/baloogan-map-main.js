
var group_dot_bases = [];

function load_hexes(turn) {

    var hexes = turn.hexes;


    $.each(hexes, function (i, hex) {

        var x = hex.x;
        var y = hex.y;
        var lon = xy_to_lon(x, y);
        var lat = xy_to_lat(x, y);
        //var report = item.html;

        var base = "";
        var base_tooltip = "";


        var airbase = "";
        var airbase_tooltip = "";


        var navalbase = "";
        var navalbase_tooltip = "";

        //var reports = "";
        var reports_sigint = "";
        var reports_combatevent = "";
        var reports_afteraction = [];
        var reports_operationalreport = "";
        var reports_tooltip = "";


        var taskforces = "";
        var taskforces_tooltip = "";

        var lcus = "";
        var lcus_tooltip = "";

        var baseIcon;
        var airbaseIcon;
        var navalbaseIcon;
        var taskforceIcon;
        var lcusIcon;

        var ship_total = 0;

        var is_dot_base = false;

        var popupOptions = { minWidth: 400, maxWidth: 600, maxHeight: 400, };
        $.each(hex.units, function (i, unit) {
            //populate htmls via that pretentious lib

            if (unit.type_str == "AirGroup") {
                if (unit.owner == 0) {
                    airbaseIcon = "Japan";
                    unit.owner = "Japan";
                }
                if (unit.owner == 1) {
                    airbaseIcon = "Allies";
                    unit.owner = "Allies";
                }
                unit = AirGroupUnit(unit);
                airbase += airgroup_template(unit);
                airbase_tooltip = airbase_tooltip + unit.name + "\n";
            } else if (unit.type_str == "Base") {
                if (unit.owner == 0) {
                    baseIcon = "Japan";
                    unit.owner = "Japan";
                }
                if (unit.owner == 1) {
                    baseIcon = "Allies";
                    unit.owner = "Allies";
                }
                unit = BaseUnit(unit);
                base += base_template(unit);
                base_tooltip = base_tooltip + unit.name;
                if (unit.row.Port.split(' ')[0] == "0" && unit.row.Airbase.split(' ')[0] == "0") {
                    is_dot_base = true;
                } else {
                    is_dot_base = false;
                }
            } else if (unit.type_str == "TaskForce") {
                if (unit.owner == 0) {
                    taskforceIcon = "Japan";
                    unit.owner = "Japan";
                }
                if (unit.owner == 1) {
                    taskforceIcon = "Allies";
                    unit.owner = "Allies";
                }
                unit = TaskForceUnit(unit);
                taskforces += taskforce_template(unit);
                taskforces += "<div style='margin-left:20px'>";
                $.each(unit.subunits, function (i, subunit) {
                    taskforces += ship_template(subunit);
                    taskforces += "<div style='margin-left:20px'>";
                    $.each(subunit.subunits, function (i, subsubunit) {
                        subsubunit = AirGroupUnit(subsubunit);
                        taskforces += airgroup_template(subsubunit);
                    });
                    taskforces += "</div>";
                });
                taskforces += "</div>";
                taskforces_tooltip = taskforces_tooltip + unit.name + "\n";
            } else if (unit.type_str == "Ship") {
                ship_total += 1;
                if (unit.owner == 0) {
                    navalbaseIcon = "Japan";
                    unit.owner = "Japan";
                }
                if (unit.owner == 1) {
                    navalbaseIcon = "Allies";
                    unit.owner = "Allies";
                }
                //ships in TFs are in TFs, these are ships at ahcnor
                navalbase += ship_template(unit);
                navalbase += "<div style='margin-left:20px'>";
                $.each(unit.subunits, function (i, subunit) {
                    subunit = AirGroupUnit(subunit);
                    navalbase += airgroup_template(subunit);
                });
                navalbase += "</div>";
                navalbase_tooltip = navalbase_tooltip + unit.name + "\n";
            } else if (unit.type_str == "LCU") {
                if (unit.owner == 0) {
                    lcuIcon = "Japan";
                    unit.owner = "Japan";
                }
                if (unit.owner == 1) {
                    lcuIcon = "Allies";
                    unit.owner = "Allies";
                }
                lcus += lcu_template(unit);
                lcus_tooltip = lcus_tooltip + unit.name + "\n";
            } else if (unit.type_str == "SigInt") {
                reports_sigint += sigint_template(unit);
                reports_tooltip = reports_tooltip + unit.report + "\n";
            } else if (unit.type_str == "CombatEvent") {
                reports_combatevent += combatevent_template(unit);
                reports_tooltip = reports_tooltip + unit.report + "\n";
            } else if (unit.type_str == "AfterAction") {
                unit.report = unit.report.trim();
                reports_afteraction.push({ text: afteraction_template(unit), first_line: unit.report.split('\n')[0] });
                reports_tooltip = reports_tooltip + unit.report.split('\n')[0] + "\n";
            } else if (unit.type_str == "OperationalReport") {
                reports_operationalreport += operationalreport_template(unit);
                reports_tooltip = reports_tooltip + unit.report + "\n";
            }
        });

        

        var nudge_y = 0.0;
        if (model_side == "Both") {
            if (turn.side_string == "Allies") {
                nudge_y = 0.07;
            } else if (turn.side_string == "Japan") {
                nudge_y = -0.07;
            }
        }
        if (model_side != "Both" || (baseIcon == "Japan" && turn.side_string == "Japan") || (baseIcon == "Allies" && turn.side_string == "Allies")) {
            if (base != "") {
                var pos = [lat + 0.09, lon - 0.09]; //intentionall no nudge_y
                if (is_dot_base) {
                    if (baseIcon == "Japan") {
                        group_dot_bases.push(L.marker(pos, { icon: baseIcon_j, title: base_tooltip })
                                .bindPopup(base, popupOptions));
                    } else {
                        group_dot_bases.push(L.marker(pos, { icon: baseIcon_a, title: base_tooltip })
                            .bindPopup(base, popupOptions));
                    }
                } else {
                    if (baseIcon == "Japan") {
                        L.marker(pos, { icon: baseIcon_j, title: base_tooltip })
                            .addTo(map)
                            .bindPopup(base, popupOptions);
                    } else {
                        L.marker(pos, { icon: baseIcon_a, title: base_tooltip })
                            .addTo(map)
                            .bindPopup(base, popupOptions);
                    }
                }
            }
        }
        if (lcus != "") {
            var pos = [lat - 0.12 + nudge_y, lon - 0.23];
            if (lcuIcon == "Japan") {
                L.marker(pos, { icon: lcuIcon_j, title: lcus_tooltip })
                    .addTo(map)
                    .bindPopup(lcus, popupOptions);
            } else {
                L.marker(pos, { icon: lcuIcon_a, title: lcus_tooltip })
                    .addTo(map)
                    .bindPopup(lcus, popupOptions);
            }

        }
        if (airbase != "") {
            var pos = [lat + 0.21, lon + 0.13];
            if (airbaseIcon == "Japan") {
                L.marker(pos, { icon: airbaseIcon_j, title: airbase_tooltip })
                    .addTo(map)
                    .bindPopup(airbase, popupOptions);
            } else {
                L.marker(pos, { icon: airbaseIcon_a, title: airbase_tooltip })
                    .addTo(map)
                    .bindPopup(airbase, popupOptions);
            }

        }

        if (taskforces != "") {
            var pos;
            if (base == "") {
                pos = [lat + 0.1 + nudge_y, lon - 0.1];
            } else {
                pos = [lat - 0.10 + nudge_y, lon + 0.06];
            }
            if (taskforceIcon == "Japan") {
                L.marker(pos, { icon: taskforceIcon_j, title: taskforces_tooltip })
                    .addTo(map)
                    .bindPopup(taskforces, popupOptions);
            } else {
                L.marker(pos, { icon: taskforceIcon_a, title: taskforces_tooltip })
                    .addTo(map)
                    .bindPopup(taskforces, popupOptions);
            }
        }

        if (navalbase != "") {
            var pos = [lat + 0.22, lon - 0.25];
            if (navalbaseIcon == "Japan") {
                L.marker(pos, { icon: navalbaseIcon_j, title: navalbase_tooltip })
                    .addTo(map)
                    .bindPopup(navalbase, popupOptions);
            } else {
                L.marker(pos, { icon: navalbaseIcon_a, title: navalbase_tooltip })
                    .addTo(map)
                    .bindPopup(navalbase, popupOptions);
            }
        }
        if (reports_sigint != "" || reports_combatevent != "" || reports_afteraction != "" || reports_operationalreport != "") {
            reports = "";
            var accordion = randomString(8);
            reports += "<div class='panel-group' id='" + accordion + "' style='max-height:400px; overflow-y:scroll;'>";
            var accordion_first = " in ";
            if (reports_sigint != "") {
                var collapse_sigint = randomString(8);
                reports += "<div class='panel panel-default'>";
                reports += "<div class='panel-heading'>";
                reports += "<h4 class='panel-title'><a data-toggle='collapse' data-parent='#" + accordion + "' href='#" + collapse_sigint + "'>Signals Intelligence</a></h4>";
                reports += "</div>";
                reports += "<div id=" + collapse_sigint + " class='panel-collapse collapse " + accordion_first + "'>";
                reports += "<div class='panel-body'>";
                reports += reports_sigint;
                reports += "</div>";
                reports += "</div>";
                reports += "</div>";
                accordion_first = "";
            }
            if (reports_combatevent != "") {
                var collapse_combatevent = randomString(8);
                reports += "<div class='panel panel-default'>";
                reports += "<div class='panel-heading'>";
                reports += "<h4 class='panel-title'><a data-toggle='collapse' data-parent='#" + accordion + "' href='#" + collapse_combatevent + "'>Combat Events</a></h4>";
                reports += "</div>";
                reports += "<div id=" + collapse_combatevent + " class='panel-collapse collapse " + accordion_first + "'>";
                reports += "<div class='panel-body'>";
                reports += reports_combatevent;
                reports += "</div>";
                reports += "</div>";
                reports += "</div>";
                accordion_first = "";
            }
            $.each(reports_afteraction, function (i, report_aar) {
                var collapse_afteraction = randomString(8);
                reports += "<div class='panel panel-default'>";
                reports += "<div class='panel-heading'>";
                reports += "<h4 class='panel-title'><a data-toggle='collapse' data-parent='#" + accordion + "' href='#" + collapse_afteraction + "'>" + report_aar.first_line + "</a></h4>";
                reports += "</div>";
                reports += "<div id=" + collapse_afteraction + " class='panel-collapse collapse " + accordion_first + "'>";
                reports += "<div class='panel-body'>";
                reports += report_aar.text;
                reports += "</div>";
                reports += "</div>";
                reports += "</div>";
                accordion_first = "";
            });

            if (reports_operationalreport != "") {
                var collapse_operationalreport = randomString(8);
                reports += "<div class='panel panel-default'>";
                reports += "<div class='panel-heading'>";
                reports += "<h4 class='panel-title'><a data-toggle='collapse' data-parent='#" + accordion + "' href='#" + collapse_operationalreport + "'>Operational Reports</a></h4>";
                reports += "</div>";
                reports += "<div id=" + collapse_operationalreport + " class='panel-collapse collapse " + accordion_first + "'>";
                reports += "<div class='panel-body'>";
                reports += reports_operationalreport;
                reports += "</div>";
                reports += "</div>";
                reports += "</div>";
                accordion_first = "";
            }

            reports += "</div>";
            L.marker([lat + 0.35 + nudge_y, lon - 0.08], { icon: reportIcon, title: reports_tooltip })
                .addTo(map)
                .bindPopup(reports, { minWidth: 800, maxWidth: 800, maxHeight: 400, minHeight: 400 });
        }

    });
}


var comments_popupOptions = { maxWidth: 600, maxHeight: 400, };


var map_bounds = map.getBounds();
$.each(comments_json, function (i, comment_group) {
    var comment_html = "";
    var comment_x;
    var comment_y;
    $.each(comment_group, function (i, comment) {
        comment_x = comment.x;
        comment_y = comment.y;
        comment.message = marked(comment.message);
        comment_html += comment_template(comment);
    });
    var circ_lat = xy_to_lat(comment_x, comment_y);
    var circ_lon = xy_to_lon(comment_x, comment_y);
    var circ = L.circle([circ_lat, circ_lon], 0.4, { color: 'red' })
        .addTo(map)
        .bindPopup(comment_html, comments_popupOptions);
    //only open things on screen
    if (map_bounds.contains([circ_lat, circ_lon])) {
        circ.openPopup();
    }

});



(function () {

    var speed_fac = 3.0 / 16.0;
    var initial = 2000;
    if (model_side == "Both") {
        var link1 = '/OperationGlacierGameData/' + game_name + '/Turns/' + 'Allies' + '/' + model_date + '/Turn';
        setTimeout(function () { $.getJSON(link1 + '0.json').done(load_hexes); }, 0000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '1.json').done(load_hexes); }, 1000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '2.json').done(load_hexes); }, 2000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '3.json').done(load_hexes); }, 3000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '4.json').done(load_hexes); }, 4000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '5.json').done(load_hexes); }, 5000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '6.json').done(load_hexes); }, 6000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '7.json').done(load_hexes); }, 7000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '8.json').done(load_hexes); }, 8000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '9.json').done(load_hexes); }, 9000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '10.json').done(load_hexes); }, 10000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '11.json').done(load_hexes); }, 11000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '12.json').done(load_hexes); }, 12000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '13.json').done(load_hexes); }, 13000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '14.json').done(load_hexes); }, 14000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link1 + '15.json').done(load_hexes); }, 15000 * speed_fac + initial);

        var link2 = '/OperationGlacierGameData/' + game_name + '/Turns/' + 'Japan' + '/' + model_date + '/Turn';
        setTimeout(function () { $.getJSON(link2 + '0.json').done(load_hexes); }, 0000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '1.json').done(load_hexes); }, 1000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '2.json').done(load_hexes); }, 2000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '3.json').done(load_hexes); }, 3000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '4.json').done(load_hexes); }, 4000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '5.json').done(load_hexes); }, 5000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '6.json').done(load_hexes); }, 6000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '7.json').done(load_hexes); }, 7000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '8.json').done(load_hexes); }, 8000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '9.json').done(load_hexes); }, 9000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '10.json').done(load_hexes); }, 10000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '11.json').done(load_hexes); }, 11000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '12.json').done(load_hexes); }, 12000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '13.json').done(load_hexes); }, 13000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '14.json').done(load_hexes); }, 14000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link2 + '15.json').done(load_hexes).done(final); }, 15000 * speed_fac + initial);
    } else {
        var link = '/OperationGlacierGameData/' + game_name + '/Turns/' + model_side + '/' + model_date + '/Turn';
        setTimeout(function () { $.getJSON(link + '0.json').done(load_hexes); }, 0000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '1.json').done(load_hexes); }, 1000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '2.json').done(load_hexes); }, 2000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '3.json').done(load_hexes); }, 3000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '4.json').done(load_hexes); }, 4000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '5.json').done(load_hexes); }, 5000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '6.json').done(load_hexes); }, 6000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '7.json').done(load_hexes); }, 7000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '8.json').done(load_hexes); }, 8000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '9.json').done(load_hexes); }, 9000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '10.json').done(load_hexes); }, 10000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '11.json').done(load_hexes); }, 11000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '12.json').done(load_hexes); }, 12000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '13.json').done(load_hexes); }, 13000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '14.json').done(load_hexes); }, 14000 * speed_fac + initial);
        setTimeout(function () { $.getJSON(link + '15.json').done(load_hexes).done(final); }, 15000 * speed_fac + initial);
    }

}).call(this);