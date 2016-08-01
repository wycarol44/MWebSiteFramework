MilesTabDropDown = function (tabSelector, selectedTabSelector, onClientTabSelectionChanged) {

    var tabSelector = $(tabSelector);
    var selectedTabSelector = $(selectedTabSelector);
    
    this.onClientTabSelectionChanged = onClientTabSelectionChanged;
    
    this.beginLayout = function() {
        var tabsul = tabSelector;
        var selectedTab = $('#' + selectedTabSelector.val());

        if (selectedTab) {
            tabsul.find("li").not(".dropdown").not(selectedTab).removeClass('active');
            selectedTab.addClass('active');
        }

        layoutTabs(tabsul);

        var timer;

        $(window).on('resize', function () {
            clearTimeout(timer);
            timer = setTimeout(layoutTabs(tabsul), 100);
        });
    }


    var layoutTabs = function(tabsul) {
        var addtoDropdown = [];

        var dropdown = tabsul.find('li.dropdown');

        dropdown.removeClass('hide');
        dropdown.children('a').text(' <span class="caret"></span>');

        var dropdownmenu = dropdown.find('.dropdown-menu');

        //remove any tabs added to dropdown and put it back to parent ul
        tabsul.append(dropdownmenu.find('li'))
              .children('li').not('.dropdown')
              .each(function () {
                  var listitem = $(this);
                  if (this.offsetTop > 0) {
                      addtoDropdown.push(listitem)
                  }
              });

        //get the overflowing tabs and add them to dropdown
        if (addtoDropdown.length > 0) {
            dropdownmenu.append(addtoDropdown);

            if (dropdownmenu.find('.active').length == 1) {
                dropdown.addClass('active');

                var activetabHTML = dropdownmenu.find('.active > a').html();
                dropdown.children('a').html(activetabHTML + ' <span class="caret"></span>');

            } else {
                dropdown.removeClass('active');
                dropdown.children('a').html(addtoDropdown[0].children('a').text() + ' <span class="caret"></span>');
            }
        }
        else {
            dropdown.addClass('hide');
        }
    }


    this.onTabClicked = function (sender, tabtext, tabValue)
    {
        var args = {
            selectedTabText: tabtext,
            selectedTabValue: tabValue
        };
        

        //fire selected Event
        if (this.onClientTabSelectionChanged) {
            //raise event
         return this.onClientTabSelectionChanged(sender, args);
        }
        return true;
    }



}