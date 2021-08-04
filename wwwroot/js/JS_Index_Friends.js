
let partMyURL = "/CollectVoters";
if (window.location.href.substring(0, 16) == "http://localhost") {
    partMyURL = "";
}

// Define a custom selector icontains instead of overriding the existing expression contains
// A global js asset file will be a good place to put this code
//$.expr[':'].icontains = function(a, i, m) {
//  return $(a).text().toUpperCase()
//      .indexOf(m[3].toUpperCase()) >= 0;
//};


//$(function() {
//    $('#filter15').on('keyup', function() {  // changed 'change' event to 'keyup'. Add a delay if you prefer
//        $("#friendTable td.colFilter:icontains('" + $(this).val() + "')").parent().show();  // Use our new selector icontains
//        $("#friendTable td.colFilter:not(:icontains('" + $(this).val() + "'))").parent().hide();  // Use our new selector icontains
//    });

//});

var friendDataTable;

$(document).ready(function () {

     friendDataTable = $('#friendTable').DataTable({
        retrieve: true,
        paging: false,
        language: {
            url: "http://cdn.datatables.net/plug-ins/1.10.20/i18n/Russian.json"
        },

        initComplete: function () {
            this.api().columns([6,8,13,14,19,21]).every(function () {
                var column = this;
                var select = $('<select><option value="">Все</option></select>')
                    .appendTo($($(column.header()))) //$(column.footer().empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });

                $(select).click(function (e) {
                    e.stopPropagation();
                });

                column.data().unique().sort().each(function (d, j) {
                    if (column.search() === '^' + d + '$') {
                        select.append('<option value="' + d + '" selected="selected">' + d.substr(0, 30) + '</option>')
                    } else {
                        select.append('<option value="' + d + '">' + d.substr(0, 30) + '</option>')
                    }
                });
            });
        }
    });
});

// Действия при перерисовки таблицы
$('#friendTable').on('draw.dt', function () {
    countVoters('friendTable', 18);
});

function UpdateTablePlagin () {

    $('#friendTable').DataTable({

        retrieve: true,
        paging: false,
        language: {
            url: "http://cdn.datatables.net/plug-ins/1.10.20/i18n/Russian.json"
        },

        initComplete: function () {
            this.api().columns([6, 8, 13, 14, 19, 21]).every(function () {
                var column = this;
                var select = $('<select><option value="">Все</option></select>')
                    .appendTo($($(column.header()))) //$(column.footer().empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });

                $(select).click(function (e) {
                    e.stopPropagation();
                });

                column.data().unique().sort().each(function (d, j) {
                    if (column.search() === '^' + d + '$') {
                        select.append('<option value="' + d + '" selected="selected">' + d.substr(0, 30) + '</option>')
                    } else {
                        select.append('<option value="' + d + '">' + d.substr(0, 30) + '</option>')
                    }
                });
            });
        }
    });
};


// Загрузка избирательных округов
$(document).ready(function () {

    $.ajax({
        // url: "http://localhost:18246/api/API/getElectoralDistrict",
        url: partMyURL + "/api/API/getElectoralDistrict",
        //url: "/CollectVoters/api/API/getElectoralDistrict",
        headers:
        {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('#RequestVerificationToken').val()
        },
        type: 'GET',
        dataType: "json",
        success: function (data) {

            if (data != undefined) {
                var dataSort = data.sort(function (a, b) {
                    return ((a.name === b.name) ? 0 : ((a.name > b.name) ? 1 : -1));
                });
            }
            DataFillingSelect(dataSort, 'idElectoralDistrict', 'name', 'SelectElectoralDistrictId', '<option/>');

        },
        error: function (result, status, er) {
            alert("error: " + result + " status: " + status + " er:" + er);
        }
    });

});


let nrows = document.getElementById('friendTable').tBodies[0].rows.length;
document.getElementById('numberRecords').innerHTML = "Количество избирателей: " + (nrows);

$('friendTable').ready(countVoters('friendTable',18));
$('friendTable').change(countVoters('friendTable',18));

function countVoters(idObjectCount, numberColumn) {
    var table = document.getElementById(idObjectCount);
    var rows = table.tBodies[0].rows;
    var total = 0;
    // Assume first row is headers, adjust as required
    // Assume last row is footer, addjust as required
    for (var i = 0, iLen = rows.length - 1; i <= iLen; i++) {
        var checked_ = rows[i].cells[numberColumn].getElementsByTagName('input')[0].checked;
        if (checked_ == true) {
            total += 1;
        }
    }
    document.getElementById('totalVoter').innerHTML = total.toFixed(0);
}

function deleteSelected(idObject, numberColumn) {

    var table = document.getElementById(idObject);
    var rows = table.tBodies[0].rows;
    var jsonMasId = [];
    // Assume first row is headers, adjust as required
    // Assume last row is footer, addjust as required
    for (let i = 0, iLen = rows.length - 1; i <= iLen; i++) {
        var checked_ = rows[i].cells[numberColumn].getElementsByTagName('input')[0].checked;
        if (checked_ == true) {

            var idFriend = rows[i].cells[1].innerHTML;
            idFriend = idFriend.trim(); //Удаляем пробелы в начале и в конце
            jsonMasId.push(Number.parseInt(idFriend));
        }
    }
    if (jsonMasId.length > 0) {
        $.ajax({
            url: partMyURL +'/api/APIFriends/DeleteFriends/',
            //url: '/CollectVoters/api/APIFriends/DeleteFriends/',
            type: 'POST',
            data: JSON.stringify(jsonMasId),
            contentType: "application/json;charset=utf-8",
            success: function (response) {
                console.log(response);
                deleteTrTableBody(idObject, jsonMasId);
                updatingFields(idObject, 'numberRecords');
            },
            error: function (result, status, er) {
                alert("error: " + result + " status: " + status + " er:" + er);
            }
        });
    }
}
// Обновление списка избирателей
$(function () {
    $("#SelectElectoralDistrictId").change(function () {
        var formData = { 'IdElectoralDistrict': Number.parseInt($('#SelectElectoralDistrictId').val()), 'Name': $('#SelectElectoralDistrictId>option:selected').text() };
        //$('#' + 'friendTable' + ' tbody > tr').remove();

        //Удаление строк через DataTable
        var friendDataTable = $('#friendTable').DataTable();
        friendDataTable.rows().remove().draw();

        $.ajax({
            type: 'POST',
            //    url: '@Url.Action("GetPolingStationsByElectoralDistrict")',
            url: 'Admin/SearchFriendsByElectoralDistrict/',
            headers:
            {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('#RequestVerificationToken').val()
            },
            data: JSON.stringify(formData),
            success: function (data) {

                // Delete rows
                var friendDataTable = $('#friendTable').DataTable();
                friendDataTable.rows().remove().draw();
                // Destroy object DataTable
                friendDataTable.destroy();
                //$('#friendTable').empty();

                // Update tbody new rows with data
                $('#TbodyFriendTable').replaceWith(data);
                updatingFields('friendTable', 'numberRecords');

                UpdateTablePlagin();
            },
            error: function (result, status, er) {
                alert('error: ' + result + ' status: ' + status + ' er:' + er);
            }
        });
    });
});

function deleteTrTableBody(idObject, jsonMasId) {

    var table = document.getElementById(idObject);
    var rows = table.rows;

    for (let i = 1; i <= (rows.length - 1); i++) {

        var idFriend = table.rows[i].cells[1].innerHTML;
        if (jsonMasId.indexOf(Number.parseInt(idFriend)) != -1) {

            table.rows[i].remove();
            i--;
        }
    }
}

function updatingFields(idObjectSelect, idObjectUpdate) {
    let nrows = document.getElementById(idObjectSelect).tBodies[0].rows.length;
    document.getElementById(idObjectUpdate).innerHTML = "Количество избирателей: " + (nrows);
}

//Заполнение объекта html данными из json массива
function DataFillingSelect(data, nameProperty1, nameProperty2, idObject, propertyHtml) {

    var objectHtml = $('#' + idObject);
    objectHtml.empty();

    $.each(data, function (index, dataInstance) {
        objectHtml.append($(propertyHtml,
            {
                value: dataInstance[nameProperty1],
                text: dataInstance[nameProperty2]
            }));
    });
}
