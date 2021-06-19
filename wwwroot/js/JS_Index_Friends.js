
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

$(document).ready(function () {
    $('#friendTable').DataTable({

        retrieve: true,
        paging: false,
        //ajax: '/ajax/arrays.txt',
        language: {
            url: "http://cdn.datatables.net/plug-ins/1.10.20/i18n/Russian.json"
        },

        initComplete: function () {
            this.api().columns([0, 3, 7, 9, 10, 11, 14, 15, 16, 17, 18, 19]).every(function () {
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