using UnityEditor;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor {

	private Door targetScript;
	private Room selectedRoom;
	private bool doorIsOpen = true;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		targetScript = (Door)target;
		bool newDoorIsOpen = serializedObject.FindProperty("isOpen").boolValue;

		if (targetScript.TargetRoom != selectedRoom || newDoorIsOpen != doorIsOpen) {
			selectedRoom = targetScript.TargetRoom;
			doorIsOpen = newDoorIsOpen;

			string doorName = "DoorTo:" + targetScript.TargetRoom.name;
			if (!newDoorIsOpen) {
				doorName = doorName + " (closed)";
			}
			targetScript.transform.name = doorName;
		}
	}

}