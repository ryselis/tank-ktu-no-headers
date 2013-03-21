var prevEvent;
var prevState;
var prevEventTurret;
var prevStateTurret;
function MouseTankMove(event) {
    var xOff = 10;
    var yOff = 10;
	var positionX = $('#hidden_mouse_positionX').val();
	var positionY = $('#hidden_mouse_positionY').val();
	if (positionY >= 0) {
		var y = event.clientY;
		if (Math.abs(positionY - y) > yOff) {
            if (y < positionY) {
                MoveTankImageUp();
                if (event.shiftKey) {
                    tankMove('forward_fast', true);
                } else {
                    tankMove('forward', true);
                }
            } else {
                MoveTankImageDown();
                if (event.shiftKey) {
                    tankMove('back_fast', true);
                } else {
                    tankMove('back', true);
                }
            }
		}
	}
    if (positionX >= 0) {
        var x = event.clientX;
        if (Math.abs(positionX - x) > xOff) {
            if (x > positionX) {
                RotateTankImageClock();
                tankMove('right', true);
            }
            else {
                RotateTankImageCountClock();
                tankMove('left', true);
            }
        }
    }
}

function MouseTurretRotate(event) {
    //cancel bubble
    if (!event) var event = window.event;
    if (event) event.cancelBubble = true;
    if (event.stopPropagation) event.stopPropagation();

    position = $('#hidden_mouse_positionT').val();
    if (position >= 0) {
        var x = event.clientX;

        if (x > position) {
            RotateTurretImageClock();
            turretMove('right', true);
        }  else {
            RotateTurretImageCountClock();
            turretMove('left', true);
        }
    }
}

function Power(event) {
	$("#tankPower")
}

function SetInitPosition(event){
    $('#hidden_mouse_positionX').val(event.clientX);
    $('#hidden_mouse_positionY').val(event.clientY);
}

function SetInitPositionTurret(event) {
    $('#hidden_mouse_positionT').val(event.clientX);
}

function RevertTank(){
    $('#tankBody').css('bottom', '50px');
    $('#hidden_mouse_positionX').val(-1);
    $('#hidden_mouse_positionY').val(-1);
    $('#tankTurret').css('-moz-transform', 'rotate(0deg)');
    $('#tankTurret').css('-webkit-transform', 'rotate(360deg)');

    $('#tankBody').css('-moz-transform', 'rotate(0deg)');
    $('#tankBody').css('-webkit-transform', 'rotate(360deg)');
    RevertTankRotate();
    RevertTurret();
    RevertTankMove();
}
function RevertTurret() {
    $('#hidden_mouse_positionT').val(-1);

    turretMove('left', false);
    turretMove('right', false);
}

function RevertTankRotate() {
    $('#tankBody').css('-moz-transform', 'rotate(0deg)');
    $('#tankBody').css('-webkit-transform', 'rotate(360deg)');
    $('#hidden_mouse_positionX').val(-1);
    
   //tankMove('left', false);
}

function RevertTankMove(){
    $('#tankBody').css('bottom', '50px');
    $('#hidden_mouse_positionY').val(-1);
    //tankMove('forward', false);
}




function MoveTankImageUp() {
	$('#tankBody').css('bottom', '80px');
}

function MoveTankImageDown() {
	$('#tankBody').css('bottom', '30px');
}

function RotateTankImageClock() {
    /*    $('#tankTurret').positionedOffset($('#tankTurret'))*/
    $('#tankBody').css('-moz-transform', 'rotate(15deg)');
    $('#tankBody').css('-webkit-transform', 'rotate(15deg)');

}

function RotateTankImageCountClock() {
    $('#tankBody').css('-moz-transform',  'rotate(-15deg)');
    $('#tankBody').css('-webkit-transform','rotate(-15deg)');

}


function RotateTurretImageClock() {
/*    $('#tankTurret').positionedOffset($('#tankTurret'))*/
    $('#tankTurret').css('-moz-transform',  'rotate(15deg)');
	$('#tankTurret').css('-webkit-transform', 'rotate(15deg)');
}

function RotateTurretImageCountClock() {
	$('#tankTurret').css('-moz-transform',  'rotate(-15deg)');
	$('#tankTurret').css('-webkit-transform', 'rotate(-15deg)');
}


function request(req) {
    src='http://192.168.42.202/' + req;
    console.log(src);
    $.ajax({
    	url: src,
    	data: "",
        success: function(transport) {
            var response = transport['responseText'] || "no response text";
        },
        error: function() { console.log('Something went wrong...'); }
    });
}

function tankShoot(gun) {
    if (gun == "cannon_on") {
        request("main_gun/fire_simulate/on");
    }

}



function tankMove(move, state) {
	if (!(move == prevEvent && state == prevState)) {
		if (state) {
			switch(move) {
				case 'left':
					request('rotate/100');
					RotateTankImageCountClock();
					break;
				case 'right':
					request('rotate/160');
					RotateTankImageClock();
					break;
				case 'left_fast':
					request('rotate/0');
					RotateTankImageCountClock();
					break;
				case 'right_fast':
					request('rotate/255');
					RotateTankImageClock();
					break;
				case 'forward':
					request('move/94');
					MoveTankImageUp();
					break;
				case 'forward_fast':
					setPedalPosition(0);
					request('move/0');
					MoveTankImageUp();
					break;
				case 'back':
					request('move/160');
					MoveTankImageDown();
					break;
				case 'back_fast':
					setPedalPosition(0);
					request('move/255');
					MoveTankImageDown();
					break;
			}
		}
		// tank movement turn off

		else {
			console.log(move);
			switch(move) {
				case 'left':
					request('rotate/127');
					
					RevertTankRotate();
					break;
				case 'right':
					request('rotate/127');
					
					RevertTankRotate();
					break;
				case 'forward':
					request('move/127');
					
					RevertTankMove();
					break;
				case 'back':
					request('move/127');
					
					RevertTankMove();
					break;
			}
		}
		prevEvent = move;
		prevState = state;
	}
}


function turretMove(side, state){
	if (!(prevEventTurret == side && prevStateTurret == state)) {
	    var stateT;
	    if (state) {
	        stateT = "/on";
	    }   else {
	        stateT = "/off";
	    }
		request('turret/' + side + stateT);
		if (state) {
	        switch(side){
	            case 'left':
	                RotateTurretImageCountClock();
	                break;
	            case 'right':
	                RotateTurretImageClock();
	                break;
	        }
	    } else {
	        $('#tankTurret').css('-moz-transform', 'rotate(0deg)');
	        $('#tankTurret').css('-webkit-transform', 'rotate(360deg)');
	    }
    prevEventTurret = side;
    prevStateTurret = state;
   }
}


var shiftDown = false;
function setPedalPosition (pos) {
    if (pos == 1) {
        $('#speedPedal').css("background", "url(images/pedal.png) 0 94px");
        shiftDown = true;
    }   else {
        $('#speedPedal').css("background", "url(images/pedal.png) 0 0");
        shiftDown = false;
    }
}

