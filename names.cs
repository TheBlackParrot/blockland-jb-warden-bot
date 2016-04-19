function setPrisonerName(%override) {
	%name = $AutoWarden::Names.value[getRandom(0, $AutoWarden::Names.length-1)];
	
	if(%override !$= "") {
		if(%override $= "clear") {
			commandToServer('messageSent', "You no longer have a name. You may be referred to as anything now.");
			$AutoWarden::PrisonerName["singular"] = "";
			$AutoWarden::PrisonerName["plural"] = "";
			return;
		} else {
			if(%override < $AutoWarden::Names.length && %override >= 0) {
				%name = $AutoWarden::Names.value[%override];
			}
		}
	}

	$AutoWarden::PrisonerName["singular"] = %name.singular;
	$AutoWarden::PrisonerName["plural"] = %name.plural;

	commandToServer('messageSent', "You are now referred to as" SPC %name.plural);
}