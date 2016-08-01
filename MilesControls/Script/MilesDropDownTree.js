
function MilesDropDownTree_OnDropDownOpening(sender, args) {

    var w = $(sender.get_element()).outerWidth();

    $(sender.get_dropDownElement()).width(w);
}

function MilesDropDownTree_OnClientEntryAdded(sender, args) {
    setTimeout(function () {
                sender.closeDropDown();
                }, 100);
}
