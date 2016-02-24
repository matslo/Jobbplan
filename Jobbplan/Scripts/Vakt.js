
    function LeggTilVakt() {
        var vakt = {
            start: $('#datetimepicker4').val(),
            end: $('#datetimepicker5').val(),
            title: $('#Tittel').val(),
            Beskrivelse: $('#Beskrivelse').val(),
            ProsjektId: $('#selectProsjekt').val(),
            BrukerId: $('#brukere').val()
        };
        $.ajax({
            url: '/api/VaktApi/',
            type: 'POST',
            data: JSON.stringify(vakt),
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                alert('Godkjent!');
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    }


    function ProId() {
        var prosjektid =  $('#selectProsjekt').val();
        if ($("#selectProsjekt:selected").length) {
            prosjektid = ($(this).find(":selected").val()); 
           
        }
        
        return prosjektid;
    }

function fullcal() {
    
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        editable: true,
        displayEventEnd: true,
        eventLimit: true, // allow "more" link when too many events
        events: {
            url: '/api/VaktApi/',
            type: 'GET',
            dataType: 'json',
            data : function() { // a function that returns an object
                return {
                    id: $('#selectProsjekt').val(),
                };

            },
            error: function () {
                $('#script-warning').show();
            }
        },
            
        loading: function (bool) {
            $('#loading').toggle(bool);
        }

    });
}

function selOnChange() {
    $('#selectProsjekt').on('change', function () {
        HentBrukere();
        $('#calendar').fullCalendar('refetchEvents');
    });
};

function HentBrukere() {
      
    $.ajax({
        url: '/api/BrukerApi/' + ProId(),
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            VisAlle(data);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
  
};

function VisAlle(brukere) {
    var strResult = "";
    $.each(brukere, function (i, p) {
        strResult += "<option value="+p.BrukerId+">"+p.Navn+"</option>";
    });
    $("#brukere").html(strResult);
}

$(document).ready(function () {  
    fullcal();
    HentBrukere();
    selOnChange();
    $("#mer").click(function () {
        var text = $("#vaktRegistrering");
        if (text.is(':hidden')) {
            text.slideDown('500');
            $("#vaktNed").removeClass('glyphicon glyphicon-menu-up');
            $("#vaktNed").addClass('glyphicon glyphicon-menu-up');
        }
        else {
            text.slideUp('500');
            $("#vaktNed").removeClass('glyphicon glyphicon-menu-up');
            $("#vaktNed").addClass('glyphicon glyphicon-menu-down');
        }
    });
});



