$(document).keydown( function (e) {
    //if (!accelerometer_on){


    switch (e.keyCode) {
        case 16:        // Shift
            setPedalPosition(1);
            break;
    }
    switch (e.keyCode) {
        case Event.KEY_LEFT:
            if (e.shiftKey)
                tankMove('left_fast', true)
            else
                tankMove('left', true);
            break;
        case Event.KEY_RIGHT:
            if (e.shiftKey)
                tankMove('right_fast', true)
            else
                tankMove('right', true);
            break;
        case Event.KEY_DOWN:
            if (e.shiftKey)
                tankMove('back_fast', true)
            else
                tankMove('back', true);
            break;
        case Event.KEY_UP:
            if (e.shiftKey)
                tankMove('forward_fast', true)
            else
                tankMove('forward', true);
            break;
        case 65:        //A
            if (e.shiftKey)
                tankMove('left_fast', true)
            else
                tankMove('left', true);
            break;
        case 68:        //D
            if (e.shiftKey)
                tankMove('right_fast', true)
            else
                tankMove('right', true);
            break;
        case 83:        //S
            if (e.shiftKey)
                tankMove('back_fast', true)
            else
                tankMove('back', true);
            break;
        case 87:        //W
            if (e.shiftKey)
                tankMove('forward_fast', true)
            else
                tankMove('forward', true);
            break;
        case 188:       // ,
            turretMove('left', true);
            break;
        case 190:       // .
            turretMove('right', true);
            break;
        case 81:        //Q
            turretMove('left', true);
            break;
        case 69:        //E
            turretMove('right', true);
            break;
        case 17:        //Ctrl
            tankShoot('cannon_on');
            break;
        case 82:
            turretMove("vertical", true);
            break;

    }

});

$(document).keyup( function(e){
    switch (e.keyCode) {
        case Event.KEY_LEFT:
            tankMove('left', false);
            break;
        case Event.KEY_RIGHT:
            tankMove('right', false);
            break;
        case Event.KEY_DOWN:
            tankMove('back', false);
            break;
        case Event.KEY_UP:
            tankMove('forward', false);
            break;
        case 65:        //A
            tankMove('left', false);
            break;
        case 68:        //D
            tankMove('right', false);
            break;
        case 83:        //S
            tankMove('back', false);
            break;
        case 87:        //W
            tankMove('forward', false);
            break;
        case 81:        //Q
            RevertTurret();
            break;
        case 69:        //E
            RevertTurret();
            break;
        case 16:        // Shift
            setPedalPosition(0);
            break;
        case 188:       // ,
            RevertTurret();
            break;
        case 190:       // .
            RevertTurret();
            break;
        case 82:
            turretMove("vertical", false);
            break;
    }
});