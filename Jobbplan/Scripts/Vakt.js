
function taLedigVakt() {

    $('body').on('click', '.btn', function () {
        var id = this.id;
        $.ajax({
            url: '/api/VaktApi/' + id,
            type: 'PUT',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                alert('Godkjent!');
                $('#calendar').fullCalendar('refetchEvents');
            },
            error: function (x, y, z) {
                //alert(x + '\n' + y + '\n' + z);
            }
        });
    })
};
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
                $('#calendar').fullCalendar('refetchEvents');
            },
            error: function (x, y, z) {
                //alert(x + '\n' + y + '\n' + z);
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
                data: function () { // a function that returns an object
                    return {
                        id: $('#selectProsjekt').val(),
                    };

                },
                error: function () {
                    $('#feil').html("<div class='alert alert-warning alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Du er ikke medlem av noen jobber enda!</strong> Gå til <a href='/Prosjekt/Index' class='alert-link'>Jobb</a> for å legge til en jobb</div>");

                    $('#script-warning').show();
                }
            },

            loading: function (bool) {
                $('#loading').toggle(bool);
            },
            eventClick: function (event, jsEvent, view) {
                var start = event.start;
                var end = event.end;
                $('#modalTitle').html(event.title);
                $('#modalBody').html(event.title);
                if (event.Brukernavn == null)
                { event.Brukernavn = "Ledig vakt";}
                $('#modalBody').append(": " + event.Brukernavn);

                $('#modalBody').append(start.format(" HH:mm"+"-"));
                $('#modalBody').append(end.format("HH:mm"));
                $('.modal-footer').html("<button id='" + event.VaktId + "' class='btn'>Ta vakt</button>");                               
                $('#eventUrl').attr('href',event.url);
                $('#fullCalModal').modal();
            
                // change the border color just for fun
                $(this).css('border-color', 'red');

            }
        });

    };


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
         
        }
    });
  
};

function VisAlle(brukere) {
    var strResult = "<option value="+0+"></option>";
    $.each(brukere, function (i, p) {
        strResult += "<option value="+p.BrukerId+">"+p.Navn+"</option>";
    });
    $("#brukere").html(strResult);
}

$(document).ready(function () {  
    fullcal();
    HentBrukere();
    selOnChange();
    taLedigVakt();
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



