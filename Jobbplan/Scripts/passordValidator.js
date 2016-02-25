function checkPasswordMatch() {
    var pass1 = document.getElementById('passord');
    var pass2 = document.getElementById('passord2');
    var message = document.getElementById('passordMatch');
    var goodColor = "#7FFF7F";
    var badColor = "#ff6666";

    if (pass1.value == pass2.value) {
        $("#ok").removeClass('glyphicon glyphicon-remove');
        $("#ok").addClass('glyphicon glyphicon-ok');
        pass2.style.backgroundColor = goodColor;
        message.style.color = goodColor;
        message.innerHTML = "Passordene er like."
    } else {
        $("#ok").removeClass('glyphicon glyphicon-ok');
        $("#ok").addClass('glyphicon glyphicon-remove');
        pass2.style.backgroundColor = badColor;
        message.style.color = badColor;
        message.innerHTML = "Passordene er ulike."
    }
}