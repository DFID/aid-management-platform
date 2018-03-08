//////////////////////////////////////////////////////////////////////////
//EditTeamMember - Function that loads the EditTeamMember Partial View 
//from the Team page.
//////////////////////////////////////////////////////////////////////////
function EditTeamMember(idObj) {

    $.ajax({
        type: 'GET',
        url: @Url.Content("../EditTeamMember/")",
        data: {
        Id: idObj //Data need to pass as parameter                       
        },
    dataType: 'html', //dataType - html
    success: function(result) {
        //alert(result);
        //Create a Div around the Partial View and fill the result
        $('#EditTeamMember').html(result);
    }
});

$("#EditTeamMember").toggle();
}

//////////////////////////////////////////////////////////////////////////
//CancelEdit - Toggles the div containing the EditTeamMember Partial View. 
//Based on how the page flows, this should always close the Partial View.
//////////////////////////////////////////////////////////////////////////

function CancelEdit() {
    $("#EditTeamMember").toggle();
}
