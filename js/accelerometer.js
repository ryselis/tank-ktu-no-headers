window.ondevicemotion = function(event){
	var accelerationX = event.accelerationIncludingGravity.x;
	var accelerationY = event.accelerationIncludingGravity.y;
	var accelerationZ = event.accelerationIncludingGravity.z;
	console.log(accelerationX, accelerationY, accelerationZ);
}
