(function($) {
    $.fn.CoCList = function() {
        var ele = this;
        $.getJSON(globCocUrl + 'mlist', function(result) {
            if (result.error > 0) {
               return;
            }
            $.each(result, function(i, item) {
                var ratioStyle;
                if (item.data.ratio == 0) {
                   ratioStyle = 'cocratioundef';
                }
                else if (item.data.offset < 1) {
                   ratioStyle = 'cocratiopoor';
                } else {
                   ratioStyle = 'cocratiogood';
                }
                ele.append(
                    '<tr>' +
                    '<td class="coccount">' + i + '</td>' +
                    '<td class="cocname cocfont" id="niktag' + i + '" rel="' + item.data.tag + '">' +
                       '<img src="' + item.data.ico + '" class="cocleagueimg">&nbsp;' + item.data.nik + '</td>' +
                    '<td class="cocrole"><span class="' + item.data.role + '"></span></td>' +
                    '<td class="coctdl">' + item.data.trophies + '</td>' +
                    '<td class="coctdl">' + item.data.send + '</td>' +
                    '<td class="coctdl">' + item.data.recive + '</td>' +
                    '<td class="' + ratioStyle + '">' + item.data.offset + '</td>' +
                    '<td class="coctdc"><span class="coclevel cocfont">' + item.data.level + '</td>' +
                    '</tr>'
                );
            });
        });
    };
}(jQuery));
