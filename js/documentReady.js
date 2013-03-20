$(document).ready(function() {
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

	$(document).mouseup(function(e){RevertTank()});
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