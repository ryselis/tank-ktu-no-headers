function startRadarAnimation() {
	    var frameHeight = 90;
	    var frames = 8;
	    var frame = 0;
	    setInterval(function () {
	        var frameOffset = (++frame % frames) * -frameHeight;
	        document.getElementById("radar").style.backgroundPosition = "0px " + frameOffset + "px";
	    }, 400);
}