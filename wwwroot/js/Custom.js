function confirmDelete(uniqueid, isTrue) {
    var deletespan = "deleteSpan_" + uniqueid;
    var confirmdeletespan = "confirmDeleteSpan_" + uniqueid;
    if (isTrue) {
        $("#" + deletespan).hide();
        $("#" + confirmdeletespan).show();
    }
    else {
        $("#" + deletespan).show();
        $("#" + confirmdeletespan).hide();
    }
}