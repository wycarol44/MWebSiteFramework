(function (global) {
    var MilesSearchPanel = global.MilesSearchPanel = global.MilesSearchPanel || {};

    //toggles expanded state of the search panel
    MilesSearchPanel.toggleExpand = function (contentPanelSelector, expandButtonSelector, clientStateSelector) {

        $(contentPanelSelector).slideToggle({
            duration: 250,
            easing: 'easeInQuad',
            done: function () {
                var $toggler = $(expandButtonSelector); //'#<%= ExpandButton.ClientID %> > .glyphicon'
                var $clientState = $(clientStateSelector);//'#<%= ClientState.ClientID %>'
                var state = JSON.parse($clientState.val());
                //add new class
                if (state.Expanded == false) {
                    $toggler.removeClass('glyphicon-chevron-down')
                        .addClass('glyphicon-chevron-up');

                    //set client state
                    state.Expanded = true;
                }
                else {
                    $toggler.removeClass('glyphicon-chevron-up')
                        .addClass('glyphicon-chevron-down');

                    //set client state
                    state.Expanded = false;
                }
                //set the json object to the client state
                $clientState.val(JSON.stringify(state));
            }
        }); return false;

    };

    //toolbar click event
    MilesSearchPanel.CommandToolBar_OnClientButtonClicking = function (sender, args) {
        var item = args.get_item();

        var command = item.get_commandName();
        var argument = item.get_commandArgument();

        if (command == "RemoveFilters") {
            var res = confirm("Are you sure you want to remove your saved filters?");

            if (!res) {
                args.set_cancel(true);
            }
        }
    };

    //scroll to a target
    //MilesSearchPanel.SearchButton_OnClientClicking = function (sender, args) {
    //    var target = $(sender.get_element()).data("target");
    //    window.scrollTo(0, $(target).offset().top);
    //}

    MilesSearchPanel.scrollToTarget = function (target) {

        window.scrollTo(0, $(target).offset().top);
    }

})(window);