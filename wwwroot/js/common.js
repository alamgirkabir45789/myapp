var Common = {
    Ajax: function (httpMethod, url, data, type, successCallBack, async, cache) {
        if (typeof async == "undefined") {
            async = true;
        }
        if (typeof cache == "undefined") {
            cache = false;
        }

        var ajaxObj = $.ajax({
            type: httpMethod.toUpperCase(),
            url: url,
            data: data,
            dataType: type,
            async: async,
            cache: cache,
            success: successCallBack,
            error: function (err, type, httpStatus) {
                Common.AjaxFailureCallback(err, type, httpStatus);
            }
        });

        return ajaxObj;
    },

    DisplaySuccess: function (message) {
        Common.ShowSuccessSavedMessage(message);
    },

    DisplayError: function (error) {
        Common.ShowFailSavedMessage(message);
    },

    AjaxFailureCallback: function (err, type, httpStatus) {
        var failureMessage = 'Error occurred in ajax call' + err.status + " - " + err.responseText + " - " + httpStatus;
        console.log(failureMessage);
    },

    ShowSuccessSavedMessage: function (messageText) {

        //use jquery BlockUI library to display message

        $.blockUI({ message: messageText });
        setTimeout($.unblockUI, 1500);
    },

    ShowFailSavedMessage: function (messageText) {

        //use jquery BlockUI library to display message

        $.blockUI({ message: messageText });
        setTimeout($.unblockUI, 1500);
    }

     //Dependency JQUERY UI url: https://jqueryui.com/autocomplete/

        //autocomplete: function (TagID, DataSourceUrl) {
        //    designation = new Array();
        //    $.ajax({
        //        url: DataSourceUrl,
        //        type: "GET",
        //        success: function (msg) {
        //            designation = msg;
        //            $("#" + TagID).autocomplete({
        //                source: designation
        //            });
        //        },
        //        dataType: "json"
        //    });
        //}
}

//Dependency JQUERY UI url: https://jqueryui.com/autocomplete/
function autocomplete(TagID, DataSourceUrl) {
    designation = new Array();
    $.ajax({
        url: DataSourceUrl,
        type: "GET",
        success: function (msg) {
            designation = msg;
            $("#" + TagID).autocomplete({
                source: designation
            });
        },
        dataType: "json"
    });
}