
let partMyURL = "/CollectVoters";
if (window.location.href.substring(0, 16) == "http://localhost") {
    partMyURL = "";
}

$(document).ready(function () {
    $('#stationsTable').DataTable({

        retrieve: true,
        paging: false,
        //ajax: '/ajax/arrays.txt',
        language: {
            url: "http://cdn.datatables.net/plug-ins/1.10.20/i18n/Russian.json"
        },

        initComplete: function () {
            this.api().columns([0,1,2]).every(function () {
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
            dataFilling(dataSort, 'idElectoralDistrict', 'name', '#SelectElectoralDistrictId', '<option/>');

            // Генерация события для элемента SelectElectoralDistrictId
            let elemSelectHouse = document.querySelector('#SelectElectoralDistrictId')
            elemSelectHouse.selectedIndex = 0;
            const event = new Event("change");
            elemSelectHouse.dispatchEvent(event);
        },
        error: function (result, status, er) {
            alert("error: " + result + " status: " + status + " er:" + er);
        }
    });

});

// Загрузка населенных пунктов
//$(document).ready(function () {

//    $.ajax({
//            // url: "http://localhost:18246/api/API/getSities",
//            url: partMyURL + "/api/API/getSities",
//            //url: "/CollectVoters/api/API/getSities",
//            headers:
//            {
//                'Accept': 'application/json',
//                'Content-Type': 'application/json',
//                'RequestVerificationToken': $('#RequestVerificationToken').val()
//            },
//            type: 'GET',
//            dataType: "json",
//            success: function (data) {

//                if (data != undefined) {
//                    var dataSort = data.sort(function (a, b) {
//                        return ((a.name === b.name) ? 0 : ((a.name > b.name) ? 1 : -1));
//                    });
//                }
//                dataFilling(dataSort, 'idCity', 'name', '#SelectCityId', '<option/>');

//                // Генерация события для элемента SelectCityId
//                let elemSelectHouse = document.querySelector('#SelectCityId')
//                elemSelectHouse.selectedIndex = 0;
//                const event = new Event("change");
//                elemSelectHouse.dispatchEvent(event);
//            },
//            error: function (result, status, er) {
//                alert("error: " + result + " status: " + status + " er:" + er);
//            }
//        });

//    });

//// Обновление списка улиц после выбора города
//$(function () {
//    $("#SelectCityId").change(function () {
//        var formData = { 'IdCity': Number.parseInt($('#SelectCityId').val()), 'Name': $('#SelectCityId>option:selected').text() };
//        $.ajax({
//            // url: "http://localhost:18246/api/API/searchStreets",
//            url: partMyURL + "/api/API/searchStreets",
//            //url: "/CollectVoters/api/API/searchStreets",
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

//                if (data != undefined) {
//                    var dataSort = data.sort(function (a, b) {
//                        return ((a.name === b.name) ? 0 : ((a.name > b.name) ? 1 : -1));
//                    });
//                }
//                dataFilling(dataSort, 'idStreet', 'name', '#SelictStreetId', '<option/>');

//                // Генерация события для элемента Select
//                let elemSelectHouse = document.querySelector('#SelictStreetId')
//                elemSelectHouse.selectedIndex = 0;
//                const event = new Event("change");
//                elemSelectHouse.dispatchEvent(event);
//            },
//            error: function (result, status, er) {
//                alert("error: " + result + " status: " + status + " er:" + er);
//            }
//        });

//    });
//});



function RequestUpdateUIK(idSelectCity, idSelectStreet) {

}

//Заполнение объекта html данными из json массива
function dataFilling(data, nameProperty1, nameProperty2, idObject, propertyHtml) {

    var objectHtml = $(idObject);
    objectHtml.empty();

    $.each(data, function (index, dataInstance) {
        objectHtml.append($(propertyHtml,
            {
                value: dataInstance[nameProperty1],
                text: dataInstance[nameProperty2]
            }));
    });
}


