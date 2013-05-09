movesForward = false;
movesBack = false;
movesLeft = false;
movesRight = false;
function onDeviceMotion(event) {
	if (accOn) {
		var accelerationX = event.accelerationIncludingGravity.x;
		var accelerationY = event.accelerationIncludingGravity.y;
		var accelerationZ = event.accelerationIncludingGravity.z;
		if (accelerationZ < -9.5) {
			tankMove('forward_fast', true)
			movesForward = true;
			//$("#stateF").text('forward_fast true');
		} else {
			if (accelerationZ < -8) {
				tankMove('forward', true);
				movesForward = true;
				//$("#stateF").text('forward true');
			} else if (movesForward) {
				tankMove('forward', false);
				movesForward = false;
				//$("#stateF").text('forward false');
			}
		}
		if (accelerationZ > 1.5) {
			//$("#stateB").text('back_fast true');
			tankMove('back_fast', true);
			movesBack = true;
		} else {
			if (accelerationZ > 0) {
				//$("#stateB").text('back true');
				tankMove('back', true);
				movesBack = true;
			} else if (movesBack){
				//$("#stateB").text('back false');
				tankMove('back', false);
				movesBack = false;
			}
		}

		if (accelerationY > 5.5) {
			//$('#stateL').text('left_fast true');
			tankMove('left_fast', true);
			movesLeft = true;
		} else { 
			if (accelerationY > 4) {
				//$('#stateL').text('left true');
				tankMove('left', true);
				movesLeft = true;
			} else if(movesLeft){
				//$('#stateL').text('left false');
				tankMove('left', false);
				movesLeft = false;
			}
		}
		
		if (accelerationY < -5.5) {
			//$('#stateR').text('right_fast true');
			tankMove('right_fast', true);
			movesRight = true;
		} else {
			if (accelerationY < -4) {
				//$('#stateR').text('right true');
				tankMove('right', true);
				movesRight = true;
			} else if(movesRight){
				//$('#stateR').text('right false');
				tankMove('right', false);
				movesRight = false;
			}
		}
	}
}
/*if (accelerationZ < -2){
 tankMove('back', true);
 }
 else{
 tankMove('back', false);s
 }*/
