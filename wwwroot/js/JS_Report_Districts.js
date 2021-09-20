
$(document).ready(function () {

    document.getElementById('totalFriends').innerHTML = CountValuetdTable('reportDistrictsTable', 1);
    document.getElementById('totalRegistretion').innerHTML = CountValuetdTable('reportDistrictsTable', 2);
    document.getElementById('totalNumberQRcodeText').innerHTML = CountValuetdTable('reportDistrictsTable', 4);

    if (document.getElementById('totalRegistretion').innerHTML !== 0) {
        document.getElementById('totalPersentRegistration').innerHTML = ((document.getElementById('totalRegistretion').innerHTML / document.getElementById('totalFriends').innerHTML) * 100).toFixed(2);
    }

});


function CountValuetdTable(idObjectCount, numberColumn) {
    var table = document.getElementById(idObjectCount);
    var rows = table.tBodies[0].rows;
    var total = 0;

    for (var i = 0, iLen = rows.length - 1; i <= iLen; i++) {

        var district = rows[i].cells[0].innerHTML
        district = district.replace(/[^+\d]/g,'').trim();
        if (district === '142' || district === '143') {

            var value = rows[i].cells[numberColumn].innerHTML;
            value = value.replace(/[^+\d]/g, '').trim();
            total += Number.parseInt(value);
        }
    }
    return total;
}
