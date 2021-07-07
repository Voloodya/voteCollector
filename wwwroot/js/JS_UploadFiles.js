

async function UploadExcelToWebService(fileSource) {
    var data = await ExcelToJSON('Freinds', fileSource);
    data = await removePropertysJsonObjects(data, ['FamilyName','Name','PatronymicName','DateBirth','CityName','Street','House','Apartment','Telephone','DistrictName','PollingStationName','Organization','FieldActivityName','PhoneNumberResponsible','DateRegistrationSite','VotingDate','Vote','Description','Group']);

    $.ajax({
        type: "POST",
        //url: "http://10.1.48.66:80/uploadinmeta/api/FileApi/uploadDataSocContract",
        url: "http://localhost:18246/api/FileApi/uploadDataFromFile",
        headers:
        {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('#RequestVerificationToken').val()
        },
        processData: false,
        data: JSON.stringify(data),
        success: function (response) {
            dataFilling(response, '#Records');
        },
        error: function (result, status, er) {
            alert("error: " + result + " status: " + status + " er:" + er);
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
function dataFilling(data, idObject) {

    var objectHtml = $(idObject);
    objectHtml.empty();

    for (var i = 0; i < data.length; i++) {
        var opt = data[i];
        var el = document.createElement("option");
        el.textContent = opt;
        el.value = opt;
        objectHtml.appendChild(el);
    }
}