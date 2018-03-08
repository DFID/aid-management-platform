$(document).ready(function () {

    //////////////////////////////////////////////////////////////////////////
    //Show/Hide Save Message
    //////////////////////////////////////////////////////////////////////////

    var SaveSuccess = $("#SaveSuccess");
    var SaveMessage = $("#SaveMessage");

    if (SaveSuccess.val() == "1") {
        SaveMessage.toggleClass("SaveMessageSuccess");
        SaveMessage.toggle();
    }

    if (SaveSuccess.val() == "0") {
        SaveMessage.toggleClass("SaveMessageFail");
        SaveMessage.html("Your Changes have not been saved.");
        SaveMessage.toggle();
    }


    /////////////////////////////////////////////////////////////////////
    //_ReviewOutputScoring.cshtml page 
    ////////////////////////////////////////////////////////////////////

    $('.review-output-btn-click-action').click(function (e) {
        e.preventDefault();
        var reviewId = $(this).attr("data-reviewid");
        var projectId = $(this).attr("data-projectid");
        var thisTableSuffix = $(this).attr("data-table-suffix");

        //Hide info message
        $("#outputscoringSaveMessageFail-" + projectId + "-" + reviewId).hide();
        $("#outputscoringSaveMessageSuccess-" + projectId + "-" + reviewId).hide();
        //client side validation check
        if ($(this).closest('form').valid()) {
            this.disabled = true;
            $(".content-loading").show();
            //PostOutputData - Cab be found on _ReviewOutputScoring.cshtml page 
            PostOutputData($(this).closest('form'), reviewId, projectId, thisTableSuffix, $(this));
        }
    });
});







/////////////////////////////////////////////////////////////////////
//Character count
////////////////////////////////////////////////////////////////////

function countChar(source, controlId, characterLimit) {
    var chars = source.value.length;
    if (chars > characterLimit) {
        source.value = source.value.substring(0, characterLimit);
        chars = characterLimit;
    } 
    $('#' + controlId).text(characterLimit - chars + ' of ' + characterLimit + ' characters left');
};

/////////////////////////////////////////////////////////////////////
//Is variable an Integer Number
////////////////////////////////////////////////////////////////////

function isInt(value) {
    var x;
    if (isNaN(value)) {
        return false;
    }
    x = parseFloat(value);
    return (x | 0) === x;
}

/////////////////////////////////////////////////////////////////////
//Check for Duplicate Inputs by Class 
////////////////////////////////////////////////////////////////////

function checkDuplicatesInputsByClassName(className) {
    // get all input elements
    var $elems = $('.' + className);
    // we store the inputs value inside this array
    var values = [];
    // return this
    var isDuplicated = false;
    // loop through elements
    $elems.each(function () {
        //If value is empty then move to the next iteration.
        if (!this.value) return true;
        //If the stored array has this value, break from the each method
        if (values.indexOf(this.value) !== -1) {
            isDuplicated = true;
            return false;
        }
        // store the value
        values.push(this.value);
    });
    return isDuplicated;
}
