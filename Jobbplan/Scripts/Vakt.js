function taLedigVakt() {

    $('body').on('click', '.btnTavakt', function () {
        var id = $(this).attr("value");
        $.ajax({
            url: '/api/VaktApi/' + id,
            type: 'PUT',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                $('#malok').html("<div class='alert alert-success' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Forespørsel sendt.</div>");
                $('#calendar').fullCalendar('refetchEvents');
                $('#fullCalModal').modal('hide');
            },
            error: function (x, y, z) {
                $('#malok').html("<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Forespørsel ble ikke sendt.</div>");
            }
        });
    })
};
function OpprettMal() {
    $('body').on('click', '#LeggTilMal', function () {
        var $this = $(this);
        $this.attr('disabled', 'disabled').html("Legger til...");
        var mal = {        
            startTid: $('#timepicker10').val(),
            sluttTid: $('#timepicker9').val(),          
            Tittel: $('#MalTittel').val(),          
            ProsjektId: $('#selectProsjekt').val(),
        };

        $.ajax({
            url: '/api/Maler/',
            type: 'POST',
            data: JSON.stringify(mal),
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                $('#malok').html("<div class='alert alert-success' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Mal ble vellykket opprettet </div>");
                HentMaler();
                $this.removeAttr('disabled').html('Legg til mal');
              
            },
            error: function (data) {
                $('#malok').html("<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Det ble ikke opprettet mal. Er du sikker på at du valgte all nødvendig informasjon? </div>");
                $this.removeAttr('disabled').html('Legg til mal');
            }
        });
    })
};
function OpprettVakt() {
$('body').on('click', '#LeggTilVakt', function () {
    var endDate = false;
    if ($("#checkAllDay").is(':checked')) {
         endDate = true;
    }  // checked

    var $this = $(this);
    $this.attr('disabled', 'disabled').html("Legger til...");
    var vakt = {
        start: $('#datetimepicker4').val(),
        startTid: $('#timepicker6').val(),
        endTid: $('#timepicker7').val(),
        end: $('#datetimepicker5').val(),
        title: $('#Tittel').val(),
        Beskrivelse: $('#Beskrivelse').val(),
        ProsjektId: $('#selectProsjekt').val(),
        BrukerId: $('#brukere').val(),
        endDato: endDate
};
    
        $.ajax({
            url: '/api/VaktApi/',
            type: 'POST',
            data: JSON.stringify(vakt),
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                $this.removeAttr('disabled').html('Legg til vakt');
                $('#calendar').fullCalendar('refetchEvents');
            },
            error: function (data) {
                $('#vaktikkeok').html("<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Ingen vakt ble lagt til i kalenderen. Har du fylt ut all nødvendig informasjon? </div>");
                $this.removeAttr('disabled').html('Legg til vakt');
           }
        });
    })
}
function EndreVakt() {
    $('body').on('click', '.btnEndreVakt', function () {
        var endDate = false;
        if ($("#echeckAllDay").is(':checked')) {
            endDate = true;
        }  
        var id = $(this).attr("value");
        var endrevakt = {
            VaktId: id,
            start: $('#datetimepicker1').val(),
            startTid: $('#etimepicker6').val(),
            end: $('#datetimepicker2').val(),
            endTid: $('#etimepicker7').val(),
            title: $('#eTittel').val(),
            Beskrivelse: $('#eBeskrivelse').val(),
            ProsjektId: $('#selectProsjekt').val(),
            BrukerId: $('#ebrukere').val(),
            endDato: endDate
        };
        $.ajax({
            url: '/api/VaktApi2/',
            type: 'PUT',
            data: JSON.stringify(endrevakt),
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                $('#calendar').fullCalendar('refetchEvents');
                $('#fullCalModal').modal('hide');
            },
            error: function (x, y, z) {
                $('#malok').html("<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Vakten ble ikke endret.</div>");
            }
        });
    })
}
function SlettVakt() {
        $('body').on('click', '.btnSlettVakt', function () {
            var id = $(this).attr("value");
            
                $.ajax({
                    url: '/api/VaktApi/' + id,
                    type: 'DELETE',
                    contentType: "application/json;charset=utf-8",
                    success: function(data) {
                        $('#fullCalModal').modal('hide');
                        $('#calendar').fullCalendar('refetchEvents');
                    },
                    error: function(x, y, z) {
                        $('#malok').html("<div class='alert alert-danger' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>Vakten ble ikke slettet fra kalenderen.</div>");
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
        return prosjektid;
    }   
function ProIdTEST() {
    var prosjektid = $('#selectProsjekt').val();
    if ($("#selectProsjekt:selected").length) {
        prosjektid = ($(this).find(":selected").val());
    }
    return prosjektid;
    }
var kalendere = {
    Absoluttalle: {
        url: '/api/VaktApi/',
        type: 'GET',
        dataType: 'json'
        ,
        error: function () {
            $('#script-warning').show();
        }

    },
        alle: {
            url: '/api/VaktApi/',
            type: 'GET',
            dataType: 'json',
            data: function () { // a function that returns an object
                return {
                    id:  $("#selectProsjekt option:selected").val()
                };
            },
            error: function () {
                $('#script-warning').show();
            }

        },
        brukers: {
            url: '/api/VaktApi2/',
            type: 'GET',
            dataType: 'json',
            data: function () { // a function that returns an object
                return {
                    id: $("#selectProsjekt option:selected").val()
                };
            },
            error: function () {
              
                $('#script-warning').show();
            }
        },
        ledige: {
        url: '/api/VaktApi3/',
        type: 'GET',
        dataType: 'json',
        data: function () { // a function that returns an object
        return {
            id: $("#selectProsjekt option:selected").val()
        };
    },
    error: function () {
        
        $('#script-warning').show();
    }

    }
    };
function fullcal() {
        $('#calendar').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month'
            },
            editable: false,
            displayEventEnd: true,
            eventLimit: true, // allow "more" link when too many events
            eventSources: [kalendere.alle],
            loading: function (bool) {
                $('.loading').toggle(bool);
            },
            eventClick: function (event, jsEvent, view) {
                var start = event.start;
                var end = event.end;
                $('#modalTitle').html(event.title);
                $('#modalBody').html(event.title);
                if (event.Brukernavn == null) {
                    event.Brukernavn = "Ledig vakt";
                    $('#tavakt').html("<button value='" + event.VaktId + "' class='btn btn-primary btnTavakt'>Ta vakt</button>");
                }
                else { $('#tavakt').html(""); }
            
                $('#modalBody').append(start.format(" HH:mm" + "-"));
                $('#modalBody').append(end.format("HH:mm"));

                $('#endrevakt').html("<button value='" + event.VaktId + "' class='btn btn-warning btnEndreVakt'>Endre vakt</button>");
                $('#slettvakt').html("<button value='" + event.VaktId + "' class='btn btn-danger btnSlettVakt'>Slett vakt</button>");
                $('#eventUrl').attr('href', event.url);
                $('#fullCalModal').modal();
                // $("#fullCalModal").html("");
                // change the border color just for fun
                $(this).css('border-color', 'red');
                

            }
        });
    };
function selOnChange() {
    $(document).on('change', '#selectProsjekt', function () {
        HentBrukere();
        ErBrukerAdmin();
        HentMaler();
        $('#calendar').fullCalendar('refetchEvents');
    });
};
function selMalOnChange() {
    $('#maler').on('change', function () {
        var end = $(this).find(":selected").attr('id');
        var start = $(this).find(":selected").val();
        var tittel = $(this).find(":selected").text();
        if (start == 0) {
            start = "";
        }
        $('#timepicker6').val(start);
        $('#timepicker7').val(end);
        $('#Tittel').val(tittel);
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
    $(".brukere").html(strResult);
}
function HentMaler() {

    $.ajax({
        url: '/api/Maler/' + ProId(),
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            VisAlleMaler(data);
        },
        error: function (x, y, z) {

        }
    });

};
function VisAlleMaler(maler) {
    var strResult = "<option value=" + 0 + "></option>";
    $.each(maler, function (i, p) {
        strResult += "<option value=" + p.startTid + " id='"+p.sluttTid+"'>"+p.Tittel + "</option>";
    });
    $(".maler").html(strResult);
}
function HentProsjekter() {
    $.ajax({
        url: '/api/ProsjektApi/',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            VisAlleProsjekter(data);
            fullcal();
            ErBrukerAdmin();
            HentBrukere();
            HentMaler();
        },
        error: function (x, y, z) {
            $('#feil').html("<div class='alert alert-warning alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button><strong>Du er ikke medlem av noen bedrifter enda!</strong> Gå til <a href='/Prosjekt/Index' class='alert-link'>denne siden</a> for å legge til en jobb</div>");
            fullcal();
        }
    });

};
function VisAlleProsjekter(prosjekter) {
    var strResult = "<select class='form-control input-sm' id='selectProsjekt'>";
    $.each(prosjekter, function (i, p) {
        strResult += "<option value='"+p.Id+"'>" + p.Arbeidsplass + "</option>";
    });
    strResult += "</select>";
    $("#prosjektM").html(strResult);
    //$("input:radio[name=prosjekter]:first").attr('checked', true);
}
function Heldags() {
    $('#checkAllDay').on('change', function() {
        var text = $("#tilDato");
      
        if (text.is(':hidden')) {
            text.slideDown('500');
            text.val(" ");
            
        } else {
            text.slideUp('500');
            text.val("");
        }
    })
};
function EndreHeldags() {
    $('#echeckAllDay').on('change', function () {
        var text = $("#etilDato");

        if (text.is(':hidden')) {
            text.slideDown('500');

        } else {
            text.slideUp('500');
        }
    })
};
function ErBrukerAdmin() {

    $.ajax({
        url: '/api/ProsjektDeltakelseApi/' + ProId(),
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            DisplayVaktreg(data);
        },
        error: function (x, y, z) {

        }
    });

};
function DisplayVaktreg(admin) {
    var text = $("#mer");
    var text2 = $("#malReg");
    var text3 = $("#vaktRegistrering");
    var knapp = $("#malKnapp");
    var endre = $("#EndreVakt");
    var slett = $("#slettvakt");
    if (admin == true)
    {
        if (endre.is(':hidden')) {
            endre.slideDown('500');
        }
      
        if (slett.is(':hidden')) {
            slett.slideDown('500');
        }
        
        if (text.is(':hidden')) {
            text.slideDown('500');
        }
    }
    else {
        slett.slideUp('500');
        text.slideUp('500');
        text2.slideUp('500');
        text3.slideUp('500');
        knapp.slideUp('500');
        endre.slideUp('500');
    }
  
};
$(document).ready(function () {
    HentProsjekter();
    selMalOnChange();
    OpprettVakt();
    selOnChange();
    taLedigVakt();
    EndreVakt();
    SlettVakt();  
    Heldags();
    EndreHeldags();
    OpprettMal();
    
   
    $('#calendar').fullCalendar('removeEventSource', kalendere.brukers);
    $('#calendar').fullCalendar('refetchEvents');
 
    $("#visAlleVakter").click(function () {
        $('#visAlleVakter').prop('disabled', true);
        $('#mineVakter').prop('disabled', false);
        $('#ledigeVakter').prop('disabled', false);
        $('#alleMineVakter').prop('disabled', false);
        $('#calendar').fullCalendar('removeEventSource', kalendere.Absoluttalle);
        $('#calendar').fullCalendar('removeEventSource', kalendere.brukers);
        $('#calendar').fullCalendar('removeEventSource', kalendere.ledige);
        $('#calendar').fullCalendar('refetchEvents');
        $('#calendar').fullCalendar('addEventSource', kalendere.alle);
        $('#calendar').fullCalendar('refetchEvents');

    });
    
    $("#alleMineVakter").click(function () {
        $('#visAlleVakter').prop('disabled', false);
        $('#mineVakter').prop('disabled', false);
        $('#ledigeVakter').prop('disabled', false);
        $('#alleMineVakter').prop('disabled', true);
        $('#calendar').fullCalendar('removeEventSource', kalendere.alle);
        $('#calendar').fullCalendar('removeEventSource', kalendere.ledige);
        $('#calendar').fullCalendar('removeEventSource', kalendere.brukers);
        $('#calendar').fullCalendar('refetchEvents');
        $('#calendar').fullCalendar('addEventSource', kalendere.Absoluttalle);
        $('#calendar').fullCalendar('refetchEvents');
    });
    $("#mineVakter").click(function () {
        $('#alleMineVakter').prop('disabled', false);
        $('#visAlleVakter').prop('disabled', false);
        $('#mineVakter').prop('disabled', true);
        $('#ledigeVakter').prop('disabled', false);
        $('#calendar').fullCalendar('removeEventSource', kalendere.alle);
        $('#calendar').fullCalendar('removeEventSource', kalendere.ledige);
        $('#calendar').fullCalendar('removeEventSource', kalendere.Absoluttalle);
        $('#calendar').fullCalendar('refetchEvents');
        $('#calendar').fullCalendar('addEventSource', kalendere.brukers);
        $('#calendar').fullCalendar('refetchEvents');
    });
    $("#ledigeVakter").click(function () {
        $('#alleMineVakter').prop('disabled', false);
        $('#visAlleVakter').prop('disabled', false);
        $('#mineVakter').prop('disabled', false);
        $('#ledigeVakter').prop('disabled', true);
        $('#calendar').fullCalendar('removeEventSource', kalendere.alle);
        $('#calendar').fullCalendar('removeEventSource', kalendere.brukers);
        $('#calendar').fullCalendar('removeEventSource', kalendere.Absoluttalle);
        $('#calendar').fullCalendar('refetchEvents');
        $('#calendar').fullCalendar('addEventSource', kalendere.ledige);
        $('#calendar').fullCalendar('refetchEvents');
    });

    $("#malKnapp").click(function() {
        var text = $("#malReg");
        if (text.is(':hidden')) {
            text.slideDown('500');
            // $("#vaktNed").removeClass('glyphicon glyphicon-chevron-down');
            //$("#vaktNed").addClass('glyphicon glyphicon-chevron-up');
        } else {
            text.slideUp('500');
            //$("#vaktNed").removeClass('glyphicon glyphicon-chevron-up');
            //$("#vaktNed").addClass('glyphicon glyphicon-chevron-down');
        }
    });

    $("#mer").click(function () {
        var text = $("#vaktRegistrering");
        var text2 = $("#malKnapp");
        var text3 = $("#malReg");

        if (text.is(':hidden')) {
            text.slideDown('500');
            text2.slideDown('500');
            $("#vaktNed").removeClass('glyphicon glyphicon-chevron-down');
            $("#vaktNed").addClass('glyphicon glyphicon-chevron-up');
        }
        else {
            text2.slideUp('500');
            text.slideUp('500');
            text3.slideUp('500');
            $("#vaktNed").removeClass('glyphicon glyphicon-chevron-up');
            $("#vaktNed").addClass('glyphicon glyphicon-chevron-down');
        }
    });
});



