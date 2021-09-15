
let limitUpload = 100;
let partMyURL = "";
if (window.location.href.substring(0, 16) == "http://localhost") {
    partMyURL = "";
}
let numberColumnWhithVote = 19;
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
            url: "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Russian.json"
        },

        initComplete: function () {
            this.api().columns([7, 8, 9, 10]).every(function () {
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

// Upload all fieldActivity, all organization, all groups, all ElectoralDistricts
$(document).ready(function () {

    UploadAllFieldActivity();
    UploadAllOrganization();
    UploadAllGroups();
    UploadAllElectoralDistrict();
    CountAllVoters();
});

// Действия при перерисовки таблицы
$('#friendTable').on('draw.dt', function () {
//    countVoters('friendTable', 18);
});

function UpdateTablePlagin () {

    $('#friendTable').DataTable({

        retrieve: true,
        paging: false,
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Russian.json"
        },

        initComplete: function () {
            this.api().columns([7, 8, 9, 10]).every(function () {
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

// Загрузка списка организаций и обновление списка избирателей по FieldActivity
$(function () {
    $("#SelectFieldActivityId").change(function () {
        var idFielfActivity = Number.parseInt($('#SelectFieldActivityId').val());

        if (idFielfActivity != 0 && !Number.isNaN(idFielfActivity) && idFielfActivity !== undefined && idFielfActivity != null) {
            $.ajax({
                url: partMyURL + "/api/API/getorganization" + "/" + idFielfActivity,
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
                    DataFillingSelect(dataSort, 'idOrganization', 'name', 'SelectOrganizationId', '<option/>');

                    //setTimeout(GeneratingChangeEvent('SelectOrganizationId'), 1000);
                },
                error: function (result, status, er) {
                    alert("error: " + result + " status: " + status + " er:" + er);
                }
            });
            UpdateListVotersByFieldActivity(limitUpload);
        }
        else if (idFielfActivity == 0) {
            UpdateListVotersByFieldActivity(limitUpload);
            UploadAllOrganization();
        }
    });
});

// Загрузка списка групп и обновление списка избирателей по Organization
$(function () {
    $("#SelectOrganizationId").change(function () {
        var idorganization = Number.parseInt($('#SelectOrganizationId').val());

        if (idorganization !== 0 && !Number.isNaN(idorganization) && idorganization !== undefined && idorganization !== null) {
            $.ajax({
                url: partMyURL + "/api/API/getgroupsbyorganization" + "/" + idorganization,
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
                    DataFillingSelect(dataSort, 'idGroup', 'name', 'SelectGroupId', '<option/>');

                    //GeneratingChangeEvent('SelectGroupId');
                },
                error: function (result, status, er) {
                    alert("error: " + result + " status: " + status + " er:" + er);
                }
            });
            UpdateListVotersByOrganization(limitUpload);
        }
        else if (idorganization == 0) {
            UploadAllGroups();
            UpdateListVotersByFieldActivity(limitUpload);
        }
    });
});

// Обновление по группе
$(function () {
    $("#SelectGroupId").change(function () {

        let idGroup = Number.parseInt($('#SelectGroupId').val());
        if (idGroup == 0) {

            if (Number.parseInt($('#SelectOrganizationId').val()) == 0) {
                UpdateListVotersByFieldActivity(limitUpload);
            }
            else {
                UpdateListVotersByOrganization(limitUpload);
            }
        }
        else {
            UpdateListVotersByGroup(limitUpload);
        }

    });
});

function UploadAllFieldActivity() {

    $.ajax({
        // url: "http://localhost:18246/api/API/getElectoralDistrict",
        url: partMyURL + "/api/API/getfieldactivite",
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
            DataFillingSelect(dataSort, 'idFieldActivity', 'name', 'SelectFieldActivityId', '<option/>');

            //GeneratingChangeEvent('SelectFieldActivityId');
        },
        error: function (result, status, er) {
            alert("error: " + result + " status: " + status + " er:" + er);
        }
    });
}

function UploadAllOrganization() {
    $.ajax({
        url: partMyURL + "/api/API/getorganizationall",
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
            DataFillingSelect(dataSort, 'idOrganization', 'name', 'SelectOrganizationId', '<option/>');

            //setTimeout(GeneratingChangeEvent('SelectOrganizationId'), 1000);
        },
        error: function (result, status, er) {
            alert("error: " + result + " status: " + status + " er:" + er);
        }
    });
}

// all groups
function UploadAllGroups() {
    $.ajax({
        url: partMyURL + "/api/API/getgroupsall",
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
            DataFillingSelect(dataSort, 'idGroup', 'name', 'SelectGroupId', '<option/>');

        },
        error: function (result, status, er) {
            alert("error: " + result + " status: " + status + " er:" + er);
        }
    });
}

// Upload all ElectoralDistrict
function UploadAllElectoralDistrict() {

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

            //GeneratingChangeEvent('SelectElectoralDistrictId');
        },
        error: function (result, status, er) {
            alert("error: " + result + " status: " + status + " er:" + er);
        }
    });
}


let nrows = document.getElementById('friendTable').tBodies[0].rows.length;
document.getElementById('totalFriends').innerHTML = nrows;

$('friendTable').ready(CountVoters('friendTable', numberColumnWhithVote));
$('friendTable').change(CountVoters('friendTable', numberColumnWhithVote));

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
                UpdatingFields(idObject, 'numberRecords', 'totalFriends');
                CountVoters('friendTable', numberColumnWhithVote);
            },
            error: function (result, status, er) {
                alert("error: " + result + " status: " + status + " er:" + er);
            }
        });
    }
}

// Обновление списка избирателей по сфере деятельности
function UpdateListVotersByFieldActivity(limit) {

    if (limit != undefined) {
        var formData = {
            'IdFieldActivity': Number.parseInt($('#SelectFieldActivityId').val()), 'Name': $('#SelectFieldActivityId>option:selected').text(),
            LimitUpload: Number.parseInt(limit) };
    }
    else {
        var formData = {
            'IdFieldActivity': Number.parseInt($('#SelectFieldActivityId').val()), 'Name': $('#SelectFieldActivityId>option:selected').text()};
    }
    //$('#' + 'friendTable' + ' tbody > tr').remove();

    if (formData.IdFieldActivity != 0 && formData.IdFieldActivity !== "" && !Number.isNaN(formData.IdFieldActivity) && formData.IdFieldActivity !== undefined) {
        $.ajax({
            type: 'POST',
            //    url: '@Url.Action("GetPolingStationsByElectoralDistrict")',
            url: '/Admin/SearchFriendsByFieldActivity/',
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
                //friendDataTable.rows.add($(data)).draw();
                //friendDataTable.Empty();
                $('#friendTable').empty();
                // Destroy object DataTable
                friendDataTable.destroy();

                // Update tbody new rows with data
                $('#friendTable').replaceWith(data);
                UpdatingFields('friendTable', 'numberRecords', 'totalFriends');

                UpdateTablePlagin();
                CountVoters('friendTable', numberColumnWhithVote);

                OnOffButton('buttonUploadAll', true);
            },
            error: function (result, status, er) {
                alert('error: ' + result + ' status: ' + status + ' er:' + er);
            }
        });

        CountVotersByFieldActivity(formData);
    }
    else {
        $.ajax({
            type: 'POST',
            //    url: '@Url.Action("GetPolingStationsByElectoralDistrict")',
            url: '/Admin/SearchFriendsByAllFieldActivity/',
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
                //friendDataTable.rows.add($(data)).draw();
                //friendDataTable.Empty();
                $('#friendTable').empty();
                // Destroy object DataTable
                friendDataTable.destroy();

                // Update tbody new rows with data
                $('#friendTable').replaceWith(data);
                UpdatingFields('friendTable', 'numberRecords', 'totalFriends');

                UpdateTablePlagin();
                CountVoters('friendTable', numberColumnWhithVote);
            },
            error: function (result, status, er) {
                alert('error: ' + result + ' status: ' + status + ' er:' + er);
            }
        });

        CountAllVoters();
    }
}

// Обновление списка избирателей по организации
function UpdateListVotersByOrganization(limit) {

    if (limit == undefined) {
        var formData = { 'IdOrganization': Number.parseInt($('#SelectOrganizationId').val()), 'Name': $('#SelectOrganizationId>option:selected').text() };
    }
    else {
        var formData = {
            'IdOrganization': Number.parseInt($('#SelectOrganizationId').val()), 'Name': $('#SelectOrganizationId>option:selected').text(),
            LimitUpload: Number.parseInt(limit)};
    }

    if (formData.IdOrganization != 0 && formData.IdOrganization !== "" && !Number.isNaN(formData.IdOrganization) && formData.IdOrganization !== undefined) {
        $.ajax({
            type: 'POST',
            //    url: '@Url.Action("GetPolingStationsByElectoralDistrict")',
            url: '/Admin/SearchFriendsByOrganization/',
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
                //friendDataTable.rows.add($(data)).draw();
                //friendDataTable.Empty();
                $('#friendTable').empty();
                // Destroy object DataTable
                friendDataTable.destroy();

                // Update tbody new rows with data
                $('#friendTable').replaceWith(data);
                UpdatingFields('friendTable', 'numberRecords', 'totalFriends');

                UpdateTablePlagin();
                CountVoters('friendTable', numberColumnWhithVote);

                OnOffButton('buttonUploadAll', true);
            },
            error: function (result, status, er) {
                alert('error: ' + result + ' status: ' + status + ' er:' + er);
            }
        });
        CountVotersByOrganization(formData);
    }
    else {
        UpdateListVotersByFieldActivity(limit);
    }
   
}

// Обновление списка избирателей по группе
function UpdateListVotersByGroup(limit) {

    if (limit == undefined) {
        var formData = { 'IdGroup': Number.parseInt($('#SelectGroupId').val()), 'Name': $('#SelectGroupId>option:selected').text() };
    }
    else {
        var formData = {
            'IdGroup': Number.parseInt($('#SelectGroupId').val()), 'Name': $('#SelectGroupId>option:selected').text(),
            LimitUpload: Number.parseInt(limit)};
    }

    if (formData.IdGroup != 0 && formData.IdGroup !== "" && !Number.isNaN(formData.IdGroup) && formData.IdGroup !== undefined) {
        $.ajax({
            type: 'POST',
            //    url: '@Url.Action("GetPolingStationsByElectoralDistrict")',
            url: '/Admin/SearchFriendsByGroup/',
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
                //friendDataTable.rows.add($(data)).draw();
                //friendDataTable.Empty();
                $('#friendTable').empty();
                // Destroy object DataTable
                friendDataTable.destroy();

                // Update tbody new rows with data
                $('#friendTable').replaceWith(data);
                UpdatingFields('friendTable', 'numberRecords', 'totalFriends');

                UpdateTablePlagin();
                CountVoters('friendTable', numberColumnWhithVote);

                OnOffButton('buttonUploadAll', true);
            },
            error: function (result, status, er) {
                alert('error: ' + result + ' status: ' + status + ' er:' + er);
            }
        });
        CountVotersByGroup(formData);
    }
    else {
        UpdateListVotersByOrganization(limit);
    }
}

// Подсчет списка избирателей и проголосовавшим со всеми фильтрами "Все"
function CountAllVoters() {

        $.ajax({
            type: 'POST',
            url: '/api/APIFriends/CountAllFriends/',
            headers:
            {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('#RequestVerificationToken').val()
            },
            data: JSON.stringify(),
            success: function (data) {

                document.getElementById('totalFriendsServ').innerHTML = data.numberFriends;
                document.getElementById('totalVoteServ').innerHTML = data.numberVoter;

            },
            error: function (result, status, er) {
                alert('error: ' + result + ' status: ' + status + ' er:' + er);
            }
        });
}

// Подсчет списка избирателей и проголосовавших по сфере деятельности
function CountVotersByFieldActivity(formDataFieldActivity) {

    if (formDataFieldActivity != undefined) {
        $.ajax({
            type: 'POST',
            url: '/api/APIFriends/CountFriendsByFieldActivity/',
            headers:
            {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('#RequestVerificationToken').val()
            },
            data: JSON.stringify(formDataFieldActivity),
            success: function (data) {

                document.getElementById('totalFriendsServ').innerHTML = data.numberFriends;
                document.getElementById('totalVoteServ').innerHTML = data.numberVoter;

            },
            error: function (result, status, er) {
                alert('error: ' + result + ' status: ' + status + ' er:' + er);
            }
        });
    }
}

// Подсчет списка избирателей и проголосовавших по организации
function CountVotersByOrganization(formDataOrganization) {

    if (formDataOrganization != undefined) {
        $.ajax({
            type: 'POST',
            url: '/api/APIFriends/CountFriendsByOrganization/',
            headers:
            {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('#RequestVerificationToken').val()
            },
            data: JSON.stringify(formDataOrganization),
            success: function (data) {

                document.getElementById('totalFriendsServ').innerHTML = data.numberFriends;
                document.getElementById('totalVoteServ').innerHTML = data.numberVoter;
            },
            error: function (result, status, er) {
                alert('error: ' + result + ' status: ' + status + ' er:' + er);
            }
        });
    }
}

// Подсчет списка избирателей и проголосовавших по группе
function CountVotersByGroup(formDataGroup) {

    if (formDataGroup != undefined) {
        $.ajax({
            type: 'POST',
            url: '/api/APIFriends/CountFriendsByGroup/',
            headers:
            {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('#RequestVerificationToken').val()
            },
            data: JSON.stringify(formDataGroup),
            success: function (data) {

                document.getElementById('totalFriendsServ').innerHTML = data.numberFriends;
                document.getElementById('totalVoteServ').innerHTML = data.numberVoter;

            },
            error: function (result, status, er) {
                alert('error: ' + result + ' status: ' + status + ' er:' + er);
            }
        });
    }
}

function UpdateVotersFullFilters() {

    //// '/Admin/SearchFriendsByFieldActivity/'
    //UpdateListVotersByFieldActivity();

    //// '/Admin/SearchFriendsByOrganization/'
    //UpdateListVotersByOrganization();

    // '/Admin/SearchFriendsByGroup/'    
    UpdateListVotersByGroup();

    OnOffButton('buttonUploadAll', false);
}

function GetNumbersVoters() {


}

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

function UpdatingFields(idObjectSelect, idObjectUpdate, idObjectUpdate2) {
    let nrows = document.getElementById(idObjectSelect).tBodies[0].rows.length;

    if (idObjectUpdate2 != undefined) {
        document.getElementById(idObjectUpdate2).innerHTML = nrows;
    }
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

function CountVoters(idObjectCount, numberColumn) {
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

// Генерация события для элемента Select
function GeneratingChangeEvent(idOdj) {
    
    let elemSelectFieldActivity = document.querySelector('#' + idOdj);
    elemSelectFieldActivity.selectedIndex = 0;
    const event = new Event("change");
    elemSelectFieldActivity.dispatchEvent(event);
}

// Обновление списка избирателей по избирательному округу
$(function () {
    $("#SelectElectoralDistrictId").change(function () {
        var formData = { 'IdElectoralDistrict': Number.parseInt($('#SelectElectoralDistrictId').val()), 'Name': $('#SelectElectoralDistrictId>option:selected').text() };
        //$('#' + 'friendTable' + ' tbody > tr').remove();

        if (formData['IdElectoralDistrict'] !== "") {
            $.ajax({
                type: 'POST',
                //    url: '@Url.Action("GetPolingStationsByElectoralDistrict")',
                url: '/Admin/SearchFriendsByElectoralDistrict/',
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
                    //friendDataTable.rows.add($(data)).draw();
                    //friendDataTable.Empty();
                    $('#friendTable').empty();
                    // Destroy object DataTable
                    friendDataTable.destroy();

                    // Update tbody new rows with data
                    $('#friendTable').replaceWith(data);
                    UpdatingFields('friendTable', 'numberRecords', 'totalFriends');

                    UpdateTablePlagin();
                    CountVoters('friendTable', numberColumnWhithVote);
                },
                error: function (result, status, er) {
                    alert('error: ' + result + ' status: ' + status + ' er:' + er);
                }
            });
        }
    });
});

// Включение отключение кнопки
function OnOffButton(idButton, onOff) {

    document.getElementById(idButton).disabled = !onOff;
}

function CreatePDFfileForUser(idObj) {

    $.ajax({
        url: partMyURL + "/api/APIFriends/GetFriend" + "/" + idObj,
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

                CreatePDFfile(data['name'], data['familyName'], data['telephone'], data['textQRcode'], data['qrCodeImageAsBase64']);
            }
        },
        error: function (result, status, er) {
            alert("error: " + result + " status: " + status + " er:" + er);
        }
    });
}

// Создание PDF
function CreatePDFfile(name, familyName, phoneNumber, textQRcode, imageDataString) {

    var doc = new jsPDF({
        //   orientation: "landscape",
        //    format: [4, 2]
    });
    var image = 'data:image/png;base64,' + imageDataString;
    doc.addFont();

    doc.text(10, 40, textQRcode);
    doc.addImage(image, 'png', 35, 20, 40, 40,);
    //doc.text(10, 90, 'НОМЕР ТЕЛЕФОНА:');
    doc.text(10, 80, phoneNumber);
    //doc.addPage();
    //doc.text(20, 20, 'Do you like that ' + idObj +'?');

    doc.save('QR-code_' + name + '_' + familyName + '.pdf');
    return false;
}

