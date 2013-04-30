$(document).ready(function() {
	$("#hidden_mouse_positionX").val("-1");
	$("#hidden_mouse_positionY").val("-1");
	$("#hidden_mouse_positionT").val("-1");
	$('#tankBody').mousemove(function(e) {
		MouseTankMove(e);
	});
	$('#tankBody').mousedown(function(e) {
		SetInitPosition(e);
	});

	$("#tankShoot").mousedown(function(e) {
		turretMove("cannon_on", true);
	})
	$("#tankShoot").mouseup(function(e) {
		turretMove("cannon_on", false);
	})

	var on = false;
	$("#tankPower").mousedown(function(e) {
		request("tank/on");
		request("tank/off");
		if(!on) {
			$("#tankPower").css("background", "url(images/tankPower.png) 0px 75px");
			on = true;
		} else {
			$("#tankPower").css("background", "url(images/tankPower.png) 0 0");
			on = false;
		}
	});

	$('#tankTurret').mousemove(function(e) {
		MouseTurretRotate(e);
	});
	$('#tankTurret').mousedown(function(e) {
		SetInitPositionTurret(e);
	});

	$(document).mouseup(function(e){
		tankMove('forward', false);
		tankMove('left', false);
		RevertTurret();
	});
	screenfull.onchange = function() {
		if (screenfull.isFullscreen == false)//&&(EnteringFullscreen==false))
		{
			FullScreenSlide.setValue(0.1);
		}
	}

	screenfull.onerror = function() {
		alert("Fullscreen API error");
	}
	
	$("#tankBody").touchwipe({
	    wipeLeft: function() {
	    //	RevertTurret();
	    	tankMove('left', true); 
	    },
     	wipeRight: function() {
     	//	RevertTurret();
     		tankMove('right', true);  
     	},
    	wipeUp: function() {
    	//	RevertTurret();
    		tankMove('back', true); 
    	},
     	wipeDown: function() { 
     	//	RevertTurret();
     		tankMove('forward', true); 
     	},
     	preventDefaultEvents: true
	}); 
	
	$("#tankTurret").touchwipe({
		wipeLeft: function() {
			tankMove('forward', false);
			tankMove('left', false);
			turretMove('left', true); 
		},
     	wipeRight: function() {
     		tankMove('forward', false);
			tankMove('left', false);
     		turretMove('right', true);  
     	},
		preventDefaultEvents: true,
		cancelBubble: true
	});
	window.addEventListener("devicemotion",onDeviceMotion,false);
	})