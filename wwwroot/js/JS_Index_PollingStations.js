
let partMyURL = "";
if (window.location.href.substring(0, 16) == "http://localhost") {
    partMyURL = "";
}

$(document).ready(function () {
    $('#pollingStationsTable').DataTable({

        retrieve: true,
        paging: false,
        //ajax: '/ajax/arrays.txt',
        language: {
            url: "http://cdn.datatables.net/plug-ins/1.10.20/i18n/Russian.json"
        },

        initComplete: function () {
            this.api().columns([1,2,3]).every(function () {
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

//// Обновление списка участков
//$(function () {
//    $("#SelectElectoralDistrictId").change(function () {
//        var formData = { 'IdElectoralDistrict': Number.parseInt($('#SelectElectoralDistrictId').val()), 'Name': $('#SelectElectoralDistrictId>option:selected').text() };
//        $.ajax({
//            // url: "http://localhost:18246/api/APIPollingStations/searchStreets",
//            url: partMyURL + "/api/APIPollingStations/SearchPolingStationsByElectoralDistrict",
//            //url: "/CollectVoters/api/APIPollingStations/SearchPolingStationsByElectoralDistrict",
//            headers:
//            {
//                'Accept': 'application/json',
//                'Content-Type': 'application/json',
//                'RequestVerificationToken': $('#RequestVerificationToken').val()
//            },
//            type: 'POST',
//            dataType: "json",
//            data: JSON.stringify(formData),
//            success: function (data) {

//                dataSort = [];
//                if (data != undefined) {
//                        dataSort = data.sort(function (a, b) {
//                        return ((a.name === b.name) ? 0 : ((a.name > b.name) ? 1 : -1));
//                    });
//                }
//                dataFillingTableBody(dataSort, 'pollingStationsTable');

//            },
//            error: function (result, status, er) {
//                alert("error: " + result + " status: " + status + " er:" + er);
//            }
//        });

//    });
//});

// Обновление списка участков
$(function () {
    $("#SelectElectoralDistrictId").change(function () {
        var formData = {'IdElectoralDistrict': Number.parseInt($('#SelectElectoralDistrictId').val()), 'Name': $('#SelectElectoralDistrictId>option:selected').text() };

        $('#' + 'pollingStationsTable' + ' tbody > tr').remove();

        $.ajax({
            type: 'POST',
        //    url: '@Url.Action("GetPolingStationsByElectoralDistrict")',
            url: 'PollingStations/GetPolingStationsByElectoralDistrict/',
            headers:
            {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('#RequestVerificationToken').val()
            },
            //dataType: "json",
            data: JSON.stringify(formData),
            success: function (data) {

                $('#tableBodyPollingStation').replaceWith(data);

            },
            error: function (result, status, er) {
                alert("error: " + result + " status: " + status + " er:" + er);
            }
        });
    });

});


function deleteTrTableBody(idObject, jsonMas) {

    var table = document.getElementById(idObject);
    var rows = table.rows;

    for (let i = 1; i <= (rows.length - 1); i++) {

        var idFriend = table.rows[i].cells[1].innerHTML;
        if (jsonMas.indexOf(Number.parseInt(idFriend)) != -1) {

            table.rows[i].remove();
            i--;
        }
    }
}

function dataFillingTableBody(data, idObjectTable) {

    //var table = document.getElementById(idObjectTable);
    var bodyTable = $(`#${idObjectTable} tbody`);
    //var table = $(idObjectTable);
    //$(idObjectTable + ' tbody').empty();
    $('#'+idObjectTable + ' tbody > tr').remove();

    for (let i = 0; i < data.length; i++) {

        bodyTable.append(createRow(data[i]));
    }

}

function createRow(data) {

    var trElement = '<tr>';

    for (key in data) {

        trElement += '<td>' + data[key] + '</td>';
    }

    trElement += '</tr>';

    return trElement;
}

function RequestUpdateUIK(idSelectCity, idSelectStreet) {

}

//Заполнение объекта html данными из json массива
function DataFillingSelect(data, nameProperty1, nameProperty2, idObject, propertyHtml) {

    var objectHtml = $('#'+idObject);
    objectHtml.empty();

    $.each(data, function (index, dataInstance) {
        objectHtml.append($(propertyHtml,
            {
                value: dataInstance[nameProperty1],
                text: dataInstance[nameProperty2]
            }));
    });
}


