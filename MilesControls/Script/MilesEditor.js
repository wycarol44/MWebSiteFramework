function MilesEditor_OnRadEditorClientLoad(editor, args) {
    editor.removeShortCut("InsertTab");

    var buttonsHolder = $get(editor.get_id() + "Top"); //get a reference to the top toolbar zone of the editor 
    var buttons = buttonsHolder.getElementsByTagName("A"); //get a reference to all A elements on the toolbar and disable the tabbing trough them 
    for (var i = 0; i < buttons.length; i++) {
        var a = buttons[i];
        a.tabIndex = -1;
        a.tabStop = false;
    }

}