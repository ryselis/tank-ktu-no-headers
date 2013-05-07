function onDeviceMotion(event){
	var accelerationX = event.accelerationIncludingGravity.x;
	var accelerationY = event.accelerationIncludingGravity.y;
	var accelerationZ = event.accelerationIncludingGravity.z;
	if (accelerationZ < -9.5){
		tankMove('forward_fast', true)
		$("#state").text('forward_fast true' + accelerationZ);
	}
	else{
	if (accelerationZ < -8){
		tankMove('forward', true);
		$("#state").text('forward true' + accelerationZ);
	}
	
	else{
		tankMove('forward', false);
		$("#state").text('forward false' + accelerationZ);
	}
	}
	
	if (accelerationZ > 0){
		$("#state").text('back true' + accelerationZ);
		tankMove('back', true);
	}
	else{
		$("#state").text('back false' + accelerationZ);
		tankMove('back', false);
	}
	
	if (accelerationY > 4){
		$('#stateLR').text('left true' + accelerationY);
		tankMove('left', true);
	}
	else{
		$('#stateLR').text('left false' + accelerationY);
		tankMove('left', false);
	}
	if (accelerationY < -4){
		$('#stateLR').text('right true' + accelerationY);
		tankMove('right', true);
	}
	else{
		$('#stateLR').text('right false' + accelerationY);
		tankMove('right', false);
	}
	/*if (accelerationZ < -2){
		tankMove('back', true);
	}
	else{
		tankMove('back', false);
	}*/
}