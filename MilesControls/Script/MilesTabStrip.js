function MilesTabStrip_TreeTabClick(sender, args) {
    var entry = args.get_entry();
    window.location.href = entry.get_value();
}