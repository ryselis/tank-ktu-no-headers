function onDeviceMotion(event){
	var accelerationX = event.accelerationIncludingGravity.x;
	var accelerationY = event.accelerationIncludingGravity.y;
	var accelerationZ = event.accelerationIncludingGravity.z;
	if (accelerationZ > 2){
		tankMove('forward', true);
		$("#state").text('move forward');
	}
	else{
		tankMove('forward', false);
		$("#state").text('move back');
	}
	
}
