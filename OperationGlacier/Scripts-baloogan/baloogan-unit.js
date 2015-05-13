function render_unit_data_panel(i, unit) {
    var html = "";
    if (i != -1) {
        html += "<div class='col-sm-6'>";
    } else {
        html += "<div class='col-sm-6'>";
    }
    html += "<div class='panel panel-default'><div class='panel-body'>";

    if (unit.owner == 0) {
        unit.owner = "Japan";
    }
    if (unit.owner == 1) {
        unit.owner = "Allies";
    }
    if (unit.type_str == "AirGroup") {
        unit = AirGroupUnit(unit);
        html += airgroup_template(unit);
    } else if (unit.type_str == "Base") {
        unit = BaseUnit(unit);
        html += base_template(unit);
    } else if (unit.type_str == "Ship") {
        html += ship_template(unit);
        html += "<div style='margin-left:20px'>";
        $.each(unit.subunits, function (i, subunit) {

            subunit = AirGroupUnit(subunit);
            html += airgroup_template(subunit);
        });
        html += "</div>";
    } else if (unit.type_str == "LCU") {
        html += lcu_template(unit);
    }
    html += "</div>";
    html += "</div>";
    html += "</div>";
    return html;
}



function render_comments(timeline, current_unit) {
    var html = "";

    html += "<div class='col-sm-6'>";

    html += "<div class='panel panel-default'>";
    html += "<div class='panel-heading'>";
    html += "<div style='float:right;'><a href='/OperationGlacier/Comments/Create?"
        + "date_string=" + current_unit.date_string
        + "&unit_timeline_id=" + current_unit.timeline_id
        + "&unit_location=" + current_unit.location
        + "&x=" + current_unit.x
        + "&y=" + current_unit.y
        + "&unit_name=" + current_unit.name
        + "&game_name=" + model_game_name
        + "'>Add Comment</a></div>";
    html += "<h2>Comments</h2>";
    html += "</div>";

    html += "<div class='panel-body'>";

    

    $.each(comments_json, function (i, comment) { comment.message = marked(comment.message); html += comment_template(comment); });

    html += "</div>";
    html += "</div>";
    html += "</div>";
    return html;
}

function render_class(timeline, current_unit) {
    html = "";
    html += "<div class='col-sm-6'>";
    html += "<div class='panel panel-default'>";
    if (current_unit.type_str == "AirGroup") {

        html += "<div class='panel-heading'><h2>" + timeline.scendata_air["Name"] + "</h2></div>";

        html += "<div class='panel-body'>";
        html += aircraft_class_template(timeline.scendata_air);


        var vals = ["", "2", "3", "4", "5", "6", "7", "8", "9", "10", ];
        html += "<strong>Armament (Normal Range):</strong>";
        html += "<ul>";

        $.each(vals, function (i, val) {
            var Wpn = [];
            Wpn.DevID = timeline.scendata_air["WpnDevID" + val];
            Wpn.Ammo = timeline.scendata_air["WpnAmmo" + val];
            Wpn.Facing = timeline.scendata_air["WpnFacing" + val];
            Wpn.Number = timeline.scendata_air["WpnNumber" + val];
            if (Wpn.DevID == 0) {
                return true;//continue;
            }
            if (Wpn.Facing == 0) {
                Wpn.Facing = "F";
            }
            if (Wpn.Facing == 1) {
                Wpn.Facing = "S";
            }
            if (Wpn.Facing == 2) {
                Wpn.Facing = "R";
            }
            if (Wpn.Facing == 3) {
                Wpn.Facing = "TT";
            }
            if (Wpn.Facing == 4) {
                Wpn.Facing = "BT";
            }
            if (Wpn.Facing == 5) {
                Wpn.Facing = "TR";
            }
            if (Wpn.Facing == 6) {
                Wpn.Facing = "BR";
            }
            if (Wpn.Facing == 7) {
                Wpn.Facing = "UP";
            }
            if (Wpn.Facing == 8) {
                Wpn.Facing = "C";
            }
            if (Wpn.Facing == 11) {
                Wpn.Facing = "INT";
            }
            if (Wpn.Facing == 12) {
                Wpn.Facing = "XT";
            }
            Wpn.Device = timeline.scendata_dev[Wpn.DevID];

            html += weapon_template(Wpn);

        });
        html += "</ul>";
        var vals = ["11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "10"];
        html += "<strong>Armament (Ext Range):</strong>";
        html += "<ul>";

        $.each(vals, function (i, val) {
            var Wpn = [];
            Wpn.DevID = timeline.scendata_air["WpnDevID" + val];
            Wpn.Ammo = timeline.scendata_air["WpnAmmo" + val];
            Wpn.Facing = timeline.scendata_air["WpnFacing" + val];
            Wpn.Number = timeline.scendata_air["WpnNumber" + val];
            if (Wpn.DevID == 0) {
                return true;//continue;
            }
            if (Wpn.Facing == 0) {
                Wpn.Facing = "F";
            }
            if (Wpn.Facing == 1) {
                Wpn.Facing = "S";
            }
            if (Wpn.Facing == 2) {
                Wpn.Facing = "R";
            }
            if (Wpn.Facing == 3) {
                Wpn.Facing = "TT";
            }
            if (Wpn.Facing == 4) {
                Wpn.Facing = "BT";
            }
            if (Wpn.Facing == 5) {
                Wpn.Facing = "TR";
            }
            if (Wpn.Facing == 6) {
                Wpn.Facing = "BR";
            }
            if (Wpn.Facing == 7) {
                Wpn.Facing = "UP";
            }
            if (Wpn.Facing == 8) {
                Wpn.Facing = "C";
            }
            if (Wpn.Facing == 11) {
                Wpn.Facing = "INT";
            }
            if (Wpn.Facing == 12) {
                Wpn.Facing = "XT";
            }
            Wpn.Device = timeline.scendata_dev[Wpn.DevID];

            html += weapon_template(Wpn);

        });
        html += "</ul>";

        html += "</div>";
        html += "</div>";
    } else if (current_unit.type_str == "Base") {



    } else if (current_unit.type_str == "Ship") {

        html += "<div class='panel-heading'><h2>" + timeline.scendata_cls["Name"] + "-class</h2></div>";

        html += "<div class='panel-body'>";
        html += ship_class_template(timeline.scendata_cls);


        var vals = ["", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", ];
        html += "<strong>Armament:</strong>";
        html += "<ul>";

        $.each(vals, function (i, val) {
            var Wpn = [];
            Wpn.DevID = timeline.scendata_cls["WpnDevID" + val];
            Wpn.Ammo = timeline.scendata_cls["WpnAmmo" + val];
            Wpn.Facing = timeline.scendata_cls["WpnFacing" + val];
            Wpn.Number = timeline.scendata_cls["WpnNumber" + val];
            Wpn.Turrent = timeline.scendata_cls["WpnTurrent" + val];
            Wpn.Armour = timeline.scendata_cls["WpnArmour" + val];
            if (Wpn.DevID == 0) {
                return true;//continue;
            }
            if (Wpn.Facing == 0) {
                Wpn.Facing = "F";
            }
            if (Wpn.Facing == 1) {
                Wpn.Facing = "C";
            }
            if (Wpn.Facing == 2) {
                Wpn.Facing = "R";
            }
            if (Wpn.Facing == 3) {
                Wpn.Facing = "RS";
            }
            if (Wpn.Facing == 4) {
                Wpn.Facing = "LS";
            }
            if (Wpn.Facing == 5) {
                Wpn.Facing = "A";
            }
            Wpn.Device = timeline.scendata_dev[Wpn.DevID];

            if (Wpn.Number == 0)
                Wpn.Number = Wpn.Armour;
            html += weapon_template(Wpn);

        });
        html += "</ul>";
        html += "</div>";
        html += "</div>";

    } else if (current_unit.type_str == "LCU") {
        if (timeline.scendata_toe != null) {
            html += "<div class='panel-heading'><h2>" + timeline.scendata_toe["Name"] + " TOE</h2></div>";

            html += "<div class='panel-body'>";
            var vals = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", ];

            html += "<ul>";

            $.each(vals, function (i, val) {
                var Wpn = [];
                Wpn.DevID = timeline.scendata_toe["WpnDevID" + val];
                //Wpn.Ammo = timeline.scendata_toe["WpnAmmo" + val];
                // Wpn.Facing = timeline.scendata_toe["WpnFacing" + val];
                Wpn.Number = timeline.scendata_toe["WpnNumber" + val];
                if (Wpn.DevID == 0) {
                    return true;//continue;
                }

                Wpn.Device = timeline.scendata_dev[Wpn.DevID];
                if (Wpn.Device != null) {
                    html += toe_weapon_template(Wpn);
                }


            });
            html += "</ul>";
            html += "</div>";
            html += "</div>";
        } else { //if no scendata_toe (this unit is then implicit)
            html += "<div class='panel-heading'><h2>TOE</h2></div>";

            html += "<div class='panel-body'>";
            var vals = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", ];

            html += "<ul>";
            if (timeline.scendata_loc["TOEDevID1"] != 0) { // check implicit toe
                $.each(vals, function (i, val) {
                    var Wpn = [];
                    Wpn.DevID = timeline.scendata_loc["TOEDevID" + val];
                    //Wpn.Ammo = timeline.scendata_toe["WpnAmmo" + val];
                    // Wpn.Facing = timeline.scendata_toe["WpnFacing" + val];
                    Wpn.Number = timeline.scendata_loc["TOENumber" + val];
                    if (Wpn.DevID == 0) {
                        return true;//continue;
                    }

                    Wpn.Device = timeline.scendata_dev[Wpn.DevID];
                    if (Wpn.Device != null) {
                        html += toe_weapon_template(Wpn);
                    }


                });
            } else { // fallback to just reading out what I started with >_> witpae is nuts
                $.each(vals, function (i, val) {
                    var Wpn = [];
                    Wpn.DevID = timeline.scendata_loc["WpnDevID" + val];
                    //Wpn.Ammo = timeline.scendata_toe["WpnAmmo" + val];
                    // Wpn.Facing = timeline.scendata_toe["WpnFacing" + val];
                    Wpn.Number = timeline.scendata_loc["WpnNumber" + val];
                    if (Wpn.DevID == 0) {
                        return true;//continue;
                    }

                    Wpn.Device = timeline.scendata_dev[Wpn.DevID];
                    if (Wpn.Device != null) {
                        html += toe_weapon_template(Wpn);
                    }


                });
            }
            html += "</ul>";
            html += "</div>";
            html += "</div>";
        }
    }
    html += "</div>";
    html += "</div>";
    return html;
}

function render_graph_html(title, id) {
    html = "";
    html += "<div class='col-sm-6'>";
    html += "<div class='panel panel-default'>";

    html += "<div class='panel-heading'><h2>" + title + "</h2></div>";

    html += "<div class='panel-body'>";
    html += "<div id='" + id + "'></div>";
    html += "</div>";
    html += "</div>";
    html += "</div>";

    return html;
}
function display_timeline(timeline) {
    //document.title = document.title.replace(model_timeline_id, timeline.name);
    var all_html = "";

    //get the data for the graphs here
    var units = timeline.unit_data.slice(); // copy array for graphs

    timeline.unit_data.reverse();//scroll down = go backwards in time
    var current_unit = timeline.unit_data.shift();



    all_html += render_unit_data_panel(-1, current_unit);
    document.querySelector('#unit-timeline').innerHTML = all_html;
    //PUT GRAPHS IN HERE, SUPPLY ETC!
    //comments too!

    all_html += render_comments(timeline, current_unit);
    document.querySelector('#unit-timeline').innerHTML = all_html;

    //  all_html += render_graphs_html(timeline, current_unit);
    //  document.querySelector('#unit-timeline').innerHTML = all_html;

    if (current_unit.type_str == "Base") {
        all_html += render_graph_html("Supply & Fuel", "supply-fuel-graph");
    }
    if (current_unit.type_str == "AirGroup") {
        all_html += render_graph_html("Ready/Repair/Reserve/Max", "air-group-ready-graph");
        all_html += render_graph_html("Exp/Mor/Fat", "air-group-exp-mor-fat-graph");
        all_html += render_graph_html("Pilots", "air-group-pilots-graph");
        all_html += render_graph_html("Kills", "air-group-kills-graph");
    }
    if (current_unit.type_str == "Ship") {
        all_html += render_graph_html("Ship Damage", "ship-damage-graph");
    }
    if (current_unit.type_str == "LCU") {
        all_html += render_graph_html("Vital Statistics", "lcu1-graph");
        all_html += render_graph_html("Assault Value", "lcu2-graph");
        all_html += render_graph_html("Base Load", "lcu3-graph");
        all_html += render_graph_html("Supply", "lcu4-graph");
    }
if (current_unit.type_str != "Base") {
        all_html += render_class(timeline, current_unit);
        document.querySelector('#unit-timeline').innerHTML = all_html;
    }


    $.each(timeline.unit_data, function (i, unit) { all_html += render_unit_data_panel(i, unit); document.querySelector('#unit-timeline').innerHTML = all_html; });


    document.querySelector('#unit-timeline').innerHTML = all_html;


    if (current_unit.type_str == "Base") {
        //supply-fuel-graph
        var data = new google.visualization.DataTable();
        data.addColumn('date', 'Turn');
        data.addColumn('number', 'Supply');
        data.addColumn('number', 'Fuel');

        $.each(units, function (i, unit) {
            data.addRows([[javascript_is_retarded(unit), Number(unit.row.Supply), Number(unit.row.Fuel)]]);
        });


        var options = {
            hAxis: {
                title: 'Turn'
            },
            series: {
                1: { curveType: 'function' }
            }
        };

        var chart = new google.visualization.LineChart(document.getElementById('supply-fuel-graph'));
        chart.draw(data, options);
    }

    if (current_unit.type_str == "AirGroup") {
        //supply-fuel-graph
        var data = new google.visualization.DataTable();
        data.addColumn('date', 'Turn');
        data.addColumn('number', 'Ready');
        data.addColumn('number', 'Repair');
        data.addColumn('number', 'Reserve');
        data.addColumn('number', 'Max');

        $.each(units, function (i, unit) {
            data.addRows([[javascript_is_retarded(unit), Number(unit.row.Ready), Number(unit.row.Repair), Number(unit.row.Reserve), Number(unit.row.MaxPlanes)]]);
        });


        var options = {
            hAxis: {
                title: 'Turn'
            },
            series: {
                1: { curveType: 'function' }
            }
        };

        var chart = new google.visualization.LineChart(document.getElementById('air-group-ready-graph'));
        chart.draw(data, options);


        data = new google.visualization.DataTable();
        data.addColumn('date', 'Turn');
        data.addColumn('number', 'Experience');
        data.addColumn('number', 'Morale');
        data.addColumn('number', 'Fatigue');

        $.each(units, function (i, unit) {
            data.addRows([[javascript_is_retarded(unit), Number(unit.row.Exp), Number(unit.row.Morale), Number(unit.row.Fat)]]);
        });
        chart = new google.visualization.LineChart(document.getElementById('air-group-exp-mor-fat-graph'));
        chart.draw(data, options);






        data = new google.visualization.DataTable();
        data.addColumn('date', 'Turn');
        data.addColumn('number', 'Pilots');

        $.each(units, function (i, unit) {
            data.addRows([[javascript_is_retarded(unit), Number(unit.row.Pilots.split(' ')[0])]]);
        });
        chart = new google.visualization.LineChart(document.getElementById('air-group-pilots-graph'));
        chart.draw(data, options);




        data = new google.visualization.DataTable();
        data.addColumn('date', 'Turn');
        data.addColumn('number', 'Kills');

        $.each(units, function (i, unit) {
            data.addRows([[javascript_is_retarded(unit), Number(unit.row.Kills)]]);
        });
        chart = new google.visualization.LineChart(document.getElementById('air-group-kills-graph'));
        chart.draw(data, options);
    }
    if (current_unit.type_str == "Ship") {
        var data = new google.visualization.DataTable();
        data.addColumn('date', 'Turn');
        data.addColumn('number', 'Sys');
        data.addColumn('number', 'Flt');
        data.addColumn('number', 'Eng');
        data.addColumn('number', 'Fire');
        data.addColumn('number', 'Wep');

        $.each(units, function (i, unit) {
            data.addRows([[javascript_is_retarded(unit), Number(unit.row.Sys.split(' ')[0]), Number(unit.row.Flt.split(' ')[0]), Number(unit.row.Eng.split(' ')[0]), Number(unit.row.Fire.split(' ')[0]), Number(unit.row.Weapon.split(' ')[0])]]);
        });


        var options = {
            hAxis: {
                title: 'Turn'
            },
            series: {
                1: { curveType: 'function' }
            }
        };

        var chart = new google.visualization.LineChart(document.getElementById('ship-damage-graph'));
        chart.draw(data, options);
    }
    if (current_unit.type_str == "LCU") {
        var data = new google.visualization.DataTable();
        data.addColumn('date', 'Turn');
        data.addColumn('number', 'Experience');
        data.addColumn('number', 'Morale');
        data.addColumn('number', 'Disruption');
        data.addColumn('number', 'Fatigue');


        $.each(units, function (i, unit) {
            data.addRows([[javascript_is_retarded(unit), Number(unit.row.Exp), Number(unit.row.Morale), Number(unit.row.Disr), Number(unit.row.Fat)]]);
            });
                
        var options = {
            hAxis: {
                title: 'Turn'
            },
            series: {
                1: { curveType: 'function' }
            }
        };

        var chart = new google.visualization.LineChart(document.getElementById('lcu1-graph'));
        chart.draw(data, options);




        data = new google.visualization.DataTable();
        data.addColumn('date', 'Turn');
        data.addColumn('number', 'AV');


        $.each(units, function (i, unit) {
            data.addRows([[javascript_is_retarded(unit), Number(unit.row.AV)]]);
            });
                
        chart = new google.visualization.LineChart(document.getElementById('lcu2-graph'));
        chart.draw(data, options);


        


        data = new google.visualization.DataTable();
        data.addColumn('date', 'Turn');
        data.addColumn('number', 'Load');


        $.each(units, function (i, unit) {
            data.addRows([[javascript_is_retarded(unit), Number(unit.row.BaseLoad)]]);
            });
                
        chart = new google.visualization.LineChart(document.getElementById('lcu3-graph'));
        chart.draw(data, options);



data = new google.visualization.DataTable();
        data.addColumn('date', 'Turn');
        data.addColumn('number', 'Supply');
        data.addColumn('number', 'Max');


        $.each(units, function (i, unit) {
            data.addRows([[javascript_is_retarded(unit), Number(unit.row.Supply.split(' ')[0]), Number(unit.row.Supply.split(' ')[2]) ]]);
            });
                
        chart = new google.visualization.LineChart(document.getElementById('lcu4-graph'));
        chart.draw(data, options);
    }
}
//download the json
(function () {
    var link = "/OperationGlacierGameData/"
        + model_game_name
        + "/Timeline/"
        + model_timeline_id
        + ".json";
    $.getJSON(link).done(display_timeline);


}).call(this);
