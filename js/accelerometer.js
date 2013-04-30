function onDeviceMotion(event){
	var accelerationX = event.accelerationIncludingGravity.x;
	var accelerationY = event.accelerationIncludingGravity.y;
	var accelerationZ = event.accelerationIncludingGravity.z;
	if (accelerationZ > 2){
		tankMove('forward', true);
	}
	else{
		tankMove('forward', false);
	}
	if (accelerationX > 2){
		tankMove('left', true);
	}
	else{
		tankMove('forward', false);
	}
	if (accelerationY > 2){
		turretMove('vertical', true);
	}
	else{
		turretMove('vertical', false);
	}
}
