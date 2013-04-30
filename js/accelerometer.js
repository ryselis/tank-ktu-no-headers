function onDeviceMotion(event){
	var accelerationX = event.accelerationIncludingGravity.x;
	var accelerationY = event.accelerationIncludingGravity.y;
	var accelerationZ = event.accelerationIncludingGravity.z;
	if (accelerationZ < -8){
		tankMove('forward', true);
		$("#state").text('forward true' + accelerationZ);
	}
	else{
		tankMove('forward', false);
		$("#state").text('forward false' + accelerationZ);
	}
	
	if (accelerationZ > 0){
		$("#state").text('back true' + accelerationZ);
		tankMove('back', true);
	}
	else{
		$("#state").text('back false' + accelerationZ);
		tankMove('back', false);
	}
	
	$('#stateLR').text(accelerationX);
	/*if (accelerationZ < -2){
		tankMove('back', true);
	}
	else{
		tankMove('back', false);
	}*/
}