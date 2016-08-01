
function MilesPhone_FormatPhoneNumber(input) {
    //format text like US/CANADA phone number
    input = "({0}) {1}-{2}".format(input.substring(0, 3),
                                    input.substring(3, 6),
                                    input.substring(6, 10));

    return input;
}

function MilesPhone_Load(sender, args) {
    

    //get text
    var text = sender.get_value();
    var isAutoPostback = sender.get_autoPostBack();

    if (text.length == 10) {
        //turn off autopostback
        sender.set_autoPostBack(false);
        //format the number
        text = MilesPhone_FormatPhoneNumber(text);
        //set the value
        sender.set_value(text);

        sender.set_autoPostBack(isAutoPostback);
    }
}

function MilesPhone_Blur(sender, args) {
    //format the phone number
    var text = sender.get_value();

    //replace any non-numeric character with nothing
    text = text.replace(/[^0-9]+/g, '');

    //if text is exactly 10 digits, format the string
    if (text.length == 10) {
        text = MilesPhone_FormatPhoneNumber(text);
        //turn off autopostback
        sender.set_autoPostBack(false);

    }

    //set the value of the parent
    //sender.get_element().parentNode.parentNode.attributes["milesphonevalidationvalue"].value = text;

    //set set the text
    sender.set_value(text);

}
