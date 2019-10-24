using UnityEditor;

[CustomEditor(typeof(DoorBlockingCharacter))]
public class DoorBlockingCharacterEditor : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		// make it so the required ID only shows if the GrantPassageManner is 'SpecificStartNodeRequires'
	}

}