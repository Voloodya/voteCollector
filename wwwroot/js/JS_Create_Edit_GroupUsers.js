
$(document).ready(function () {
    $('#groupSelectId').select2({
        dropdownParent: $('#divGroiupId'),
        placeholder: "Выберите группу",
        minimumInputLength: 2, // only start searching when the user has input 2 or more characters
        maximumInputLength: 15, // only allow terms up to 15 characters long
        language: "ru"
    });
});

$(document).ready(function () {
    $('#userSelectId').select2({
        dropdownParent: $('#divUserId'),
        placeholder: "Выберите пользователя",
        minimumInputLength: 2, // only start searching when the user has input 2 or more characters
        maximumInputLength: 15, // only allow terms up to 15 characters long
        language: "ru"
    });
});