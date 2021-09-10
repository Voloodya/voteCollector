
$(document).ready(function () {

    document.getElementById('totalFriends').innerHTML = CountValuetdTable('reportCitiesTable', 1);
    document.getElementById('totalRegistretion').innerHTML = CountValuetdTable('reportCitiesTable', 2);
    document.getElementById('totalNumberQRcodeText').innerHTML = CountValuetdTable('reportCitiesTable', 4);
});


function CountValuetdTable(idObjectCount, numberColumn) {
    var table = document.getElementById(idObjectCount);
    var rows = table.tBodies[0].rows;
    var total = 0;

    for (var i = 0, iLen = rows.length - 1; i <= iLen; i++) {

        var value = rows[i].cells[numberColumn].innerHTML;
        total += Number.parseInt(value);
    }
    return total;
}
