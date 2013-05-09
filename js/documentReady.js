$(document).ready(function() {
	document.ontouchmove = function(event){
    event.preventDefault();
	}
	$('#tank_body').mousemove(function(e) {
		console.log('trololo I move turret]')
		MouseTankMove(e)
	});
	$('#tank_body').mousedown(function(e) {
		SetInitPosition(e)
	});

	$('#turret').mousemove(function(e) {
		MouseTurretRotate(e)
	});
	$('#turret').mousedown(function(e) {
		SetInitPositionTurret(e)
	});

	$(document).mouseup(function(e){request('forward',false);});
	screenfull.onchange = function() {
		if (screenfull.isFullscreen == false)//&&(EnteringFullscreen==false))
		{
			FullScreenSlide.setValue(0.1);
		}
	}

	screenfull.onerror = function() {
		alert("Fullscreen API error");
	}
}); 