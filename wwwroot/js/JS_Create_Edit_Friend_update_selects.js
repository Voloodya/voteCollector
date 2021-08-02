
let partMyURL = "/CollectVoters";
if (window.location.href.substring(0, 16) == "http://localhost") {
    partMyURL = "";
}


// Обновление списка участков после выбора населенного пункта
$(function () {
    $("#CityId").change(function () {
        var formData = { 'IdCity': Number.parseInt($('#CityId').val()), 'Name': $('#CityId>option:selected').text()  };
        $.ajax({
            url: partMyURL + "/api/API/searchPollingStations/city",
           // url: "/CollectVoters/api/API/searchPollingStations/city",
            headers:
            {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('#RequestVerificationToken').val()
            },
            type: 'POST',
            dataType: "json",
            data: JSON.stringify(formData),
            success: function (data) {
                var dataSort = [];
                if (data != undefined) {
                    dataSort = data.sort(function (a, b) {
                    return ((a.name === b.name) ? 0 : ((a.name > b.name) ? 1 : -1));
                    });
                }
                dataFilling(dataSort, 'idPollingStation', 'name', '#PollingStationId', '<option/>');
            },
            error: function (result, status, er) {
                alert("error: " + result + " status: " + status + " er:" + er);
            }
        });
    });
});

// Обновление списка участков после выбора улицы
$(function () {
    $("#StreetId").change(function () {
        var formData = { 'IdStreet': Number.parseInt($('#StreetId').val()), 'Name': $('#StreetId>option:selected').text() };
        let select = document.getElementById('StreetId');
        const valueSelect = select.value;
        if ($('#StreetId').has('option').length != 0) {
            $.ajax({
                url: partMyURL + "/api/API/searchPollingStations/street",
                //url: "/CollectVoters/api/API/searchPollingStations/street",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('#RequestVerificationToken').val()
                },
                type: 'POST',
                dataType: "json",
                data: JSON.stringify(formData),
                success: function (data) {

                    var dataSort = [];
                    if (data != undefined) {
                          dataSort = data.sort(function (a, b) {
                          return ((a.name === b.name) ? 0 : ((a.name > b.name) ? 1 : -1));
                        });
                    }
                    dataFilling(dataSort, 'idPollingStation', 'name', '#PollingStationId', '<option/>');

                },
                error: function (result, status, er) {
                    alert("error: " + result + " status: " + status + " er:" + er);
                }
            });
        }
    });
});

// Обновление списка участков после выбора номера дома
$(function () {
    $('#HouseId').change(function () {
        var formData = { 'IdHouse': Number.parseInt($('#HouseId').val()), 'Name': $('#HouseId>option:selected').text() };
        let select = document.getElementById('HouseId')
        if ($('#HouseId').has('option').length != 0) {
            $.ajax({
                url: partMyURL + "/api/API/searchPollingStations/house",
                //url: "/CollectVoters/api/API/searchPollingStations/house",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('#RequestVerificationToken').val()
                },
                type: 'POST',
                dataType: "json",
                data: JSON.stringify(formData),
                success: function (data) {
                    var dataSort = [];
                    if (data != undefined) {
                        dataSort = data.sort(function (a, b) {
                        return ((a.name === b.name) ? 0 : ((a.name > b.name) ? 1 : -1));
                        });
                    }
                    dataFilling(dataSort, 'idPollingStation', 'name', '#PollingStationId', '<option/>');

                    // Генерация события для элемента Select
                    let elemSelectPollingStation = document.querySelector('#PollingStationId')
                    elemSelectPollingStation.selectedIndex = 0;
                    const event = new Event("change");
                    elemSelectPollingStation.dispatchEvent(event);
                },
                error: function (result, status, er) {
                    alert("error: " + result + " status: " + status + " er:" + er);
                }
            });
        }
    });
});


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

// Взятие из SElect option, который выбран
function GetSelectedOption(select) {
    const option = select.querySelector(`option[value="${select.value}"]`)
    return option;
}

        $(document).ready(function () {
            $('#StreetId').select2({
                dropdownParent: $('#divStreet'),
                placeholder: "Выберите улицу",
                minimumInputLength: 2, // only start searching when the user has input 2 or more characters
                maximumInputLength: 15, // only allow terms up to 15 characters long
                language: "ru"
            });
        });