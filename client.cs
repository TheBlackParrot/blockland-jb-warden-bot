// i suppose you could consider this a good example usage of Jettison?

// IF YOU ARE RUNNING A VERSION OF BLOCKLAND GLASS BEFORE APRIL 20TH, 2016:
// you may see various namespace linkage errors, ignore them.

// $AutoWarden::OverruleJSON = 1;

if(!isFunction("readFileJSON") || $AutoWarden::OverruleJSON) {
	exec("./json.cs");
}

exec("./activities.cs");
exec("./initials.cs");

// order: prereqs, any commands, kos'es, activity name, activity description

$AutoWarden::KOSString = "is kill-on-sight for the duration of the activity.";
$AutoWarden::OpinionatedString = "This game is opinionated. Please head to point 1 if you would like to opt out.";

function loadWardenActivities() {
	if(jettisonReadFile("Add-Ons/Client_AutoWarden/activities.json")) {
		error("Parse error at" SPC $JSON::Index @ ":" SPC $JSON::Error);
		return;
	}

	$AutoWarden::Activities = $JSON::Value.activities;
}
if(!isObject($AutoWarden::Activities)) {
	loadWardenActivities();
}

function loadWardenInitials() {
	if(jettisonReadFile("Add-Ons/Client_AutoWarden/initials.json")) {
		error("Parse error at" SPC $JSON::Index @ ":" SPC $JSON::Error);
		return;
	}

	$AutoWarden::Initials = $JSON::Value;
}

if(!isObject($AutoWarden::Initials)) {
	loadWardenInitials();
}


function scheduleAWCommand(%command, %value, %delayAdd) {
	if(%command $= "END") {
		%sch_id = NewChatSO.schedule($AutoWarden::CurrScheduleDelay, addLine, "\c5!! END OF AUTO WARDEN OUTPUT !!");
	} else {
		%sch_id = schedule($AutoWarden::CurrScheduleDelay, 0, commandToServer, %command, %value);
	}

	if(%sch_id !$= "") {
		$AutoWarden::Schedule[$AutoWarden::Schedules] = %sch_id;
		$AutoWarden::Schedules++;
	}

	if(%delayAdd) {
		$AutoWarden::CurrScheduleDelay += %delayAdd;
	}
}