(function (global) {
    var miles = global.miles = global.miles || {};


})(window);

//var _headerMenuDetached = false;




//var _counterInterval = null;
//var _count = 0;

$(document).ready(function () {
    ////attach scroll event to the window so we can tell when the header is out of view
    //$(document).scroll(function () {

    //    //get header object
    //    var $mainHeader = $("#mainHeader");

    //    //measure the height of the header
    //    var headerHeight = $mainHeader.outerHeight();
    //    var windowScrollTop = $(window).scrollTop();

    //    //if the window scroll top is greater than the header height, then
    //    //the header is out of view. detach the fav menu
    //    if (windowScrollTop > headerHeight && !_headerMenuDetached) {
    //        //detach element
    //        $(".favMenu").addClass("detached").css({ top: -40 })
    //            .animate({
    //                top: 0
    //            },
    //            {
    //                duration: 250,
    //                easing: 'easeInQuad'
    //            });


    //        _headerMenuDetached = true;
    //    }
    //    else if (windowScrollTop <= headerHeight && _headerMenuDetached) {
    //        //reattach the element to the header
    //        $(".favMenu").removeClass("detached");
    //        _headerMenuDetached = false;
    //    }


    //});

    //collapse all the mobile menu (if they are expanded) except the one clicked
    $('.navbar-toggle').click(function () {
        var $target = $($(this).data('target'));
        if (!$target.hasClass('in'))
            $('.navbar .in').removeClass('in').addClass('collapse').height(0);
    });
});






//function rdActions_OnClientButtonClicking(sender, args) {
//    var item = args.get_item();

//    var command = item.get_commandName();
//    var argument = item.get_commandArgument();

//    if (command == "UserInfo") {
//        openUserInfo();
//        args.set_cancel(true);
//    }
//}


//media query to determine whether we are in mobile mode on not

//var mobileMode is declared as global variable in global.js

var querySmallDevice = "screen and (max-width:768px)"
handlerSmallDevice = {
    match: function () { mobileMode = true },
    unmatch: function () { mobileMode = false }
}
var queryLargeDevice = "screen and (min-width:768px)"
handlerLargeDevice = {
    match: function () { mobileMode = false },
    unmatch: function () { mobileMode = true }
}


enquire.register(querySmallDevice, handlerSmallDevice);
enquire.register(queryLargeDevice, handlerLargeDevice);

