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
	
	var l = false, r = false, b = false, f = false, tl = false, tr = false;
	$("#tankBody").touchwipe({
		wipeUp: function() {
    		if (f){
   			f = false;
    			tankMove('forward', false);
	   		} else {
				b = true;
    			tankMove('back', true);
			}
    	},
     	wipeDown: function() { 
     		if (b){
     			b = false;
     			tankMove('back', false);
     		} else {
				f = true;
     			tankMove('forward', true);
			}
     	},
     	preventDefaultEvents: true
	});
	
	$("#tankBottom").touchwipe({
	    wipeLeft: function() {  		
	    	if (r){
	    		r = false;
	    		tankMove('left', false);
	    	} else {
				l = true;
				tankMove('right', true);
	    	}
	    },
     	wipeRight: function() {
     		if (l){
     			l = false;
     			tankMove('right', false);
     		}  else {
				r = true;
    			tankMove('left', true);
			}
     	},
    	eventDefaultEvents: true
	});
	 
	
	$("#tankTurret").touchwipe({
		wipeLeft: function() {
			var event = window.event;
    		if (event) event.cancelBubble = true;
    		if (event.stopPropagation) event.stopPropagation();
			if (tr){
				tr = false;
				turretMove('right', false);
			} else {
				tl = true;
				turretMove('left', true);
			}
			
		},
     	wipeRight: function() {
     		var event = window.event;
    		if (event) event.cancelBubble = true;
    		if (event.stopPropagation) event.stopPropagation();
    		if (event.stopImmediatePropagation) event.stopImmediatePropagation();
     		if (tl){
     			tl = false;
     			turretMove('left', false);
     		} else {
     			tr = true;
     			turretMove('right', true);
     		}
     	},
		preventDefaultEvents: true
	});

	
	accOn = false;
	$("#accelerometerSwitch").mousedown(function(e) {
		tankMove('forward', false);
		tankMove('left', false);
		RevertTurret();
		if(!accOn) {
			$("#accelerometerSwitch").css("background", "url(images/tankPower.png) 0px 75px");
			accOn = true;
		} else {
			$("#accelerometerSwitch").css("background", "url(images/tankPower.png) 0 0");
			accOn = false;
		}
	});
	
	window.addEventListener("devicemotion",onDeviceMotion,false);
	})