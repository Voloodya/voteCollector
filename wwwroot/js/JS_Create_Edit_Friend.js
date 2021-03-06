
let partMyURL = "";
if (window.location.href.substring(0, 16) == "http://localhost") {
    partMyURL = "";
}

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

//Установка текущей даты
function stateDate(idObject) {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    today = yyyy + '-' + mm + '-' + dd;
    document.getElementById(idObject).value = today;
}

$(document).ready(function () {

    // Устанавливаем статус Откреплен/неоткреплен пришедший с сервера во фронт checkbox
    chboxFront = document.getElementById('boolUnpinning');
    chboxServ = document.getElementById('boolUnpinningServ');
    chboxFront.checked = chboxServ.checked;

    // Скрываем/отображаем блоки согласно статуса Откреплен/неоткреплен
    HideShowBlocks('boolUnpinning', 'divCityDistrict', 'divStreet', 'divHouse', 'divApartment', 'divAdessText');

//    $(function () {
//        $("#phoneNumber").mask("+7(999) 999-9999");
//    });
});

// Обновление списка городских округов после выбора города
// Проверка откреплен/неоткреплен. Установка галочки, обновление списка участков
$(function () {
    $('#CityId').change(function () {

        var idCity = Number.parseInt($('#CityId').val());

        if (idCity !== 0 && !Number.isNaN(idCity) && idCity !== undefined && idCity !== null) {
            $.ajax({
                url: partMyURL + "/api/API/GetSitiesDistricts" + "/" + idCity,
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('#RequestVerificationToken').val()
                },
                type: 'GET',
                dataType: "json",
                success: function (data) {
                    var dataSort = []
                    if (data != undefined) {
                        dataSort = data.sort(function (a, b) {
                            return ((a.name === b.name) ? 0 : ((a.name > b.name) ? 1 : -1));
                        });
                    }
                    DataFillingSelect(dataSort, 'idCity', 'name', '#CityDistrictId', '<option/>');

                    // Генерация события для элемента Select
                    let elemSelectCity = document.querySelector('#CityDistrictId')
                    elemSelectCity.selectedIndex = 0;
                    const event = new Event("change");
                    elemSelectCity.dispatchEvent(event);

                    UpdateStateFieldsAndStationAndDistricts();
                },
                error: function (result, status, er) {
                    alert("error: " + result + " status: " + status + " er:" + er);
                }
            });
        }        
    });
});

function UpdateStateFieldsAndStationAndDistricts() {

    var selectCity = $('#CityId>option:selected').text();

    if (selectCity !== 'Оренбург') {

        //document.getElementById('boolUnpinning').checked = true;

        //document.getElementById('boolUnpinning').disabled = true;

        HideShowBlocks('boolUnpinning', 'divCityDistrict', 'divStreet', 'divHouse', 'divApartment', 'divAdessText');
        DisplayInputField('divAdessText', true);

        // Синхронизация
        chboxFront = document.getElementById('boolUnpinning');
        chboxServ = document.getElementById('boolUnpinningServ');
        chboxFront.checked = chboxServ.checked;

        if (chboxServ.checked) {

            UpdateStationByCityAfterSelect();
            UploadElectoralDistrictsAll();
        }
    }
    else if (selectCity === 'Оренбург') {

        // Генерация события для элемента Select
        //let elemSelectCityDistrict = document.querySelector('#CityDistrictId')
        //elemSelectCityDistrict.selectedIndex = 0;
        //const event = new Event("change");
        //elemSelectCityDistrict.dispatchEvent(event);

        UpdateStationByCityAfterSelect();
        UploadElectoralDistrictsAll();

        //document.getElementById('boolUnpinning').checked = false;

        //document.getElementById('boolUnpinning').disabled = false;

        ShowBlocks('boolUnpinning', 'divCityDistrict', 'divStreet', 'divHouse', 'divApartment', 'divAdessText');
        DisplayInputField('divAdessText', false);
    }
}

// Обновление списка улиц после выбора городского округа
//$(function () {
//    $("#CityDistrictId").change(function () {
//        var formData = { 'IdCity': Number.parseInt($('#CityDistrictId').val()), 'Name': $('#CityDistrictId>option:selected').text() };
//        $.ajax({
//           // url: "http://localhost:18246/api/API/searchStreets",
//            url: partMyURL+"/api/API/searchStreets",
//           // url: "/CollectVoters/api/API/searchStreets",
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
//                var dataSort = [];
//                if (data != undefined) {
//                    dataSort = data.sort(function (a, b) {
//                    return ((a.name === b.name) ? 0 : ((a.name > b.name) ? 1 : -1));
//                    });
//                }
//                DataFillingSelect(dataSort, 'idStreet', 'name', '#StreetId', '<option/>');

//                // Генерация события для элемента Select
//                let elemSelectStreet = document.querySelector('#StreetId')
//                elemSelectStreet.selectedIndex = 0;
//                const event = new Event("change");
//                elemSelectStreet.dispatchEvent(event);
//            },
//            error: function (result, status, er) {
//                alert("error: " + result + " status: " + status + " er:" + er);
//            }
//        });
//    });    
//});

// Обновление списка домов после выбора улицы
$(function () {
    $("#StreetId").change(function () {
        var formData = { 'IdStreet': Number.parseInt($('#StreetId').val()), 'Name': $('#StreetId>option:selected').text() };

        if (!Number.isNaN(formData['IdStreet'])) {
            $.ajax({
                url: partMyURL + "/api/API/searchHouse",
                //url: "/CollectVoters/api/API/searchHouse",
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
                    DataFillingSelect(dataSort, 'idHouse', 'name', '#HouseId', '<option/>');
                },
                error: function (result, status, er) {
                    alert("error: " + result + " status: " + status + " er:" + er);
                }
            });
        }
        else {
            var objectHtml = $('#HouseId');
            objectHtml.empty();
        }
    });
});


// Обновление списка групп после выбора организации
$(function () {
    $('#organizationSelectId').change(function () {

        var idorganization = Number.parseInt($('#organizationSelectId').val());

        if (idorganization !== 0 && !Number.isNaN(idorganization) && idorganization !== undefined && idorganization !== null) {
            $.ajax({
                url: partMyURL + "/api/API/GetGroupsMaxLvlByOrganization" + "/" + idorganization,
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('#RequestVerificationToken').val()
                },
                type: 'GET',
                dataType: "json",
                success: function (data) {
                    var dataSort = []
                    if (data != undefined) {
                            dataSort = data.sort(function (a, b) {
                            return ((a.name === b.name) ? 0 : ((a.name > b.name) ? 1 : -1));
                        });
                    }
                    DataFillingSelect(dataSort, 'idGroup', 'name', '#SelectGroupId', '<option/>');

                },
                error: function (result, status, er) {
                    alert("error: " + result + " status: " + status + " er:" + er);
                }
            });
        }
    });
});

//Заполнение объекта html данными из json массива
function DataFillingSelect(data, nameProperty1, nameProperty2, idObject, propertyHtml) {

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

        //$(document).ready(function () {
        //    $('#HouseId').select2({
        //        dropdownParent: $('#divHouse'),
        //        placeholder: "Выберите дом",
        //        minimumInputLength: 1, // only start searching when the user has input 3 or more characters
        //        maximumInputLength: 5, // only allow terms up to 20 characters long
        //        language: "ru"
        //    });
        //});

$(function () {
    $("#boolUnpinning").change(function () {

        // Синхронизируем статусы Откреплен/неоткреплен серверного и пользовательского елемента
        chboxFront = document.getElementById('boolUnpinning');
        chboxServ = document.getElementById('boolUnpinningServ');
        chboxServ.checked = chboxFront.checked;

        if (chboxFront.checked) {
            UpdateStationByCityAfterSelect();
            UploadElectoralDistrictsAll();
        }
    });
});

function HideShowBlocks(idchbox, idObjectHidden1, idObjectHidden2, idObjectHidden3, idObjectHidden4, idObjectVisible)
{  
    var chbox = document.getElementById(idchbox);

    // Устанавливаем статус в серверный checkbox Откреплен/Неоткреплен из фронтального checkbox
    var chboxServ = document.getElementById('boolUnpinningServ');
    //chboxServ.checked = chbox.checked;

    // Если "Откреплен"
    if (chbox.checked) {

        // Не Оренбург
        if ($('#CityId>option:selected').text() !== 'Оренбург') {

            HideBlocks(idObjectHidden1, idObjectHidden2, idObjectHidden3, idObjectHidden4, idObjectVisible);
            DisplayInputField(idObjectVisible, true);
        }
        else { // Оренбург          

            ShowBlocks(idObjectHidden1, idObjectHidden2, idObjectHidden3, idObjectHidden4, idObjectVisible);
            DisplayInputField(idObjectVisible, false);
        }

    }
    else { // Если "Неоткреплен"

        // Не Оренбург
        if ($('#CityId>option:selected').text() !== 'Оренбург') {

            HideBlocks(idObjectHidden1, idObjectHidden2, idObjectHidden3, idObjectHidden4, idObjectVisible);
            DisplayInputField(idObjectVisible, true);
            // Поля участок и округ "Пустые"
            var selectStation = $('#StationId');
            var selectElectoralDistrict = $('#ElectoralDistrictId');
            selectStation.empty();
            selectElectoralDistrict.empty();
        }
        else { // Оренбург

            ShowBlocks(idObjectHidden1, idObjectHidden2, idObjectHidden3, idObjectHidden4, idObjectVisible);
            DisplayInputField(idObjectVisible, false);
            UploadStationByCitydistrictAndStreetAndHouse();
        }
    }

//    if ($('#CityId>option:selected').text() !== 'Оренбург') {

//        document.getElementById('boolUnpinning').checked = true;
//        document.getElementById('boolUnpinning').disabled = true;

//        if (chbox.checked) {

//            hideObj1.style.display = 'none';
//            hideObj2.style.display = 'none';
//            hideObj3.style.display = 'none';
//            hideObj4.style.display = 'none';
//            showObj.style.display = 'block';

//            UpdateStationByCityAfterSelect();
//            UploadElectoralDistrictsAll();
//        }
//        else {

//            hideObj1.style.display = 'block';
//            hideObj2.style.display = 'block';
//            hideObj3.style.display = 'block';
//            hideObj4.style.display = 'block';
//            showObj.style.display = 'none';

//            UpdateStationByCityAfterSelect();            
//        }
//    }
//    else {

//        if (chbox.checked) {

//            UpdateStationByCityAfterSelect();
//            UploadElectoralDistrictsAll();
//            ShowBlocks('boolUnpinning', 'divCityDistrict', 'divStreet', 'divHouse', 'divApartment', 'divAdessText');

//        }
//        else {

//            UploadStationByCitydistrictAndStreetAndHouse();
//        }
//    }
}


function ShowBlocks(idObjectHidden1, idObjectHidden2, idObjectHidden3, idObjectHidden4) {

    let hideObj1 = document.getElementById(idObjectHidden1);
    let hideObj2 = document.getElementById(idObjectHidden2);
    let hideObj3 = document.getElementById(idObjectHidden3);
    let hideObj4 = document.getElementById(idObjectHidden4);
    

        hideObj1.style.display = 'block';
        hideObj2.style.display = 'block';
        hideObj3.style.display = 'block';
        hideObj4.style.display = 'block';
}

function HideBlocks(idObjectHidden1, idObjectHidden2, idObjectHidden3, idObjectHidden4) {

    let hideObj1 = document.getElementById(idObjectHidden1);
    let hideObj2 = document.getElementById(idObjectHidden2);
    let hideObj3 = document.getElementById(idObjectHidden3);
    let hideObj4 = document.getElementById(idObjectHidden4);

    hideObj1.style.display = 'none';
    hideObj2.style.display = 'none';
    hideObj3.style.display = 'none';
    hideObj4.style.display = 'none';

}

function DisplayInputField(idObjectVisible, display) {

    let showObj = document.getElementById(idObjectVisible);

    if (display) {
        showObj.style.display = 'block';
    }
    else {
        showObj.style.display = 'none';
    }

}

// Проверка на выбор значений select улицы и дома
function ValidateSelect(idObjSelect1, idObjSelect2) {

    if ($('#CityId>option:selected').text() === 'Оренбург') {
        var selectStreet = document.getElementById(idObjSelect1);
        var selectHouse = document.getElementById(idObjSelect2);
        var selectedStreetValue = selectStreet.options[selectStreet.selectedIndex].value;
        var selectedHouseValue = selectHouse.options[selectHouse.selectedIndex].value;
        if (selectedStreetValue !== '' && selectedStreetValue !== ' ' && selectedHouseValue !== '' && selectedHouseValue !== ' ') {
            return true;
        }
        else return false;
    }
}