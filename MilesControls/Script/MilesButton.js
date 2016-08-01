
function MilesButton_Init(sender, args) {
    //add event handlers
    sender.add_clicking(MilesButton_OnClientClicking);
    sender.add_clicked(MilesButton_OnClientClicked);
}

function MilesButton_OnClientClicking(sender, args) {
    var element = sender.get_element();
    sender.canceled = false;

    //if the button does not cause validation, we dont fire the
    //validation functions
    if (element.attributes["causesvalidation"].value == "false")
        return;

    //validate the page
    var valid = true;
    if (typeof Page_IsValid !== 'undefined') {
        valid = Page_IsValid;
    }
    
    //if valid, continue. otherwise cancel
    if (!valid) {
        //cancel the click
        //args.set_cancel(true);
        sender.canceled = true;
    }

}

function MilesButton_OnClientClicked(sender, args) {

    if (sender.canceled != undefined && sender.canceled == true) {
        sender.set_enabled(true);
        return;
    }


    //activate the spinner
    var element = sender.get_element()
    MilesButton_StartSpinner(element);


    MilesButton_Enable(sender, false);

}


function MilesButton_Enable(sender, value) {
    var element = sender.get_element()

    //disable all the related buttons
    if (element.attributes["relatedbuttons"] != undefined) {
        var relatedBtns = element.attributes["relatedbuttons"].value.split(',');
        for (var x = 0; x < relatedBtns.length; x++) {
            //disable the button
            var btn = $find(relatedBtns[x]);
            if (btn != null) {
                //enable/disable
                btn.set_enabled(value);
            }
        }
    }

    //check to see if autodisablesamebuttontypes is set
    var autodisablesamebuttontypes = element.attributes["autodisablesamebuttontypes"];
    if (autodisablesamebuttontypes != undefined && autodisablesamebuttontypes.value == "true") {
        var action = element.attributes["actiontype"].value;
        //find all similar buttons
        var $buttons = $('span[actiontype="' + action + '"]')
        $buttons.each(function () {
            var btn = $find(this.id);
            if (btn != null && btn.get_element() != element) {
                //enable/disable
                btn.set_enabled(value);
            }
        });
    }
}


function MilesButton_StartSpinner(target) {
    var opts = {
        lines: 9, // The number of lines to draw
        length: 2, // The length of each line
        width: 5, // The line thickness
        radius: 5, // The radius of the inner circle
        corners: 1, // Corner roundness (0..1)
        rotate: 0, // The rotation offset
        color: '#555', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 40, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false, // Whether to use hardware acceleration
        className: '', // The CSS class to assign to the spinner
        zIndex: 0, // The z-index (defaults to 2000000000)
        top: 'auto', // Top position relative to parent in px
        left: 'auto' // Left position relative to parent in px
    };


    var spinner = new Spinner(opts).spin(target);
    return spinner;
}