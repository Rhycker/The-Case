using UnityEditor;

[CustomEditor(typeof(DialogueUI))]
public class DialogUIEditor : Editor {

	private void OnEnable() {
		if (EditorApplication.isPlaying) { return; }
		DialogueUI targetScript = (DialogueUI)target;
		targetScript.Editor_SetUIActive(true);
	}

	private void OnDisable() {
		DialogueUI targetScript = (DialogueUI)target;
		if(targetScript == null) { return; }
		if(Selection.gameObjects.Length == 0) {
			targetScript.Editor_SetUIActive(false);
			return;
		}

		if(Selection.activeTransform.root != targetScript.transform) {
			targetScript.Editor_SetUIActive(false);
		}
	}

}