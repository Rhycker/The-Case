using UnityEditor;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor {

	private Door targetScript;
	private Room selectedRoom;
	private bool doorIsLocked = true;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		targetScript = (Door)target;
		bool newDoorIsLocked = serializedObject.FindProperty("isLocked").boolValue;

		if (targetScript.TargetRoom != selectedRoom || newDoorIsLocked != doorIsLocked) {
			selectedRoom = targetScript.TargetRoom;
			doorIsLocked = newDoorIsLocked;

			string doorName = "DoorTo:" + targetScript.TargetRoom.name;
			if (newDoorIsLocked) {
				doorName = doorName + " (locked)";
			}
			targetScript.transform.name = doorName;
		}
	}

}