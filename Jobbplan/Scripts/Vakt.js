
function taLedigVakt() {

    $('body').on('click', '.btnTavakt', function () {
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
    function EndreVakt() {
        $('body').on('click', '.btnEndreVakt', function () {
            var id = this.id;
            var endrevakt = {
                VaktId: id,
                start: $('#datetimepicker1').val(),
                end: $('#datetimepicker2').val(),
                title: $('#eTittel').val(),
                Beskrivelse: $('#eBeskrivelse').val(),
                ProsjektId: $('#selectProsjekt').val(),
                BrukerId: $('#ebrukere').val()
            };
            $.ajax({
                url: '/api/VaktApi2/',
                type: 'PUT',
                data: JSON.stringify(endrevakt),
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
    }
    function VisKnapper() {
        var text = $("#vaktEndring");
        var text2 = $("#visKnapp")
        if (text.is(':hidden')) {
            text.slideDown('500');

        }
        else {
            text.slideUp('500');
           }
    };
    function ProId() {
        var prosjektid =  $('#selectProsjekt').val();
        if ($("#selectProsjekt:selected").length) {
            prosjektid = ($(this).find(":selected").val()); 
           
        }
        
        return prosjektid;
    }
   

    function ProIdTEST() {
        var prosjektid = $("#radioProsjekt :radio:checked").val();
        if ($("#radioProsjekt:checked").length) {
            prosjektid = $("#radioProsjekt :radio:checked").val();

        }

        return prosjektid;
    }

    var kalendere = {
        alle: {
            url: '/api/VaktApi/',
            type: 'GET',
            dataType: 'json',
            data: function () { // a function that returns an object
                return {
                    id: $('#selectProsjekt').val()
                };
            },
            error: function () {
                $('#feil').html("<div class='alert alert-warning alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Du er ikke medlem av noen jobber enda!</strong> Gå til <a href='/Prosjekt/Index' class='alert-link'>Jobb</a> for å legge til en jobb</div>");

                $('#script-warning').show();
            }

        },
        brukers: {
            url: '/api/VaktApi2/',
            type: 'GET',
            dataType: 'json',
            data: function () { // a function that returns an object
                return {
                    id: $('#selectProsjekt').val()
                };
            },
            error: function () {
                $('#feil').html("<div class='alert alert-warning alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Du er ikke medlem av noen jobber enda!</strong> Gå til <a href='/Prosjekt/Index' class='alert-link'>Jobb</a> for å legge til en jobb</div>");

                $('#script-warning').show();
            }
        }
    };

    function fullcal() {
        $('#calendar').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
        
            displayEventEnd: true,
            eventLimit: true, // allow "more" link when too many events
            eventSources: [kalendere.alle, kalendere.brukers],

            loading: function (bool) {
                $('#loading').toggle(bool);
            },
            eventClick: function (event, jsEvent, view) {
                var start = event.start;
                var end = event.end;
                $('#modalTitle').html(event.title);
                $('#modalBody').html(event.title);
                if (event.Brukernavn == null) {
                    event.Brukernavn = "Ledig vakt";
                    $('#tavakt').html("<button id='" + event.VaktId + "' class='btn btn-primary btnTavakt'>Ta vakt</button>");
                }

                $('#modalBody').append(": " + event.Brukernavn);

                $('#modalBody').append(start.format(" HH:mm" + "-"));
                $('#modalBody').append(end.format("HH:mm"));

                $('#endrevakt').html("<button id='" + event.VaktId + "' class='btn btn-warning btnEndreVakt'>Endre vakt</button>");
                $('#slettvakt').html("<button id='" + event.VaktId + "' class='btn btn-danger btnSlettVakt'>Slett vakt</button>");
                $('#eventUrl').attr('href', event.url);
                $('#fullCalModal').modal();
                // $("#fullCalModal").html("");
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
};/*
    function selOnChangeTEST() {
       
    $('#radioProsjekt').on('change', function () {    
             HentBrukere();
        $('#calendar').fullCalendar('refetchEvents');
    });
};*/
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
    $(".brukere").html(strResult);
}

function HentProsjekter() {
    $.ajax({
        url: '/api/ProsjektApi/',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            VisAlleProsjekter(data);
        },
        error: function (x, y, z) {

        }
    });

};

function VisAlleProsjekter(prosjekter) {
    var strResult = "";
    $.each(prosjekter, function (i, p) {
        strResult += "<input type='radio' name='prosjekter' value="+ p.Id+" checked>"+ p.Arbeidsplass + "<br>";
    });
    $(".prosjekterTest").html(strResult);
    //$("input:radio[name=prosjekter]:first").attr('checked', true);
}
    
$(document).ready(function () {  
    
    HentBrukere();
    selOnChange();
    taLedigVakt();
    EndreVakt();
    HentProsjekter();
    fullcal();
    $('#calendar').fullCalendar('removeEventSource', kalendere.brukers);
    $('#calendar').fullCalendar('refetchEvents');
   // selOnChangeTEST();
    
    $("#visAlleVakter").click(function () {
        $('#visAlleVakter').prop('disabled', true);
        $('#mineVakter').prop('disabled', false);
        $('#calendar').fullCalendar('removeEventSource', kalendere.brukers);
        $('#calendar').fullCalendar('refetchEvents');
        $('#calendar').fullCalendar('addEventSource', kalendere.alle);
        $('#calendar').fullCalendar('refetchEvents');

    });
    $("#mineVakter").click(function () {
        $('#visAlleVakter').prop('disabled', false);
        $('#mineVakter').prop('disabled', true);
        $('#calendar').fullCalendar('removeEventSource', kalendere.alle);
        $('#calendar').fullCalendar('refetchEvents');
        $('#calendar').fullCalendar('addEventSource', kalendere.brukers);
        $('#calendar').fullCalendar('refetchEvents');
    });


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



