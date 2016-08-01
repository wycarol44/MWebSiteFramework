//Highlighter
//Author: Eric Butler
//Date: july 2013

var ValidatorHighlight_InvalidValidators = [];

//Gets a list of all the validators on the page
function ValidatorHighlight_GetValidators() {
    //create list of validators
    var validators = [];
    //check to see if the page has validators
    if (typeof (Page_Validators) !== "undefined") {
        validators = Page_Validators;
    }

    return validators;
}

//initializes the validator highlighter
function ValidatorHighlight_Init() {

    var validators = ValidatorHighlight_GetValidators();

    //hook up each validator
    for (var x = 0; x < validators.length; x++) {
        //get each validator on the page
        var val = validators[x];
        if (val.controltovalidate != undefined) {

            var element = document.getElementById(val.controltovalidate);
            if (element) {
                
                //replace the validator's evaluation function
                if (val.evalFunction == undefined) {
                    val.evalFunction = val.evaluationfunction;
                }

                //set new function
                val.evaluationfunction = function evalFunction(val) {
                    var valid = val.evalFunction(val);
                    var $e = $("#" + val.controltovalidate);


                    //before the final determination that the control is valid, all validators associated
                    //to the control must be valid.
                    var isvalid = false;
                    if ($e[0].Validators != undefined) {
                        //check that all are valid
                        for (var z = 0; z < $e[0].Validators.length; z++) {
                            var v = $e[0].Validators[z];
                            //set the value for the matching validator

                            if (v.id == val.id) {
                                v.validState = valid;

                                break;
                            }
                        }


                        //check that all are valid. its false until all pass
                        for (var z = 0; z < $e[0].Validators.length; z++) {
                            var v = $e[0].Validators[z];
                            //get the valid state
                            if (v.validState || (v.controltocompare != undefined && v.controltocompare == $e[0].id)) {
                                isvalid = true;

                            }
                            else {
                                //set to false and break if even one is not valid
                                isvalid = false;

                                break;
                            }
                        }

                    }
                    else {
                        isvalid = valid;
                    }

                    
                    //highlight the controls. Here we look at isvalid because this determines if
                    //all validators for a control are valid.
                    
                    //check to see if this is a telerik control
                    var $type = $find(val.controltovalidate);
                    
                    //check what type of control it is, then apply the style
                    if (typeof Telerik != 'undefined' && typeof Telerik.Web.UI.RadDatePicker != 'undefined' && $type instanceof Telerik.Web.UI.RadDatePicker
                        || typeof Telerik != 'undefined' && typeof Telerik.Web.UI.RadDateTimePicker != 'undefined' && $type instanceof Telerik.Web.UI.RadDateTimePicker) {
                        $type = $find(val.controltovalidate + "_dateInput");
                        //rad input
                        ValidatorHighlight_RadInputApplyClass($type, isvalid);
                    }
                    else if (typeof Telerik != 'undefined' && typeof Telerik.Web.UI.RadTextBox != 'undefined' && $type instanceof Telerik.Web.UI.RadTextBox
                        || typeof Telerik != 'undefined' && typeof Telerik.Web.UI.RadNumericTextBox != 'undefined' && $type instanceof Telerik.Web.UI.RadNumericTextBox
                        || typeof Telerik != 'undefined' && typeof Telerik.Web.UI.RadDateInput != 'undefined' && $type instanceof Telerik.Web.UI.RadDateInput
                        ) {
                        //rad input
                        ValidatorHighlight_RadInputApplyClass($type, isvalid);
                    }
                    else {
                        
                        //apply appropriate styles
                        ValidatorHighlight_ApplyClass($e, isvalid);
                    }





                    //add the validator to the collection if it is invalid, and not already in the list
                    var index = $.inArray(val.id, ValidatorHighlight_InvalidValidators);
                    if (index > -1) {
                        //its there in the list, if the validator valid, remove it
                        if (valid) {
                            ValidatorHighlight_InvalidValidators.splice(index, 1);
                        }
                    }
                    else {
                        //its not there, so check to see if the validator is not valid
                        //if not, then add it
                        if (!valid) {
                            ValidatorHighlight_InvalidValidators.push(val.id);
                        }
                    }
                    

                    return valid;
                }
            }

        }
    }

    
    //run the validators against any that are in the array.
    for (var x = 0; x < ValidatorHighlight_InvalidValidators.length; x++) {

        var id = ValidatorHighlight_InvalidValidators[x];

        for (var y = 0; y < validators.length; y++) {
            var val = validators[y];
            
            if (val.id == id) {
                //run the last validator again to see if we should trigger the popup
                ValidatorValidate(val, val.validationGroup, event);
            }
        }
    }


}

function ValidatorHighlight_ApplyClass($e, value) {
    //check to see if we should show invalid or not
    if (!value) {
        $e.addClass("invalid-control");
    }
    else {
        $e.removeClass("invalid-control");
    }
}

function ValidatorHighlight_RadInputApplyClass($e, value) {
    //check to see if we should show invalid or not
    if (!value) {

        //see if we already have the class
        if ($e.get_styles().EnabledStyle[1].indexOf("invalid-control") == -1) {
            //we have to do things a bit different for rad textbox
            $e.get_styles().EnabledStyle[1] += " invalid-control";
            $e.get_styles().FocusedStyle[1] += " invalid-control";
            $e.updateCssClass();
        }
    }
    else {
        //remove the style
        $e.get_styles().EnabledStyle[1] = $e.get_styles().EnabledStyle[1].replace("invalid-control", "");
        $e.get_styles().FocusedStyle[1] = $e.get_styles().FocusedStyle[1].replace("invalid-control", "");
    }
}



