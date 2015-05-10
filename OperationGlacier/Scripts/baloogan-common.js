

function isEven(n) {
    return n == parseFloat(n) ? !(n % 2) : void 0;
}

function randomString(length) {
    var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz'.split('');

    if (!length) {
        length = Math.floor(Math.random() * chars.length);
    }

    var str = '';
    for (var i = 0; i < length; i++) {
        str += chars[Math.floor(Math.random() * chars.length)];
    }
    return "x" + str;
}

function AirGroupUnit(unit) {

    unit.airgroup_image_px_x = ((unit.bitmap - 1) % 4) * 150;
    unit.airgroup_image_px_y = Math.floor(((unit.bitmap - 1) / 4)) * 60;
    return unit;
}

function BaseUnit(unit) {

    unit.row.Supplyc = commaSeparateNumber(unit.row.Supply);
    unit.row.Fuelc = commaSeparateNumber(unit.row.Fuel);
    return unit;
}
function TaskForceUnit(unit) {
    unit.taskforce_image_px = 60;
    if (unit.row.Mission == "Air Combat") {
        unit.taskforce_image_px = unit.taskforce_image_px * 0;
    }
    if (unit.row.Mission == "Surface Combat") {
        unit.taskforce_image_px = unit.taskforce_image_px * 1;
    }
    if (unit.row.Mission == "Bombardment") {
        unit.taskforce_image_px = unit.taskforce_image_px * 2;
    }
    if (unit.row.Mission == "Fast Transport") {
        unit.taskforce_image_px = unit.taskforce_image_px * 3;
    }
    if (unit.row.Mission == "Transport") {
        unit.taskforce_image_px = unit.taskforce_image_px * 4;
    }
    if (unit.row.Mission == "Replenishment") {
        unit.taskforce_image_px = unit.taskforce_image_px * 5;
    }
    if (unit.row.Mission == "Mine Laying") {
        unit.taskforce_image_px = unit.taskforce_image_px * 6;
    }
    if (unit.row.Mission == "Sub Patrol") {
        unit.taskforce_image_px = unit.taskforce_image_px * 7;
    }
    if (unit.row.Mission == "Sub Minelaying") {
        unit.taskforce_image_px = unit.taskforce_image_px * 8;
    }
    if (unit.row.Mission == "Sub Transport") {
        unit.taskforce_image_px = unit.taskforce_image_px * 9;
    }
    if (unit.row.Mission == "Cargo") {
        unit.taskforce_image_px = unit.taskforce_image_px * 10;
    }
    if (unit.row.Mission == "Barge") {
        unit.taskforce_image_px = unit.taskforce_image_px * 11;
    }
    if (unit.row.Mission == "Air Transport") {
        unit.taskforce_image_px = unit.taskforce_image_px * 12;
    }
    if (unit.row.Mission == "CV Escort") {
        unit.taskforce_image_px = unit.taskforce_image_px * 13;
    }
    if (unit.row.Mission == "Amphibious") {
        unit.taskforce_image_px = unit.taskforce_image_px * 14;
    }
    if (unit.row.Mission == "ASW Combat") {
        unit.taskforce_image_px = unit.taskforce_image_px * 15;
    }
    if (unit.row.Mission == "PT Boat") {
        unit.taskforce_image_px = unit.taskforce_image_px * 16;
    }
    if (unit.row.Mission == "Tanker") {
        unit.taskforce_image_px = unit.taskforce_image_px * 17;
    }
    if (unit.row.Mission == "Mine Sweeping") {
        unit.taskforce_image_px = unit.taskforce_image_px * 18;
    }
    if (unit.row.Mission == "Landing Craft") {
        unit.taskforce_image_px = unit.taskforce_image_px * 19;
    }
    if (unit.row.Mission == "Midget Submarine") {
        unit.taskforce_image_px = unit.taskforce_image_px * 20;
    }
    if (unit.row.Mission == "Support") {
        unit.taskforce_image_px = unit.taskforce_image_px * 21;
    }
    if (unit.row.Mission == "Local Minesweeping") {
        unit.taskforce_image_px = unit.taskforce_image_px * 22;
    }
    if (unit.row.Mission == "Midget Sub Carrier") {
        unit.taskforce_image_px = unit.taskforce_image_px * 23;
    }
    if (unit.row.Mission == "Escort") {
        unit.taskforce_image_px = unit.taskforce_image_px * 24;
    }
    return unit;
}