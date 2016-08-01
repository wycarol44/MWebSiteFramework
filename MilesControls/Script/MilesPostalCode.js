function MilesPostalCode_FormatPostalCode(text) {
    //test if its the 9 digit postal code
    var regex = new RegExp("^[0-9]{9}$");
    if (regex.test(text)) {
        //split value into two parts at char 5
        text = text.substring(0, 5) + "-" + text.substring(5, 9);
    }

    return text;
}

function MilesPostalCode_Load(sender, args) {
    var text = sender.get_value();

    //format the postal code
    text = MilesPostalCode_FormatPostalCode(text);

    //set set the text
    sender.set_value(text);
}

function MilesPostalCode_Blur(sender, args)
{

    var text = sender.get_value();

    //format the postal code
    text = MilesPostalCode_FormatPostalCode(text);

    //set set the text
    sender.set_value(text);
}