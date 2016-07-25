$(document).ready(function () {
    //Populate Contact
});

function LoadContacts() {
    //The html() method sets or returns the content (innerHTML) of the selected elements.
    //When this method is used to set content, it overwrites the content of ALL matched elements.
    $('#update_panel').html('Loading Data...');

    $.ajax({
        //Executing to our controller action 'GetContacts' to get the data that action returns
        url: '/home/Test',
        type: 'GET',
        dataType: 'json',
        success: function (d) {
            //if byte/text length > 0
            if (d.length > 0) {
                //$data = html table element
                var $data = $('<table></table>').addClass('table table-responsive table-striped');
                //header = html table header element
                var header = "<thead><tr><th>Scored Labels</th><th>Scored Probabilities</th></tr></thead>";
                 
                $data.append(header);
                
                //iterator function, which can be used to seamlessly iterate over both objects and arrays.
                $.each(JSON.parse(d), function (i, row) {
                    
                    var $row = $('<tr/>');
                     
                    //Json Query
                    var score = (JSON.parse(d).Results.output1.value.Values[0]);

                    var myArray = score;
                    $row.append($('<td/>').html(myArray[0]));
                    $row.append($('<td/>').html(myArray[1]));

                    //Add row to data (table)
                    $data.append($row);
                });

                $('#update_panel').html($data);
            }
            else {
                //If no data is found then update the "update-panel" div with text 'No Data Found'
                var $noData = $('<div/>').html('No Data Found!');
                $('#update_panel').html($noData);
            }
        },
        error: function () {
            alert('Error! Please try again.');
        }
    });

}
