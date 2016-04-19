function doActivity(%override) {
	$AutoWarden::CurrScheduleDelay = 0;
	$AutoWarden::Schedules = 0;

	%activity = $AutoWarden::Activities.value[getRandom(0, $AutoWarden::Activities.length-1)];
	
	if(%override !$= "") {
		if(%override < $AutoWarden::Activities.length && %override >= 0) {
			%activity = $AutoWarden::Activities.value[%override];
		}
	}
	//%activity.dump();

	echo("doing" SPC %activity.name);

	if(%activity.keyExists["prerequisites"]) {
		echo("prerequisites exist");

		for(%i=0;%i<%activity.prerequisites.length;%i++) {
			scheduleAWCommand('messageSent', %activity.prerequisites.value[%i], 4000);
		}
	} else {
		echo("prerequisites do not exist");
	}

	scheduleAWCommand('messageSent', "---- We will be doing" SPC %activity.name SPC " ----", 5000);
	scheduleAWCommand('spc', %activity.value["collision"], 0);

	if(%activity.value["opinionated"]) {
		scheduleAWCommand('messageSent', "++" SPC $AutoWarden::OpinionatedString SPC "++", 5000);
	}

	if(%activity.keyExists["koses"]) {
		echo("KOSes exist");

		for(%i=0;%i<%activity.koses.length;%i++) {
			scheduleAWCommand('messageSent', "!!" SPC %activity.koses.value[%i] SPC $AutoWarden::KOSString SPC "!!", 3000);
		}
	} else {
		echo("KOSes do not exist");
	}
	
	if(%activity.keyExists["description"]) {
		echo("description exists");

		for(%i=0;%i<%activity.description.length;%i++) {
			if(%i == %activity.description.length-1) {
				if(%activity.keyExists["delay"]) {
					%delay = getRandom(%activity.delay.min, %activity.delay.max) * 1000;
				}
			}

			%line = %activity.description.value[%i];

			// .+RNG_VALUE.min015.max030.int5+.
			if(strStr(%line, ".+RNG_VALUE") != -1) {
				%full = getSubStr(%line, strStr(%line, ".+RNG_VALUE"), (strStr(%line, "+.") == -1 ? strLen(%line) : strStr(%line, "+.") - strStr(%line, ".+RNG_VALUE") + 2));
				talk(%full);

				// would be easier to just 000 every value honestly
				%rand_val = getRandom(getSubStr(%full, strStr(%full, "min")+3, 3), getSubStr(%full, strStr(%full, "max")+3, 3));
				if(strStr(%full, "int") != -1) {
					%mod = getSubStr(%full, strStr(%full, "int")+3, 1);
				}
				%rand_val = %rand_val - (%rand_val % (%mod > 0 ? %mod : 5));

				%line = strReplace(%line, %full, %rand_val);
			}

			if(strStr(%line, ".+PRISONER_NAME_PLURAL+.") != -1) {
				%line = strReplace(%line, ".+PRISONER_NAME_PLURAL+.", ($AutoWarden::PrisonerName["plural"] $= "" ? "prisoners" : $AutoWarden::PrisonerName["plural"]));
			}

			if(strStr(%line, ".+PRISONER_NAME+.") != -1) {
				%line = strReplace(%line, ".+PRISONER_NAME+.", ($AutoWarden::PrisonerName["singular"] $= "" ? "prisoner" : $AutoWarden::PrisonerName["singluar"]));
			}

			scheduleAWCommand('messageSent', %line, 3000 + %delay);
		}
	} else {
		echo ("description doesn't exist");
	}

	if(%activity.keyExists["delay"]) {
		scheduleAWCommand('messageSent', %activity.delay.message, 0);
	}
}
//doActivity();

function cancelActivities() {
	for(%i=0;%i<$AutoWarden::Schedules;%i++) {
		cancel($AutoWarden::Schedule[%i]);
	}
	commandToServer('messageSent', "==== CANCELLED AUTO WARDEN. PLEASE FREEZE WHERE YOU ARE. ====");
}

function listActivities() {
	for(%i=0;%i<$AutoWarden::Activities.length;%i++) {
		%activity = $AutoWarden::Activities.value[%i];

		NewChatSO.addLine("\c2" @ %i @ ".\c6" SPC %activity.name);
	}
}