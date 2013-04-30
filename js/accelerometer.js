function onDeviceMotion(event){
	var accelerationX = event.accelerationIncludingGravity.x;
	var accelerationY = event.accelerationIncludingGravity.y;
	var accelerationZ = event.accelerationIncludingGravity.z;
	if (accelerationZ > 2){
		tankMove('forward', true);
		$("#state").text(accelerationZ);
	}
	else{
		tankMove('forward', false);
		$("#state").text(accelerationZ);
	}
	/*if (accelerationZ < -2){
		tankMove('back', true);
	}
	else{
		tankMove('back', false);
	}*/
}