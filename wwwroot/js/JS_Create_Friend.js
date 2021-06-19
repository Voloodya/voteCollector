

//Автоматич установка даты голосования
function setDate(sourseidObj, idObject) {
    chbox = document.getElementById(sourseidObj);
    if (chbox.checked) {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
        var yyyy = today.getFullYear();
        today = yyyy + '-' + mm + '-' + dd;
        document.getElementById(idObject).value = today;
    }
    else {
        document.getElementById(idObject).value = null;
    }
}

// Обновление списка улиц после выбора города
$(function () {
    $("#CityId").change(function () {
        var formData = { 'CityId': Number.parseInt($('#CityId').val()), 'Name': $('#CityId>option:selected').text() };
        $.ajax({
            url: "http://localhost:18246/api/API/searchStreets",
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

                dataFilling(data, 'idStreet', 'name', '#StreetId', '<option/>');
            },
            error: function (result, status, er) {
                alert("error: " + result + " status: " + status + " er:" + er);
            }
        });
    });
});

// Обновление списка домов после выбора улицы
//$(function () {
//    $("#StreetId").change(function () {
//        var id = $('#StreetId').val();
//        $.ajax({
//            type: 'GET',
//            url: '@Url.Action("GetHouses")/' + id,
//            success: function (data) {
//                $('#HouseId').replaceWith(data);
//            },
//            error: function (result, status, er) {
//                alert("error: " + result + " status: " + status + " er:" + er);
//            }
//        });
//    });
//});

$(function () {
    $("#StreetId").change(function () {
        var formData = { 'IdStreet': Number.parseInt($('#StreetId').val()), 'Name': $('#StreetId>option:selected').text() };
        $.ajax({
            url: "http://localhost:18246/api/API/searchHouse",
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

                dataFilling(data, 'idHouse', 'name', '#HouseId', '<option/>');
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
        $.ajax({
            url: "http://localhost:18246/api/API/searchPollingStations/street",
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
                dataFilling(data, 'idPollingStation', 'name', '#PollingStationId', '<option/>');
            },
            error: function (result, status, er) {
                alert("error: " + result + " status: " + status + " er:" + er);
            }
        });
    });
});

// Обновление списка участков после выбора номера дома
$(function () {
    $('#HouseId').change(function () {
        var formData = { 'IdHouse': Number.parseInt($('#HouseId').val()), 'Name': $('#HouseId>option:selected').text() };
        $.ajax({
            url: "http://localhost:18246/api/API/searchPollingStations/house",
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
                dataFilling(data, 'idPollingStation', 'name', '#PollingStationId', '<option/>');
            },
            error: function (result, status, er) {
                alert("error: " + result + " status: " + status + " er:" + er);
            }
        });
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

        //$(document).ready(function () {
        //    $('.selectСhoice').select2({
        //        placeholder: "Выберите улицу",
        //        minimumInputLength: 1, // only start searching when the user has input 3 or more characters
        //        maximumInputLength: 15, // only allow terms up to 20 characters long
        //        language: "ru"
        //    });
        //});

        //$(document).ready(function () {
        //    $('.selectСhoice').select2({
        //        dropdownParent: $('#divHouse'),
        //        placeholder: "Выберите дом",
        //        minimumInputLength: 1, // only start searching when the user has input 3 or more characters
        //        maximumInputLength: 5, // only allow terms up to 20 characters long
        //        language: "ru"
        //    });
        //});