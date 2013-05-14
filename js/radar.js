function startRadarAnimation() {
	    var frameHeight = 130;
	    var frames = 8;
	    var frame = 0;
	    setInterval(function () {
	        var frameOffset = (++frame % frames) * -frameHeight;
	        document.getElementById("radar").style.backgroundPosition = "0px " + frameOffset + "px";
	    }, 400);
}