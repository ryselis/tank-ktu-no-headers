function onDeviceMotion(event) {
	if (accOn) {
		var accelerationX = event.accelerationIncludingGravity.x;
		var accelerationY = event.accelerationIncludingGravity.y;
		var accelerationZ = event.accelerationIncludingGravity.z;
		if (accelerationZ < -9.5) {
			tankMove('forward_fast', true)
			//$("#stateF").text('forward_fast true');
		} else {
			if (accelerationZ < -8) {
				tankMove('forward', true);
				//$("#stateF").text('forward true');
			} else {
				tankMove('forward', false);
				//$("#stateF").text('forward false');
			}
		}
		if (accelerationZ > 1.5) {
			//$("#stateB").text('back_fast true');
			tankMove('back_fast', true);
		} else {
			if (accelerationZ > 0) {
				//$("#stateB").text('back true');
				tankMove('back', true);
			} else {
				//$("#stateB").text('back false');
				tankMove('back', false);
			}
		}

		if (accelerationY > 5.5) {
			//$('#stateL').text('left_fast true');
			tankMove('left_fast', true);
		} else { 
			if (accelerationY > 4) {
				//$('#stateL').text('left true');
				tankMove('left', true);
			} else {
				//$('#stateL').text('left false');
				tankMove('left', false);
			}
		}
		
		if (accelerationY < -5.5) {
			//$('#stateR').text('right_fast true');
			tankMove('right_fast', true);
		} else {
			if (accelerationY < -4) {
				//$('#stateR').text('right true');
				tankMove('right', true);
			} else {
				//$('#stateR').text('right false');
				tankMove('right', false);
			}
		}
	}
}
/*if (accelerationZ < -2){
 tankMove('back', true);
 }
 else{
 tankMove('back', false);
 }*/
