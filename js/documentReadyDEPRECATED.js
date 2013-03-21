$(document).ready(function() {
	$('#tankBody').mousemove(function(e) {
		//console.log('trololo I move turret]')
		MouseTankMove(e)
	});
	$("tankShoot").mousedown(function(e) {
		console.log("tank is shooting");
		tankShoot("cannon_on");
	})
	$('#tankBody').mousedown(function(e) {
		SetInitPosition(e);
	});

	$('#tankTurret').mousemove(function(e) {
		MouseTurretRotate(e)
	});
	$('#tankTurret').mousedown(function(e) {
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