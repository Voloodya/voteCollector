
$(document).ready(function () {

    document.getElementById('totalFriends').innerHTML = CountValuetdTable('reportFieldActivityTable', 3);
    document.getElementById('totalRegistretion').innerHTML = CountValuetdTable('reportFieldActivityTable', 4);
    document.getElementById('totalNumberQRcodeText').innerHTML = CountValuetdTable('reportFieldActivityTable', 9);
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
