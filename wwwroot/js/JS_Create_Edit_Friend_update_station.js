
// Обновление списка участков после выбора городского округа
$(function () {
    $("#CityDistrictId").change(function () {

        chbox = document.getElementById('boolUnpinning');

        if (!chbox.checked) {
            var formData = { 'IdCity': Number.parseInt($('#CityDistrictId').val()), 'Name': $('#CityDistrictId>option:selected').text() };
            $.ajax({
                url: partMyURL + "/api/API/searchStations/city",
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
                            return (Number.parseInt(a) - Number.parseInt(b));
                        });
                    }
                    DataFillingSelect(dataSort, 'idStation', 'name', '#StationId', '<option/>');
                },
                error: function (result, status, er) {
                    alert("error: " + result + " status: " + status + " er:" + er);
                }
            });
        }
    });
});

// Обновление списка участков после выбора населенного пункта
function UpdateStationByCityAfterSelect() {

    var formData = { 'IdCity': Number.parseInt('1'), 'Name': '' };

    $.ajax({
        url: partMyURL + "/api/API/searchStations/city",
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
                    return (Number.parseInt(a) - Number.parseInt(b));
                });
            }
            DataFillingSelect(dataSort, 'idStation', 'name', '#StationId', '<option/>');
        },
        error: function (result, status, er) {
            alert("error: " + result + " status: " + status + " er:" + er);
        }
    });
}

// Обновление списка участков после выбора улицы
$(function () {
    $("#StreetId").change(function () {

        chbox = document.getElementById('boolUnpinning');

        if (!chbox.checked) {

        var formData = { 'IdStreet': Number.parseInt($('#StreetId').val()), 'Name': $('#StreetId>option:selected').text() };
        let select = document.getElementById('StreetId');
        const valueSelect = select.value;

            if ($('#StreetId').has('option').length != 0) {
                $.ajax({
                    url: partMyURL + "/api/API/searchStations/street",
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
                                return (Number.parseInt(a) - Number.parseInt(b));
                            });
                        }
                        UploadElectoralDistrictsByStation(data);
                        DataFillingSelect(dataSort, 'idStation', 'name', '#StationId', '<option/>');
                    },
                    error: function (result, status, er) {
                        alert("error: " + result + " status: " + status + " er:" + er);
                    }
                });
            }
        }
    });
});

// Обновление списка участков после выбора номера дома
$(function () {
    $('#HouseId').change(function () {

        chbox = document.getElementById('boolUnpinning');

        if (!chbox.checked) {

        var formData = { 'IdHouse': Number.parseInt($('#HouseId').val()), 'Name': $('#HouseId>option:selected').text() };
            let select = document.getElementById('HouseId');

            if ($('#HouseId').has('option').length != 0) {
                $.ajax({
                    url: partMyURL + "/api/API/searchStations/house",
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
                                return (Number.parseInt(a) - Number.parseInt(b));
                            });
                        }
                        DataFillingSelect(dataSort, 'idStation', 'name', '#StationId', '<option/>');

                        // Генерация события для элемента Select
                        let elemSelectPollingStation = document.querySelector('#StationId')
                        elemSelectPollingStation.selectedIndex = 0;
                        const event = new Event("change");
                        elemSelectPollingStation.dispatchEvent(event);
                    },
                    error: function (result, status, er) {
                        alert("error: " + result + " status: " + status + " er:" + er);
                    }
                });
            }
        }
    });
});

// Обновление списка округов после установления участка
$(function () {
    $('#StationId').change(function () {
        var formData = { 'IdStation': Number.parseInt($('#StationId').val()), 'Name': $('#StationId>option:selected').text() };

            $.ajax({
                url: partMyURL + "/api/API/searchElectoraldistrict/station",
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
                            return (Number.parseInt(a) - Number.parseInt(b));
                        });
                    }
                    DataFillingSelect(dataSort, 'idElectoralDistrict', 'name', '#ElectoralDistrictId', '<option/>');

                },
                error: function (result, status, er) {
                    alert("error: " + result + " status: " + status + " er:" + er);
                }
            });
    });
});

// Обновление списка округов после выбора УЛИЦЫ
function UploadElectoralDistrictsByStation(masStation) {

    if (masStation != undefined && masStation.length > 0) {
        $.ajax({
            url: partMyURL + "/api/API/searchElectoraldistrict/stations",
            //url: "/CollectVoters/api/API/searchPollingStations/house",
            headers:
            {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('#RequestVerificationToken').val()
            },
            type: 'POST',
            dataType: "json",
            data: JSON.stringify(masStation),
            success: function (data) {
                var dataSort = data;
                if (data != undefined) {
                    dataSort = data.sort(function (a, b) {
                        return (Number.parseInt(a) - Number.parseInt(b));
                    });
                }
                DataFillingSelect(dataSort, 'idElectoralDistrict', 'name', '#ElectoralDistrictId', '<option/>');

            },
            error: function (result, status, er) {
                alert("error: " + result + " status: " + status + " er:" + er);
            }
        });
    }
    else {
        $('#ElectoralDistrictId').empty();
    }
}

// Загрузка всех избирательных округов
function UploadElectoralDistrictsAll() {
    $.ajax({
        url: partMyURL + "/api/API/getAllElectoraldistrict",
        //url: "/CollectVoters/api/API/searchPollingStations/house",
        headers:
        {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('#RequestVerificationToken').val()
        },
        type: 'GET',
        dataType: "json",
        data: null,
        success: function (data) {
            var dataSort = data;
            if (data != undefined) {
                dataSort = data.sort(function (a, b) {
                    return (Number.parseInt(a) - Number.parseInt(b));
                });
            }
            DataFillingSelect(dataSort, 'idElectoralDistrict', 'name', '#ElectoralDistrictId', '<option/>');

        },
        error: function (result, status, er) {
            alert("error: " + result + " status: " + status + " er:" + er);
        }
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

function UploadStationByCitydistrictAndStreetAndHouse() {

    if ($('#StreetId>option:selected').text() !== '' && $('#StreetId>option:selected').text() !== ' ' && $('#HouseId>option:selected').text() !== '' && $('#HouseId>option:selected').text() !== ' ') {

        var formData = {
            'IdHouse': Number.parseInt($('#HouseId').val()), 'NameHouse': $('#HouseId>option:selected').text(),
            'IdStreet': Number.parseInt($('#StreetId').val()), 'NameStreet': $('#StreetId>option:selected').text()};


            $.ajax({
                url: partMyURL + "/api/API/searchStations/streetAndhouse",
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
                            return (Number.parseInt(a) - Number.parseInt(b));
                        });
                    }
                    DataFillingSelect(dataSort, 'idStation', 'name', '#StationId', '<option/>');

                    // Генерация события для элемента Select
                    let elemSelectPollingStation = document.querySelector('#StationId')
                    elemSelectPollingStation.selectedIndex = 0;
                    const event = new Event("change");
                    elemSelectPollingStation.dispatchEvent(event);
                },
                error: function (result, status, er) {
                    alert("error: " + result + " status: " + status + " er:" + er);
                }
            });
    }
}
