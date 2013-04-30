function onDeviceMotion(event){
	var accelerationX = event.accelerationIncludingGravity.x;
	var accelerationY = event.accelerationIncludingGravity.y;
	var accelerationZ = event.accelerationIncludingGravity.z;
	if (accelerationZ - 4){
		tankMove('forward', true);
		$("#state").text('forward true',accelerationZ);
	}
	else{
		tankMove('forward', false);
		$("#state").text('forward false', accelerationZ);
	}
	/*if (accelerationZ < -2){
		tankMove('back', true);
	}
	else{
		tankMove('back', false);
	}*/
}