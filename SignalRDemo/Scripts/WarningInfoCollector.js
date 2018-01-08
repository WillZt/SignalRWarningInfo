if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

$(function () {

    var ticker = $.connection.warningInfoHub;
    var $infoTable = $('#infoTable');
    var $infoTableBody = $infoTable.find('tbody');
    var rowTemplate = '<tr data-id="{MessageId}"><td>{MessageId}</td><td>{PipeId}</td><td>{AreaId}</td><td>{WarningLevel}</td><td>{WarningValue}</td><td>{WarningTime}</td></tr>';

    // 开始client和server连接
    $.connection.hub.start().done(init);

    function init() {
        ticker.server.getAllWarningInfo().done(function (infos) {
            $infoTableBody.empty();
            $.each(infos, function () {
                var info = formatInfo(this);
                $infoTableBody.append(rowTemplate.supplant(info));
            });
        });
    }

    function formatInfo(info) {
        return $.extend(info, {
            WarningValue: info.WarningValue.toFixed(2),
        });
    }

    ticker.client.updateWarningInfo = function (infos) {
        $infoTableBody.empty();
        $.each(infos, function () {
            var info = formatInfo(this);
            $infoTableBody.append(rowTemplate.supplant(info));
        });
    }

});