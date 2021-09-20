
let partMyURL = '';
if (window.location.href.substring(0, 16) == "http://localhost") {
    partMyURL = '';
}

async function RequestQRcodes(idObjectDate, idObjectTime, idObjectDateTo, idObjectTimeTo) {

    var date = document.getElementById(idObjectDate).value;
    var time = document.getElementById(idObjectTime).value;

    var dateTo = document.getElementById(idObjectDateTo).value;
    var timeTo = document.getElementById(idObjectTimeTo).value;

    var formData = { 'date': date, 'time': time, 'dateTo': dateTo, 'timeTo': timeTo};

    $.ajax({
        type: 'POST',
        url: partMyURL + '/api/QRcodeСheckAPI/requestqrcodesreceiving',
        headers:
        {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('#RequestVerificationToken').val()
        },
        processData: false,
        data: JSON.stringify(formData),
        success: function (data) {

            if (data['notFoundQRcodes'] != null && data['notFoundQRcodes'] !== undefined) {
                DataFillingTableBody(data['notFoundQRcodes'], 'qrCodesNotFoundTable');
            }
            document.getElementById('idDivMessage').innerHTML = "<h3>" + " Загрузка завершена!" + "</h3>";
            alert("Загрузка завершена!");
            document.getElementById('idDivStatus').innerHTML = "<h4> Статус запроса: " + data['status'] + "</h4>";
            document.getElementById('idDivError').innerHTML = "<h4> Ошибки: " + data['error'] + "</h4>";
            document.getElementById('idDivNumberReceivedCodes').innerHTML = "<h4> Получено QR-кодов: " + data['numberReceivedCodes'] + "</h4>";
            document.getElementById('idDivNumberMarkedCodes').innerHTML = "<h4> Проверено QR-кодов: " + data['numberMarkedCodes'] + "</h4>";
            document.getElementById('idDivNumberNotFound').innerHTML = "<h4> Ненайдено QR-кодов: " + data['numberNotFound'] + "</h4>";
            document.getElementById('idDivDateTimeRequest').innerHTML = "<h4> Время запроса: " + data['dateTimeRequest'] + "</h4>";

            if (data['foundQRcodes'] != null && data['foundQRcodes'] !== undefined) {
                DataFillingTableBody(data['foundQRcodes'], 'qrCodesFoundTable');
            }
        },
        error: function (result, status, er) {
            alert('error: ' + result + ' status: ' + status + ' er:' + er);
        }
    });
}

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