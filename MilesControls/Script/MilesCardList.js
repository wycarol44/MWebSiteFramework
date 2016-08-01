MilesCardList = function (cardListClientId, cardListSelector, selectedStateSelector, checkAllSelector, allowSelection,
                            allowMultiSelection, infoBarDetachable, onClientItemSelectionChanged) {
    var instance = this;

    //initializes the object
    var cardListSelector = cardListSelector;
    var selectedStateSelector = selectedStateSelector;

    //client side events
    this.onClientItemSelectionChanged = onClientItemSelectionChanged;


    //store state object
    //get the client state object
    this.state = JSON.parse($(selectedStateSelector).val());

    var listView = null;
    setTimeout(function () {
        listView = $find(cardListClientId);
        listView.cardList = instance;


    }, 100);


    //saves the state
    var saveState = function () {
        //set the json object to the client state
        $(selectedStateSelector).val(JSON.stringify(instance.state));
    }

    //restores the selected items
    var restore = function () {
        var $items = $(cardListSelector + ' .card').each(function (x) {
            var $item = $(this);

            //make sure the card has a checkbox and it is not disabled
            //var $chk = $item.find(".select input:enabled");
            //if ($chk.length) {

            //find a matching value
            if (instance.getItemIndex($item) > -1) {
                //add the class
                $item.addClass("selected");

                //check the checkbox
                $item.find(".select input").prop("checked", true);

            }
            //}
        });

        //check if all items are selected
        checkIfAllSelected($items);

    };

    //deselects all items (across all pages)
    var clear = function () {
        //clear all selected items
        instance.state.SelectedItems.length = 0;
        //clear selected class
        $(cardListSelector + ' .card').removeClass("selected");

        //uncheck the checkbox
        $(cardListSelector + ' .card .select input').prop("checked", false);
    }

    //checks to see if all items are selected
    var checkIfAllSelected = function ($items) {
        if (typeof ($items) === 'undefined') {
            $items = $(cardListSelector + ' .card');
        }
        //check/uncheck all
        $(checkAllSelector).prop("checked",
            ($items.length > 0 && $items.length == $items.filter(".selected").length));
    }

    //selects a single item
    var selectItem = function ($item, index) {

        //remove any other selection if multi select not allowed
        if (!allowMultiSelection) {
            clear();
        }

        //add the class
        $item.addClass("selected");

        //check the checkbox
        $item.find(".select input").prop("checked", true);

        //get the cardlist value
        var value = $item.data("value").toString();

        //add this to the selected state
        instance.state.SelectedItems.push({ Index: index, Value: value });

        //persist the state
        saveState();

        //fire the on selecting event
        if (instance.onClientItemSelectionChanged) {
            //raise event
            instance.onClientItemSelectionChanged(listView, index, $item);
        }

        //debug
        console.log(JSON.stringify(instance.state));
    }

    //deselects a single item
    var deselectItem = function ($item, index) {

        //remove the class
        $item.removeClass("selected");

        //check the checkbox
        $item.find(".select input").prop("checked", false);

        //find the item in the list and remove it
        var x = instance.getItemIndex($item);
        if (x > -1) {
            //remove the item from the array
            instance.state.SelectedItems.splice(x, 1);

            //persist the state
            saveState();
        }

        //fire the on selecting event
        if (instance.onClientItemSelectionChanged) {
            //raise event
            instance.onClientItemSelectionChanged(listView, index, $item);
        }

        //debug
        console.log(JSON.stringify(instance.state));
    }

    //adds or removes selected state
    var toggleSelection = function (e) {
        var $item = $(this).parent().parent();
        var index = $item.parent().parent().children().index($item.parent());
        //toggle the selected class
        if ($item.hasClass("selected")) {
            //deselect
            deselectItem($item, index);
        }
        else {
            //select item
            selectItem($item, index);
        }

        //check if all items are selected
        checkIfAllSelected();
    }

    //selects all items
    var selectAll = function (e) {
        var $chkAll = $(this);

        //select all
        $(cardListSelector + ' .card').each(function (x) {
            var $item = $(this);

            //make sure the card has a checkbox and it is not disabled
            var $chk = $item.find(".select input:enabled");
            if ($chk.length) {


                //reselect only if checkbox is checked
                if ($chkAll.prop("checked")) {
                    //select item
                    if (instance.getItemIndex($item) == -1) {
                        selectItem($item, x);
                    }
                }
                else {
                    //deselect the item
                    if (instance.getItemIndex($item) > -1) {
                        deselectItem($item, x);
                    }
                }
            }
        });


    }

    //determines is the item is selected
    this.getItemIndex = function ($item) {
        //get the cardlist value
        var value = $item.data("value").toString();
        var index = -1;
        //check to see if the item is in the list
        for (var x = 0; x < instance.state.SelectedItems.length; x++) {
            var obj = instance.state.SelectedItems[x];
            //check the value property
            if (obj.Value == value) {
                index = x;
                break;
            }

        }

        return index;
    }


    //drag and drop functions

    var isItemDragging = false;
    var draggedItemIndex = null;

    this.itemDragStarted = function (sender, args) {
        isItemDragging = true;
        draggedItemIndex = args.get_itemIndex();
    }

    //update the cursor if the user is dragging the node over supported drop target -same listview
    this.itemDragging = function (sender, args) {
        checkDropTargets(args.get_domEvent().srcElement || args.get_domEvent().originalTarget);
    }

    this.itemDropping = function (sender, args) {
        if (sender.get_id() == cardListClientId) {
            isItemDragging = false;
            document.body.style.cursor = "";

            var node = args.get_destinationElement();
            if (!isChildOf(cardListClientId, node)) {
                args.set_cancel(true);
            }
            else {
                var $dataItem = $(args.get_destinationElement()).parents("div[id$='CardPanel']");

                args.get_destinationElement().id = $dataItem.data("item-id");

            }
        }

    }

    this.itemDropped = function (sender, args) {
        document.body.style.cursor = "";
        isItemDragging = false;
    }

    var checkDropTargets = function (node) {
        if (!isChildOf(cardListClientId + "_ListContainerPanel", node)) {
            document.body.style.cursor = "no-drop";
        }
        else {
            document.body.style.cursor = "";

        }
    }

    //find the cardPanel element with id so that we can look for the dataitem element
    var getDataItemControl = function (node) {
        var parentdiv = node.parentNode;
        while (!parentdiv.id) {
            parentdiv = getDataItemControl(node.parentNode);
        }
        return parentdiv;
    }

    var isChildOf = function (parentId, element) {
        while (element) {
            if (element.id && element.id.indexOf(parentId) > -1) {
                return true;
            }
            element = element.parentNode;
        }
        return false;
    }

    this.onMouseOverCard = function (sender) {
        if (isItemDragging) {
            var $target = $(sender);
            //if the dragging over same card, donot change the border
            if ($target.data("index") != draggedItemIndex) {
                if ($target.data("index") < draggedItemIndex) {
                    $target.addClass("drag-over-top");
                }
                else if ($target.data("index") > draggedItemIndex) {
                    $target.addClass("drag-over-bottom");
                }
            }
        }
    }

    this.onMouseOutCard = function (sender) {
        if (isItemDragging) {
            var $target = $(sender);
            $target.removeClass("drag-over-top");
            $target.removeClass("drag-over-bottom");
            //sender.style.borderTop = "none";
        }
    }
    this.onMouseUpCard = function (sender) {
        isItemDragging = false;
    }
    //end drag and drop functions







    if (allowSelection) {
        setTimeout(function () {
            //restore state
            restore();

            //hook up some events
            $(cardListSelector + ' .card .select input').click(toggleSelection);

            $(cardListSelector + ' .action').click(function () {
                //don't want to select the card if we click on an action
                //event.cancelBubble = true;
                //event.stopPropagation();
            });

            //add event to check all checkbox if allowed to make multiple selections
            if (allowMultiSelection) {
                $(checkAllSelector).change(selectAll);
            }
        });
    }

    //if infobar is detachable
    if (infoBarDetachable) {
        //get header object
        this.$infoBar = $(cardListSelector + " .info-row");

        //if no info bar, ignore
        if (this.$infoBar.length) {

            //variables
            this.infoBarDetached = false;
            //measure the height of the header
            this.infoBarPosY = this.$infoBar.offset().top;
            this.infoBarHeight = this.$infoBar.height();

            //resize the info bar when the window is resized
            $(window).resize(function () {
                //calculate width
                var infoBarWidth = $(cardListSelector).innerWidth()
                    + parseInt($(cardListSelector).css("margin-left"))
                    + parseInt($(cardListSelector).css("margin-right"));

                //set width
                instance.$infoBar.css({ width: infoBarWidth });
            });

            //attach scroll event to the window so we can tell when the header is out of view
            $(document).scroll(function () {

                //calculate the window scroll pos
                var windowScrollTop = $(window).scrollTop();

                //if the window scroll top is greater than the header height, then
                //the header is out of view. detach the fav menu
                if (windowScrollTop > (instance.infoBarPosY + instance.infoBarHeight + 100) && !instance.infoBarDetached) {

                    //trigger the resize event
                    $(window).trigger("resize");

                    //detach element
                    instance.$infoBar.addClass("detached").css({ top: -instance.infoBarHeight })
                        .animate({
                            top: 0
                        },
                        {
                            duration: 250,
                            easing: 'easeInQuad'
                        });


                    instance.infoBarDetached = true;
                }
                else if (windowScrollTop <= (instance.infoBarPosY + instance.infoBarHeight + 50) && instance.infoBarDetached) {
                    //set width
                    instance.$infoBar.css({ width: 'auto' });
                    //reattach the element to the header
                    instance.$infoBar.removeClass("detached");
                    instance.infoBarDetached = false;
                }


            });
        }
    }

}