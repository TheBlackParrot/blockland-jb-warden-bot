// i suppose you could consider this a good example usage of Jettison?

// IF YOU ARE RUNNING A VERSION OF BLOCKLAND GLASS BEFORE APRIL 20TH, 2016:
// you may see various namespace linkage errors, ignore them.

// $AutoWarden::OverruleJSON = 1;

if(!isFunction("readFileJSON") || $AutoWarden::OverruleJSON) {
	exec("./json.cs");
}

exec("./activities.cs");
exec("./initials.cs");
exec("./names.cs");

// order: prereqs, any commands, kos'es, activity name, activity description

$AutoWarden::KOSString = "is kill-on-sight for the duration of the activity.";
$AutoWarden::OpinionatedString = "This game is opinionated. Please head to point 1 if you would like to opt out.";

function loadWardenData() {
	if(jettisonReadFile("Add-Ons/Client_AutoWarden/activities.json")) {
		error("Parse error at" SPC $JSON::Index @ ":" SPC $JSON::Error);
		return;
	}

	$AutoWarden::Activities = $JSON::Value.activities;

	if(jettisonReadFile("Add-Ons/Client_AutoWarden/initials.json")) {
		error("Parse error at" SPC $JSON::Index @ ":" SPC $JSON::Error);
		return;
	}

	$AutoWarden::Initials = $JSON::Value;

	if(jettisonReadFile("Add-Ons/Client_AutoWarden/madlibs.json")) {
		error("Parse error at" SPC $JSON::Index @ ":" SPC $JSON::Error);
		return;
	}

	$AutoWarden::MadlibLines = $JSON::Value.lines;

	if(jettisonReadFile("Add-Ons/Client_AutoWarden/names.json")) {
		error("Parse error at" SPC $JSON::Index @ ":" SPC $JSON::Error);
		return;
	}

	$AutoWarden::Names = $JSON::Value.names;

	$AutoWarden::LoadedData = 1;
}
if(!$AutoWarden::LoadedData) {
	loadWardenData();
}

function getMadlibLine() {
	commandToServer('messageSent', $AutoWarden::MadlibLines.value[getRandom(0, $AutoWarden::MadlibLines.length-1)]);
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

package AWCommandDetection {
	function NMH_Type::send(%this) {
		%content = %this.getValue();

		switch$(getWord(%content, 0)) {
			case "!i":
				doInitial();
				%this.setValue("");

			case "!a":
				%this.setValue("");

				if(getWord(%content, 1) $= "list") {
					listActivities();
					return;
				}
				if(getWord(%content, 1) $= "cancel") {
					cancelActivities();
					return;
				}
				
				doActivity(getWord(%content, 1));

			case "!mad":
				getMadlibLine();
				%this.setValue("");

			case "!gt":
				%item[0] = "life";
				%item[1] = "death";
				%item[2] = "choose 1 person to die";

				commandToServer('teamMessageSent', %item[getRandom(0, 2)]);

				%this.setValue("");

			case "!sc":
				commandToServer('messageSent', "During every activity, I ask that all guards please stand clear of the prisoners unless you are designated.");
				%this.setValue("");

			case "!n":
				setPrisonerName(getWord(%content, 1));
				%this.setValue("");
		}

		return parent::send(%this);
	}
};
activatePackage(AWCommandDetection);