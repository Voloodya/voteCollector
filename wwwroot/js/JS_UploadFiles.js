
let partMyURL = '';
if (window.location.href.substring(0, 16) == "http://localhost") {
    partMyURL = '';
}

async function UploadExcelToWebService(fileSource) {
    var data = await ExcelToJSON('Freinds', fileSource);
    data = await removePropertysJsonObjects(data, ['FamilyName', 'Name', 'PatronymicName', 'DateBirth', 'Unpinning', 'City', 'CityDistrict', 'Street', 'House', 'Apartment', 'Telephone', 'ElectiralDistrict', 'PollingStationName', 'Organization', 'FieldActivityName', 'PhoneNumberResponsible', 'DateRegistrationSite', 'VotingDate', 'Vote', 'TextQRcode', 'Email', 'Description', 'Group', 'LoginUsers','Adress']);

    $.ajax({
        type: 'POST',
        url: partMyURL+ '/api/FileApi/uploadDataFromFile',
        //url: "/CollectVoters/api/FileApi/uploadDataFromFile",
        headers:
        {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('#RequestVerificationToken').val()
        },
        processData: false,
        data: JSON.stringify(data),
        success: function (data) {

            //if (data != undefined) {
            //    data.sort();
            //}
            //DataFillingDiv(response, 'outText');
            DataFillingTableBody(data, 'fileUploadTable');
        },
        error: function (result, status, er) {
            alert('error: ' + result + ' status: ' + status + ' er:' + er);
        }
    });
}

async function UploadExcelToWebServiceMVC(fileSource) {
    var data = await ExcelToJSON('Freinds', fileSource);
    data = await removePropertysJsonObjects(data, ['FamilyName', 'Name', 'PatronymicName', 'DateBirth', 'Unpinning', 'City', 'CityDistrict', 'Street', 'House', 'Apartment', 'Telephone', 'ElectiralDistrict', 'PollingStationName', 'Organization', 'FieldActivityName', 'PhoneNumberResponsible', 'DateRegistrationSite', 'VotingDate', 'Vote', 'TextQRcode', 'Email', 'Description', 'Group', 'LoginUsers','Adress']);

    $.ajax({
        type: 'POST',
        url: partMyURL + '/UploadFiles/uploadDataFromFile',
        //url: "/CollectVoters/api/FileApi/uploadDataFromFile",
        headers:
        {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('#RequestVerificationToken').val()
        },
        processData: false,
        data: JSON.stringify(data),
        success: function (data) {

            //if (data != undefined) {
            //    data.sort();
            //}
            //DataFillingDiv(response, 'outText');
            DataFillingTableBody(data, 'fileUploadTable');
        },
        error: function (result, status, er) {
            alert('error: ' + result + ' status: ' + status + ' er:' + er);
        }
    });
}


//Функция конвертации файла в json
var ExcelToJSON = function (sheetNameRead, fileSource) {

    // Get The File From The Input
    var oFile = document.getElementById(fileSource).files[0];
    //var oFile = oFileIn.target.files[0];
    var sFilename = oFile.name;
    // Create A File Reader HTML5
    var reader = new FileReader();

    var json_objects = [];
    return new Promise(resolve => {
        // Ready The Event For When A File Gets Selected
        reader.onload = function (event) {
            var data = event.target.result;
            var cfb = XLSX.read(data, { type: 'binary' });
            // Loop Over Each Sheet
            cfb.SheetNames.forEach(function (sheetName) {

                if (sheetName == sheetNameRead) {
                    // Obtain The Current Row As CSV
                    var sCSV = XLS.utils.make_csv(cfb.Sheets[sheetName]);
                    var json_object = XLS.utils.sheet_to_json(cfb.Sheets[sheetName]);

                    json_objects.push(json_object);
                }
            });
            if (json_objects != undefined & json_objects.length > 0) {
                json_objects = json_objects[0];
            }
            resolve(json_objects);
        };

        // Tell JS To Start Reading The File.. You could delay this if desired
        reader.readAsBinaryString(oFile);
    });
}

//Удаление лишних полей из json-объектов
function removePropertysJsonObjects(jsonObjects, neededProperties) {

    return new Promise(resolve => {
        let result = jsonObjects.map(
            item => Object.fromEntries(
                Object.entries(item).filter(
                    ([key, value]) => neededProperties.includes(key)
                )
            )
        );
        resolve(result);
    });
}

//Заполнение объекта html данными из json массива
function DataFillingSelect(data, idObject, propertyHtml) {

    var objectHtml = $(idObject);
    objectHtml.empty();

    for (var i = 0; i < data.length; i++) {
        objectHtml.append($(propertyHtml,
            {
                value: data[i],
                text: data[i]
            }));
    }
}

function DataFillingDiv(data, idObject) {

    var divhtml = document.getElementById(idObject);
    divhtml.innerHTML = JSON.stringify(data);
}

function RequestUploadImage(fileSource) {
    var data = new FormData();
    var files = $('#' + fileSource).get(0).files;
    // Add the uploaded image content to the form data collection
    if (files.length > 0) {
        data.append("UploadedFile", files[0]);
    }

    data.append("infoFile","No info");  //Other data
    $.ajax({
        type: 'POST',
        url: partMyURL + '/api/FileApi/uploadFileQRCode',
        //url: "/CollectVoters/api/FileApi/uploadDataFromFile",
        contentType: false,
        processData: false,
        data: data,
        success: function (response) {
            alert('File uploaded');
            console.log(response);
        },
        error: function (result, status, er) {
            alert('error: ' + result + ' status: ' + status + ' er:' + er);
        }
    });
};

function DataFillingTableBody(data, idObjectTable) {

    //var table = document.getElementById(idObjectTable);
    var bodyTable = $(`#${idObjectTable} tbody`);
    //var table = $(idObjectTable);
    //$(idObjectTable + ' tbody').empty();
    $('#' + idObjectTable + ' tbody > tr').remove();

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