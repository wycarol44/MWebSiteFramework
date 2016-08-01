
(function (global) {
    var miles = global.miles = global.miles || {};

    //updates the entity footer
    miles.updateEntityFooter = function (value) {
        //get the entity footer
        $(".entity-footer").text(value);
    }

    //adjust the height of the dialog content
    miles.adjustDialogContentHeight = function () {
        $(document).ready(function () {
            //get content divs
            var $content = $(".dialog-content");
            var $footer = $(".dialog-actions");

            if ($content.length > 0 && $footer.length > 0) {

                //calculate height
                var dh = $footer.outerHeight();
                var wh = $(window).outerHeight();
                //make the content div the height - footer height
                $content.height(wh - (dh));

            }
        });
    }


    //override default validation summary behavior
    miles.initValidationSummary = function() {

        //override the default behavior of the validation summary
        if (typeof (ValidationSummaryOnSubmit) !== "undefined") {

            //store original summary function
            if (typeof (BaseValidationSummaryOnSubmit) === "undefined")
                BaseValidationSummaryOnSubmit = ValidationSummaryOnSubmit;

            //create new summary function
            ValidationSummaryOnSubmit = function (validationGroup) {
                //call original
                BaseValidationSummaryOnSubmit(validationGroup);

                //get the summary info
                if (typeof (Page_ValidationSummaries) == "undefined")
                    return;

                //show the summary
                for (sums = 0; sums < Page_ValidationSummaries.length; sums++) {
                    summary = Page_ValidationSummaries[sums];
                    summary.style.display = "none";
                    if (!Page_IsValid && IsValidationGroupMatch(summary, validationGroup)) {
                        //show the summary
                        showMsg(summary.innerHTML, false, 'Please correct the following', 'errorMsg', 'center');
                    }
                }
            }
        }
    }


    //disable backspace - so user accidentally doesnt lose any information by going to the last page in history because of the backspace
    $(document).keydown(function (e) {
        var elid = $(document.activeElement).is('input[type="text"]:focus,input[type="password"]:focus,input[type="email"]:focus,textarea:focus');
        if (e.keyCode === 8 && !elid) {
            return false;
        };
    });

})(window);

/*
 * Rad Window Functions
 */

//thse window sizes are based on a 1.5 aspect ratio
var WINDOW_RATIO = 1.5;
var WINDOW_SIZE_PERCENT = 0.8;

var MAX_WINDOW_HEIGHT = 550;
var MAX_WINDOW_WIDTH = MAX_WINDOW_HEIGHT * WINDOW_RATIO;

var MED_WINDOW_HEIGHT = MAX_WINDOW_HEIGHT * WINDOW_SIZE_PERCENT;
var MED_WINDOW_WIDTH = MED_WINDOW_HEIGHT * WINDOW_RATIO;

var MIN_WINDOW_HEIGHT = MED_WINDOW_HEIGHT * WINDOW_SIZE_PERCENT;
var MIN_WINDOW_WIDTH = MIN_WINDOW_HEIGHT * WINDOW_RATIO;



var WINDOW_CUSTOM = -1;
var WINDOW_SMALL = 0;
var WINDOW_MEDIUM = 1;
var WINDOW_LARGE = 2;



//Handles the close event
function windowClosed(sender, args) {
    //remove the close handler so it doesnt stack
    sender.remove_close(windowClosed);

    //call the callback function
    if (sender.closeCallback != undefined) {
        sender.closeCallback(sender, args);
    }
}


//Gets the current window
function getRadWindow() {
    var wnd = null;

    if (window.radWindow)
        wnd = window.radWindow;
    else if (window.frameElement.radWindow)
        wnd = window.frameElement.radWindow;

    //return the current window
    return wnd;
};


//closes a rad window
function closeWindow(val) {
    var wnd = getRadWindow();

    if (wnd) {
        wnd.close(val);
    }

};

//end rad window functions




/*
 *JGROWL
 */
//shows the jGrowl message
function showMsg(msg, sticky, header, theme, position, life) {

    if (typeof sticky === 'undefined' || sticky == null) {
        sticky = false;
    }
    //if (typeof header === 'undefined' || header == null) {
    //    header = "";
    //}
    if (typeof theme === 'undefined' || theme == null) {
        theme = "notifyMsg";
    }
    if (typeof position === 'undefined' || position == null) {
        position = "center";
    }
    if (typeof life === 'undefined' || life == null) {
        life = 3000;
    }


    //show message
    $.jGrowl(msg, {
        position: position,
        sticky: sticky,
        theme: theme,
        header: header,
        life: life
    });

}

//END JGROWL





/*
 * String functions
 */

/*
 * formats a string
 */
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
        ? args[number]
        : match
            ;
        });
    };
}

//end string functions

var mobileMode = false;

