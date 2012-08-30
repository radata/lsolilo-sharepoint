if (typeof Type != "undefined" && Type && Type.registerNamespace) {
    Type.registerNamespace("FPS");
}

if (typeof FPS.Utility == "undefined")
    FPS.Utility = function () { };

FPS.Utility.moveAggregationRowToBottom = function (searchFrom) {
    var tbodies = document.getElementsByTagName("tbody");
    var aggrRows = [];
    for (var i = 0; i < tbodies.length; i++) {
        if (tbodies[i].id == "aggr")
            aggrRows.push(tbodies[i]);
    }
    for (var i = 0; i < aggrRows.length; i++) {
        var parent = aggrRows[i].parentNode;
        parent.removeChild(aggrRows[i]);
        parent.appendChild(aggrRows[i]);
        FPS.Utility.setAlignmentAllCells(aggrRows[i]);
    }
};

FPS.Utility.setAlignmentAllCells = function (tbody) {
    for (var i = 0; i < tbody.rows.length; i++) {
        for (var j = 0; j < tbody.rows[i].cells.length; j++) {
            tbody.rows[i].cells[j].style.textAlign = 'right';
        }
    }
};

if (!Type.isClass(FPS.Utility))
    FPS.Utility.registerClass("FPS.Utility");

typeof (Sys) != "undefined" && Sys && Sys.Application && Sys.Application.notifyScriptLoaded();
NotifyScriptLoadedAndExecuteWaitingJobs("FPS.Controls.Utility.js");