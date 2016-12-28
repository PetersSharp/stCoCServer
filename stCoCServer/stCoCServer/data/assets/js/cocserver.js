
(function($) {

    window.cocDateUrl = '';
    window.cocUserUrl = '';
    window.cocLocalizeDtUrl  = '';
    window.cocLocalizeLangUrl = '/assets/js/cocServerLang';
    window.cocGlobSelector = 'send';
    window.autoLanguage = navigator.language || navigator.userLanguage;
	   autoLanguage = ((!autoLanguage) ? 'ru' : autoLanguage.toLowerCase());
    window.EnumUpdate = {
         urlIrc: 0,
         urlClan: 1
    };
    String.prototype.Format = function()
    {
	var s = this,
            i = arguments.length;
	while (i--) {
	        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
	}
	return s;
    };
    String.prototype.AddSpan = function(eclass)
    {
	return '<span class="' + eclass + '">' + this + '</span>';
    };
    if (typeof String.prototype.endsWith !== 'function') {
	String.prototype.endsWith = function (suffix) {
		return (this.indexOf(suffix, this.length - suffix.length) !== -1);
	};
    }
    $.fn.ErrorsOn = function(code,msg) {
		$("#error-box").css('display','block');
		$("#error-message").html(' ' + code + ' - ' + msg);
    };
    $.fn.ErrorsOff = function() {
		$("#error-box").css('display','none');
		$("#error-message").html('');
    };
    $.fn.LangFormatData = function(name, value) {
	if (typeof globLangObj[name] === 'undefined') {
		return value;
	}
	return globLangObj[name];
    };
    var Base64 =
    {
	_keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",
	encode:  function(e) {
		var t = "";
		var n,r,i,s,o,u,a;
		var f = 0;
		e = Base64._utf8_encode(e);
		while(f < e.length) {
			n = e.charCodeAt(f++);
			r = e.charCodeAt(f++);
			i = e.charCodeAt(f++);
			s = n >> 2;
			o = (n&3) << 4 | r >> 4;
			u = (r&15) << 2 | i >> 6;
			a = i & 63;
			if (isNaN(r)) {
				u = a = 64;
			} else if(isNaN(i)) {
				a = 64;
			}
			t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a);
		} return t
	},
	decode: function(e){
		var t = "";
		var n,r,i;
		var s,o,u,a;
		var f=0;
		e = e.replace(/[^A-Za-z0-9+/=]/g,"");
		while(f < e.length) {
			s = this._keyStr.indexOf(e.charAt(f++));
			o = this._keyStr.indexOf(e.charAt(f++));
			u = this._keyStr.indexOf(e.charAt(f++));
			a = this._keyStr.indexOf(e.charAt(f++));
			n = s << 2 | o >> 4;
			r = (o&15) << 4 | u >> 2;
			i = (u&3) << 6 | a;
			t = t + String.fromCharCode(n);
			if (u!=64) {
				t = t + String.fromCharCode(r);
			}
			if (a!=64) {
				t = t + String.fromCharCode(i);
			}
		}
		t = Base64._utf8_decode(t);
		return t;
	},
	_utf8_encode: function(e) {
		e = e.replace(/rn/g,"n");
		var t = "";
		for (var n=0;n < e.length; n++) {
			var r = e.charCodeAt(n);
			if (r < 128) {
				t += String.fromCharCode(r)
			} else if(r > 127 && r < 2048) {
				t += String.fromCharCode(r >> 6 | 192);
				t += String.fromCharCode(r & 63 | 128)
			} else {
				t += String.fromCharCode(r >> 12 | 224);
				t += String.fromCharCode(r >> 6 & 63 | 128);
				t += String.fromCharCode(r&63|128);
			}
		} return t;
	},
	_utf8_decode: function(e) {
		var t = "";
		var n = 0;
		var r = c1 = c2 = 0;
		while (n < e.length) {
			r = e.charCodeAt(n);
			if (r < 128) {
				t += String.fromCharCode(r);
				n++;
			} else if(r > 191 && r < 224) {
				c2 = e.charCodeAt(n+1);
				t += String.fromCharCode((r&31) << 6 | c2 & 63);
				n += 2;
			} else {
				c2 = e.charCodeAt(n+1);
				c3 = e.charCodeAt(n+2);
				t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63);
				n +=3;
			}
		}
		return t;
	}
    };

    $.fn.CoCInfo = function() {

	var isMapDataLoaded = false;
	var cocmap = null;

	window.updateEmpty = function() {};
	window.showClanMap = function(coutrySelect) {

	  if(!isMapDataLoaded)
	  {
		return;
	  }
	  var mapele = $('#cocclanmap');
	  if (mapele.css('display') == 'block')
	  {
	        $('#cocclanmap').css({'display':'none'});
		return;
	  }
          mapele.css({'display':'block'});
	  if ((typeof cocmap != 'undefined') && (cocmap != null))
	  {
		return;
	  }
	  mapele.vectorMap({
		"map": 'world_mill',
		"regionsSelectable": false,
		"regionsSelectableOne": false,
		"backgroundColor": 'rgb(255, 245, 230)',
		"regionStyle": {
		  "initial": {
		    "fill": '#F9E9C8',
		    "fill-opacity": 1,
		    "stroke": 'none',
		    "stroke-width": 0,
		    "stroke-opacity": 1
		  },
		  "hover": {
		    "fill": '#EBD089',
		    "fill-opacity": 0.8,
		    "cursor": 'pointer'
		  },
		  "selected": {
		    "fill": '#EBD089'
		  },
		  "selectedHover": {
		  }
		},
		"series": {
		    "regions": [{
		        "values": {},
		        "attribute": 'fill'
		    }]
		}
	  });
	  var objc = [];
	  objc[coutrySelect] = '#EBD089';

	  cocmap = mapele.vectorMap('get', 'mapObject');
	  cocmap.series.regions[0].setValues(objc);
	  cocmap.updateSize();
	};

        var ele = this;
	$.getScript('/assets/js/jquery.jvectormap.world.js')
	  .done(function(script, textStatus) {
	    isMapDataLoaded = true;
	})
	  .fail(function(jqxhr, settings, exception) {
	    $.fn.ErrorsOn(jqxhr.status, exception);
	    isMapDataLoaded = false;
	});
        $.getJSON('/clan/info/', function() {})
          .done(function(items) {
	    if (items.error != 0) {
		$.fn.ErrorsOn(400, items.msg);
		return;
	    }
	    if (items.data.length === 0) {
		$.fn.ErrorsOn(400, 'Clan data empty!');
		return;
	    }
	    $.fn.ErrorsOff();
	    var data = items.data[0];
            var warpub = ((data.warpub === 1) ? 
		$.fn.LangFormatData('clanyes','Yes') : 
		$.fn.LangFormatData('clanno','No')
	    );
            var cocdesc = ((data.desc.length === 0) ? '' : $.trim(data.desc).replace('.','.<br/>'));
            var warfreq;
            switch (data.warfreq) {
                case "lessThenOncePerWeek" : {  warfreq = '1 <'; break; }
                case "oncePerWeek"         : {  warfreq = '1'; break; }
                case "moreThanOncePerWeek" : {  warfreq = '3'; break; }
                case "always"              : {  warfreq = '> 3'; break; }
                case "never"               :
                case "Unknown"             :
                default                    : {  warfreq = '0'; break; }
            }
            var clanintype;
            switch (data.type) {
                case "inviteOnly" : {  clanintype = $.fn.LangFormatData('cocclaninvite',data.type); break; }
                case "open"       : {  clanintype = $.fn.LangFormatData('cocclanopen',data.type); break; }
                case "closed"     : {  clanintype = $.fn.LangFormatData('cocclanclosed',data.type); break; }
                default           : {  clanintype = $.fn.LangFormatData('cocclannone','Unknown'); break; }
            }
            $('#cocclanimg').css({'background':'url(' + data.ico + ')','background-size':'100px 100px', 'background-repeat':'no-repeat'});
            $('#cocclanflag').css({'background':'url(' + data.flag + ')','background-repeat':'no-repeat'});
            $('#cocclanname').html(data.name);
            $('#cocclandesc').html(cocdesc);
            $('#cocclanlevel').html(data.level);
            $('#cocclanleveladd').html(data.level);
            $('#cocclanscore').html(data.points);
            $('#cocclanlocctry').html(data.locctry);
            $('#cocclanlocname').html(data.locname);
            $('#cocclantag').html(data.tag);
            $('#cocclanmembers').html(data.members + '/50');
            $('#cocclanwarfreq').html(warfreq + '/7');
            $('#cocclanwarwin').html(data.warwin);
            $('#cocclanwarstr').html(data.warstr);
            $('#cocclanwarpub').html(warpub);
            $('#cocclanin').html(clanintype);
            $('#cocclantrophies').html('> ' + data.trophies);

	    if (data.locreal == 1)
	    {
		$('#cocclanflag').css({'cursor': 'pointer'});
		$('#cocclanflag').on('click', function() {
		   if (isMapDataLoaded)
		   {
			showClanMap(data.locctry.toUpperCase());
		   }
		});
	    }
        });
    };
    $.fn.CoCList = function() {

	var dt = null;
	var counter = 1;
        var detailRows = [];
	cocLocalizeDtUrl  = '/assets/js/dataTablesLang/lang.' + autoLanguage + '.json';

	window.reloadDataTable = function() {
	     if (dt != null) {
		dt.ajax.url('/clan/list/' + cocDateUrl + cocUserUrl).load();
	     }
	};
	window.insFormatRole = function(role) {
             var insRole = '<span class="txt' + role + '" ';
             switch(role) {
                case "leader": {
                   insRole += 'data-localize="usrleader">' + $.fn.LangFormatData('usrleader','Leader');
                   break;
                }
                case "coLeader": {
                   insRole += 'data-localize="usrcoleader">' + $.fn.LangFormatData('usrcoleader','coLeader');
                   break;
                }
                case "admin": {
                   insRole += 'data-localize="usradmin">' + $.fn.LangFormatData('usradmin','Admin');
                   break;
                }
                case "member": {
                   insRole += 'data-localize="usrmember">' + $.fn.LangFormatData('usrmember','Member');
                   break;
                }
                default: {
                   insRole += 'data-localize="usrundef">' + $.fn.LangFormatData('usrundef','Undefined');
                   break;
                }
             }
	     return insRole + '</span>';
	};
	window.insFormatDataTables = function(dtr) {
	        var insRate = ((dtr.prank === dtr.rank) ?
        	    '<span class="cocequal"></span>' :
	            ((dtr.prank > dtr.rank) ?
        	       '<div class="cocdownclr"><span class="cocdown"></span>-' + (dtr.prank - dtr.rank) + '</div>' :
	               '<div class="cocupclr"><span class="cocup"></span>+' + (dtr.rank - dtr.prank)) + '</div>'
		);
		return  '<table><tr class="cocselect">' + 
			'<td class="coctdc"> ' +
				'<b>ID:</b>&nbsp;<span class="txtcoctag">#' + dtr.tag + '</span><br> ' + 
				'<b><span data-localize="th2">' + $.fn.LangFormatData('th2','Nik') + '</span>:</b>&nbsp;' + dtr.nik + '<br/> ' +
				'<b><span data-localize="th3">' + $.fn.LangFormatData('th3','Role') + '</span>:</b>&nbsp;' + insFormatRole(dtr.role) +
			'</td>' +
			'<td class="coctdc">' +
				'<center><img src="' + dtr.ico + '" /></center>' +
			'</td>' + 
			'<td class="coctdc">' +
				insRate +
			'</td>' +
			'<td class="coctdc"> ' +
				'<b><span data-localize="th9">'  + $.fn.LangFormatData('th9','Join') + '</span>:</b>&nbsp;<span class="cocdate">' + dtr.dtin + '</span><br/> ' +
				'<b><span data-localize="th10">' + $.fn.LangFormatData('th10','Exits') + '</span>:</b>&nbsp;<span class="cocdate">' + dtr.dtout + '</span><br/> ' +
				'<b><span data-localize="th11">' + $.fn.LangFormatData('th11','Note') + '</span>:</b>&nbsp;<span class="cocnote">'  + dtr.note + '</span><br/>' +
			'</td>' +
			'</tr></table>';
	};
	window.isUserFilter = function(isfilter) {
		cocUserUrl = ((isfilter) ? "1/" : "0/");
	};
        dt = $(this).DataTable({
		"deferRender": true,
                "stateSave": true,
                "lengthMenu": [[50, 25, 10, -1], [50, 25, 10, "All"]],
                "order": [[ 3, "desc" ]],
                "ajax": {
                 	url: '/clan/list/' + cocDateUrl + cocUserUrl,
			async: true,
                 	dataFilter: function(data) {
                 		var json = jQuery.parseJSON(data);
		                if (json.error > 0)
                 		{
		                	json.recordsTotal = 0;
					json.recordsFiltered = 0;
					json.data = [];
					$.fn.ErrorsOn(json.error, json.msg);
					return JSON.stringify(json);
				}
			return data;
			},
			error: function(jqXHR, msg) {
				$.fn.ErrorsOn(jqXHR.status, msg);
				return null;
			}
                 },
                 "language": {
                 	"url": cocLocalizeDtUrl
                 },
                 "initComplete": function () {
                 	$.fn.CoCLanguage("cocclan");
                 },
                 "rowCallback": function( row, data ) {
			$(row).attr('id','rid'+counter);
			counter++;
                 },
                 "columns": [
                 	{
				"data":       null,
				"class":      "coccount",
				"orderable":  false,
				"searchable": false,
				"render": function (data, type, row)  {
					return counter;
				}
			},{
				"data": "nik",
				"class": "cocname",
				"render": function (data, type, row)  {
					return  '<img src="' + row['ico'] + '" class="cocleagueimg"/>&nbsp;<span class="cocfont">' + data + '</span>';
				}
			},{
				"data": "role",
				"class": "cocrole",
				"render": function (data, type, row)  {
					return  '<span class="'+ data +'"></span>';
				}
			},{
				"data": "trophies",
				"class": "coctdl"
			},{
				"data": "send",
				"class": "coctdl"
			},{
				"data": "receive",
				"class": "coctdl",
			},{
				"data": "ratio",
				"class": "cocrole",
				"render": function (data, type, row)  {
					var ratioStyle;
					if (data == 0) {
						ratioStyle = 'cocratioundef';
					}
					else if (data < 1) {
						ratioStyle = 'cocratiopoor';
					} else {
						ratioStyle = 'cocratiogood';
					}
					return  '<span class="' + ratioStyle + '">' + data + '</span>';
				}
			},{
				"data": "level",
				"class": "coctdc",
				"render": function (data, type, row)  {
					return  '<span class="coclevel cocfont">' + data + '</span>';
				}
			}
		]       
		});
		$(this).on( "click", "td.cocname", function(event) {
		    var tr = $(this).closest('tr');
		    var row = dt.row(tr);

			if ( row.child.isShown() ) {
				row.child.hide();
				tr.removeClass('shown');
			} else {
				row.child( insFormatDataTables(row.data()) ).show();
				tr.addClass('shown');
			}
		});
    };

    $.fn.CoCWar = function() {

	var dt = null;
	var counter = 1;
        var detailRows = [];
	cocLocalizeDtUrl  = '/assets/js/dataTablesLang/lang.' + autoLanguage + '.json';

	window.resultCoCWarTdStyle = function(res) {
	     var brStyle = '2px solid ';
             switch(res) {
                case "win": {
                   return brStyle + '#60CC60';
                }
                case "lose": {
                   return brStyle + '#E04E49';
                }
                default: {
                   return brStyle + '#4955E0';
                }
             }
	};
	window.resultCoCWarText = function(res) {
             var insResult = '<span class="cocfont" style="color:';
             switch(res) {
                case "win": {
                   insResult += 'rgb(149, 226, 149)">' + $.fn.LangFormatData('warwin','Win!');
                   break;
                }
                case "lose": {
                   insResult += 'rgb(243, 178, 176)">' + $.fn.LangFormatData('warlose','Lose');
                   break;
                }
                default: {
                   insResult += 'rgb(247, 86, 83)">' + $.fn.LangFormatData('warundef','Undefined');
                   break;
                }
             }
	     return insResult + '</span>';
	};
        dt = $(this).DataTable({
		"deferRender": true,
                "stateSave": true,
                "lengthMenu": [[100, 50, 25, 10, -1], [100, 50, 25, 10, "All"]],
                "order": [[ 1, "desc" ]],
                "ajax": {
                 	url: '/clan/warlog/',
			async: true,
                 	dataFilter: function(data) {
                 		var json = jQuery.parseJSON(data);
		                if (json.error > 0)
                 		{
		                	json.recordsTotal = 0;
					json.recordsFiltered = 0;
					json.data = [];
					$.fn.ErrorsOn(json.error, json.msg);
					return JSON.stringify(json);
				}
			return data;
			},
			error: function(jqXHR, msg) {
				$.fn.ErrorsOn(jqXHR.status, msg);
				return null;
			}
                 },
                 "language": {
                 	"url": cocLocalizeDtUrl
                 },
                 "initComplete": function () {
                 	$.fn.CoCLanguage("cocclan");
                 },
                 "rowCallback": function( row, data ) {
			$(row).attr('id','rid'+counter);
			counter++;
                 },
		 "createdRow": function(row, data, dataIndex) {
			$(row).find('td').css('border-bottom', resultCoCWarTdStyle(data.result));
                 },
                 "columns": [
                 	{
				"data":       null,
				"class":      "coccount",
				"orderable":  false,
				"searchable": false,
				"render": function (data, type, row)  {
					return counter;
				}
			},{
				"data": "dtend",
				"class": "coctdl",
				"render": function (data, type, row)  {
					var arr = data.split(' ');
					return  '<center><span class="cocwardate">'+ arr[0] +'</span><br/>' +
						'<span class="cocwartime">'+ arr[1] +'</span></center>';
				}
			},{
				"data": "result",
				"class": "coctdl",
				"render": function (data, type, row)  {
					return  resultCoCWarText(data);
				}
			},{
				"data": "cattacks",
				"class": "coctdl",
				"render": function (data, type, row)  {
					return  '<span class="cocwarnumber">'+ data +'</span>';
				}
			},{
				"data": "cdestruct",
				"class": "coctdl",
				"render": function (data, type, row)  {
					return  '<span class="cocwarnumber">'+ data +'%</span>';
				}
			},{
				"data": "cexp",
				"class": "coctdl",
				"render": function (data, type, row)  {
					return  '<span class="cocwarnumber">'+ data +'</span>';
				}
			},{
				"data": "cstars",
				"class": "coctdl",
				"render": function (data, type, row)  {
					return  '<span class="cocwarnumber">'+ data +'</span>';
				}
			},{
				"data": null,
				"width": "15%",
				"orderable":  false,
				"searchable": false,
				"render": function (data, type, row)  {
					return  '<div style="float:right;margin:10px;">' +
						'<span class="cocwarbadge" style="background:url('+ row.cico +') no-repeat;background-size:cover;"></span>' +
						'<span class="cocwarlevel">'+ row.clevel +'</span>'  +
						'<span class="cocwarname">'+ row.cname +'</span></div>';
				}
			},{
				"data": null,
				"orderable":  false,
				"searchable": false,
				"render": function (data, type, row)  {
					return  '<div class="cocwarvs"><center>'+ row.members +'/'+ row.members +'</center></div>';
				}
			},{
				"data": null,
				"width": "15%",
				"orderable":  false,
				"render": function (data, type, row)  {
					return  '<div style="float:left;margin:10px;">' +
						'<span class="cocwarbadge" style="background:url('+ row.opico +') no-repeat;background-size:cover;"></span>' +
						'<span class="cocwarlevel">'+ row.oplevel +'</span>'  +
						'<span class="cocwarname">'+ row.opname +'</span></div>';
				}
			},{
				"data": "opattacks",
				"class": "coctdl",
				"render": function (data, type, row)  {
					return  '<span class="cocwarnumber">'+ data +'</span>';
				}
			},{
				"data": "opdestruct",
				"class": "coctdl",
				"render": function (data, type, row)  {
					return  '<span class="cocwarnumber">'+ data +'%</span>';
				}
			}
		]       
		});
    };

    $.fn.CoCDonation = function() {

    var dataArray = [];
    google.charts.load('current', {packages: ['corechart']});
    window.reDrawData = function() {
       google.charts.setOnLoadCallback(drawPieChart);
    };
    function drawPieChart() {
        if ((cocGlobSelector == 'null') || (cocGlobSelector == '')) { return; }
        var langTag = 'donation' + cocGlobSelector;
        var pieopt = {
          'title': $.fn.LangFormatData('donationtitle','Best donation in clan (TOP 5)') + ' - ' + $.fn.LangFormatData(langTag,'?'),
          'width': (($(document).width()/100)*80),
          'height': (($(document).height()/100)*65),
          'pieHole':0.4,
          'legend':{'position':'left','textStyle':{'color':'#D8B028','fontSize':13,'bold':true}},
          'titleTextStyle':{'color':'#404040','fontSize':14}
        };
        $.getJSON('/clan/donation/' + cocGlobSelector + '/' + cocDateUrl + cocUserUrl, function() {})
          .done(function(items) {
	    if (items.error != 0) {
		$.fn.ErrorsOn(400, items.msg);
		return;
	    }
	    $.fn.ErrorsOff();
	    dataArray = [];
            switch(cocGlobSelector) {
                case 'total':  {
		    dataArray.push([
			$.fn.LangFormatData('donationsend','Donation send'),
                    	$.fn.LangFormatData('donationreceive','Donation receive')
		    ]);
		    dataArray.push([
			$.fn.LangFormatData('donationsend','Donation send'),
			((typeof items.data[0] !== 'undefined') ? items.data[0].tsend : 0)
		    ]);
		    dataArray.push([
                    	$.fn.LangFormatData('donationreceive','Donation receive'),
                    	((typeof items.data[0] !== 'undefined') ? items.data[0].treceive : 0)
		    ]);
                    break;
                }
                default:  {
		    dataArray.push([
			$.fn.LangFormatData('th2','nik name'),
                    	$.fn.LangFormatData('donation' + cocGlobSelector,'Donation ' + cocGlobSelector)
		    ]);
		    $.each(items.data, function(idx, val) {
			dataArray.push([val.nik, val.send ]);
		    });
                    break;
                }
            }
            var cdata = new google.visualization.arrayToDataTable(dataArray);
            var chart = new google.visualization.PieChart(document.getElementById('piechartdiv'));
            chart.draw(cdata, pieopt);
        });
      }
      reDrawData();
    };

    $.fn.CoCnotify = function() {

	var sse = null;
	var divele = this;
	var isSseSetup = false;
	var eventSseSet = null;
	var eventStorage = 'CoCEventSet';

	$.fn.NotifyDataUpdate = function(event, value, isfmt) {
		var strout = Base64.decode(value);
		if (isfmt)
		{
			var json = jQuery.parseJSON(strout);
			strout = 
				$.fn.LangFormatData('fmt' + json.id,'{0}&nbsp;{1}&nbsp;{2}&nbsp;{3}&nbsp;{4}&nbsp;{5}').Format(
					json.name.AddSpan("notifyname"),
					json.vold.AddSpan("notifyvold"),
					json.vnew.AddSpan("notifyvnew"),
					json.vres.AddSpan("notifyvres"),
					json.vs.AddSpan("notifyvs"),
					$.fn.NotifyNumberSet(json)
				);
		}
		$.fn.NotifyTextUpdate(event, strout);
	    };
	$.fn.NotifyTextUpdate = function(event, value) {
		var date = new Date();
		var ele = $('<div/>', {
			id: event,
			html: (date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds()).AddSpan("notifydate") + value,
			class: 'notifytext'
		});
		$(divele).prepend(ele);
	    };
	$.fn.NotifyStatusUpdate = function(ele, value) {
		var date = new Date();
		$(ele).html(
			(date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds()).AddSpan("notifydate") + value
		);
	    };
	$.fn.NotifyNumberSet = function(value) {
		var str = "";
		if (
		    (value.id.endsWith("ChangeLevel")) ||
		    (value.id.endsWith("ChangeTrophies")) ||
		    (value.id.endsWith("ChangePoints")) ||
		    (value.id.endsWith("ChangeMembers")) ||
		    (value.id.endsWith("ChangeWarWin")) ||
		    (value.id.endsWith("ChangeWarSeries")) ||
		    (value.id.endsWith("ChangeDonationSend")) ||
		    (value.id.endsWith("MemberChangeDonationReceive"))
		   )
		{
			var vcalc = parseInt(value.vcalc);
			if (vcalc === 0) {
				str = '<span class="numequal">' +value.vcalc + '</span>';
			} else if (vcalc > 0) {
				str = '<span class="numup">' +value.vcalc + '</span>';
			} else if (vcalc < 0) {
				str = '<span class="numdown">' +value.vcalc + '</span>';
			} else {
				str = '<span class="numequal">=</span>';
			}
		}
		return str;
	    };
	$.fn.NotifyErrorPrint = function(event, txtid, value, update) {
		$.fn.ErrorsOn(
			event,
			'<span data-localize="' + txtid +'">' +
			$.fn.LangFormatData(txtid, value) +
			'</span>' + ((update !== null) ? '&nbsp;' + update : '')
		);
	    };
	$.fn.NotifySaveEvent = function(evid, state) {
		var result = $.grep(eventSseSet, function(e) { return e.name == evid; });
		if (result.length == 1) {
			result[0].check = state;
			localStorage.setItem(eventStorage, JSON.stringify(eventSseSet));
			if (state)
			{
				sse.addEventListener(evid, function(event) {
					$.fn.NotifyDataUpdate(evid, event.data, true);
				}, false);
			} else {
				sse.removeEventListener(evid, function(event) {
					$.fn.NotifyDataUpdate(evid, event.data, true);
				}, false);
			}
		}
	    };
	$.fn.NotifyInitEvent = function(source) {
		if (this.isSseSetup) {
			return;
		}
		eventSseSet = jQuery.parseJSON(localStorage.getItem(eventStorage));
		if (eventSseSet == null) {
			this.isSseSetup = true;
			eventSseSet = jQuery.parseJSON(Base64.decode(source));
			localStorage.setItem(eventStorage, JSON.stringify(eventSseSet));
		}
		$('#ssevent').html("");
		$.each(eventSseSet, function( key, value ) {
			var langid = 'notify' + value.name;
			var ele = $('<div/>', {
				id: value.name,
				html: 
					'<label><input type="checkbox" id="chkb-' + value.name + '" ' + ((value.check) ? 'checked="true"' : '') + ' value="">&nbsp;' +
					'<span data-localize="' + langid + '">' + 
					$.fn.LangFormatData(langid, value.desc) +
					'</span></label>',
				class: ((value.check) ? 'chkb-check' : 'chkb-uncheck')
			});
			$('#ssevent').append(ele);

			if (value.check)
			{
				sse.addEventListener(value.name, function(event) {
					$.fn.NotifyDataUpdate(value.name, event.data, true);
				}, false);
			}
		});
		$('#ssevent' + ' ' + ':checkbox').on('change', function() {
			var idele = this.id.substring(5);
			$('#' + idele).toggleClass('chkb-check').toggleClass('chkb-uncheck');
			$.fn.NotifySaveEvent(idele, (($(this).is(':checked')) ? true : false));
		});
		$('#notifyclear').on('click', function() {
			$(divele).html("");
		});
	};

	if (!!window.EventSource) {

		sse = new EventSource('/notify/sse/');

	        sse.onopen = function (event) {
			$.fn.NotifyStatusUpdate(
				'#cocnotifyinfo',
				$.fn.LangFormatData('notifyServerOpen','Connection Opened')
			);
	        };
	        sse.onerror = function (event) {
			if (event.eventPhase == EventSource.CLOSED) {
				var msgdefault = 'Connection Closed';
				$.fn.NotifyStatusUpdate(
					'#cocnotifyinfo',
					$.fn.LangFormatData('notifyServerClose', msgdefault)
				);
				$.fn.NotifyErrorPrint('ServerError','notifyServerClose', msgdefault, null);
			}
		};
		sse.onmessage = function (event) {
			$.fn.NotifyDataUpdate('Normal', event.data, true);
		};
		sse.addEventListener('EventSetup', function(event) {
			$.fn.NotifyInitEvent(event.data);
		}, false);
		sse.addEventListener('TestAlive', function(event) {
			console.log('+ TestAlive', Base64.decode(event.data));
		}, false);
		sse.addEventListener('ServerShutDown', function(event) {
			sse.close();
			$.fn.NotifyErrorPrint('ServerShutDown','notifyServerShutDown','Server ShutDown, stop any events', Base64.decode(event.data));
		}, false);

	} else {
		$.fn.NotifyErrorPrint('ServerError','notifyServerError','Your browser not support Server Side Events', null);
	}
    };

    $.fn.CoCDatePicker = function(updateMode, updateCallBack, dpYear, dpMonth, dpDay) {

	var ele = this;
	$.getScript('/assets/js/datePickerLang/bootstrap-datepicker.' + autoLanguage + '.min.js')
	  .done(function(script, textStatus) {
	    $.fn.CoCDatePickerLoad(ele, updateMode, updateCallBack, dpYear, dpMonth, dpDay);
	})
	  .fail(function(jqxhr, settings, exception) {
	    $.fn.ErrorsOn(jqxhr.status, exception);
	    $.fn.CoCDatePickerLoad(ele, updateMode, updateCallBack, dpYear, dpMonth, dpDay);
	});
    };
    $.fn.CoCDatePickerLoad = function(ele, updateMode, updateCallBack, dpYear, dpMonth, dpDay) {

	var curDate = new Date();
	dpYear  = ((typeof dpYear  === 'undefined' || dpYear === null)  ? curDate.getFullYear() : dpYear);
	dpMonth = ((typeof dpMonth === 'undefined' || dpMonth === null) ? (curDate.getMonth() + 1) : dpMonth);
	dpDay   = ((typeof dpDay   === 'undefined' || dpDay === null)   ? curDate.getDay() : dpDay);

	ele  = ((typeof ele  === 'undefined' || ele === null)  ? this : ele);
	updateCallBack = ((typeof updateCallBack === 'undefined' || updateCallBack === null)   ? '' : updateCallBack);
	cocDateUrl = ((updateMode == EnumUpdate.urlIrc) ? 
		(dpYear + '/' + dpMonth + '/' + dpDay) :
		(dpMonth + '/' + dpYear)
	);
	var eleinput = $(ele).closest('div').find('input');
	$(eleinput).val(cocDateUrl);
	cocDateUrl +=  '/';
	$('#srvdate').html(
		curDate.getFullYear() + '/' + (curDate.getMonth() + 1) + '/' + curDate.getDay()
	);
	$(ele).datepicker({    
		format: ((updateMode == EnumUpdate.urlIrc) ? "yyyy/mm/dd" : "mm/yyyy"),
    		startView: ((updateMode == EnumUpdate.urlIrc) ? 0 : 1),
    		minViewMode: ((updateMode == EnumUpdate.urlIrc) ? 0 : 1),
    		maxViewMode: 2,    
		autoclose: true,
    		toggleActive: true,    
    		clearBtn: true,    
    		todayBtn: "linked",
		todayHighlight: true,
		language: autoLanguage,
		endDate: "new Date()",
		orientation: "bottom right",
		defaultViewDate: { "year": dpYear, "month": dpMonth, "day": dpDay }
	});
        $(ele).on('changeDate', function() {
		$.fn.ErrorsOff();
		cocDateUrl = $(ele).datepicker('getFormattedDate') + '/';
		if (updateMode == EnumUpdate.urlIrc) {
			$(location).attr('href','/irclog/' + cocDateUrl);
		} else if (typeof window[updateCallBack] === 'function') {
		        window[updateCallBack]();
		} else {
			$.fn.ErrorsOn(400, "Javascript library is corrupt");
		}
	});
    };

    $.fn.CoCLanguage = function(table) {

	$("[data-localize]").localize(
		table,
		{
		   pathPrefix: cocLocalizeLangUrl,
		   skipLanguage: ["en", "en-US"]
		}
	);
    };

} (jQuery));
