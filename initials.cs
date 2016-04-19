function doInitial() {
	%data = $AutoWarden::Initials;

	if(getRandom(0, 2)) {
		%str = "please";
	}

	%action = %data.actions.value[getRandom(0, %data.actions.length-1)];
	%str = trim(%str SPC %action.action);

	if(%action.usesSpot) {
		%spot = %data.spots.value[getRandom(0, %data.spots.length-1)];
		%str = trim(%str SPC "on the" SPC %spot.spot);

		if(%spot.isSmall) {
			commandToServer('spc', 0);
		} else {
			commandToServer('spc', 1);
		}
	}

	if(!getRandom(0, 3) && %action.usesSpot) {
		%facing = %data.facing.value[getRandom(0, %data.facing.length-1)];
		%str = trim(%str SPC "facing the" SPC %facing);
	}
	
	if(!getRandom(0, 3) && %action.usesSpot) {
		%touching = %data.touching.value[getRandom(0, %data.touching.length-1)];
		%str = trim(%str SPC "touching your" SPC %touching);
	}

	if(getRandom(0, 2)) {
		%time = %data.times.value[getRandom(0, %data.times.length-1)];
		%str = trim(%str SPC "by" SPC %time);
	}

	commandToServer('messageSent', strUpr(getSubStr(%str, 0, 1)) @ getSubStr(%str, 1, strLen(%str)) @ ".");
}