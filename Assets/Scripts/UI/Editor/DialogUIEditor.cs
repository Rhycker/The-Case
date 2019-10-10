using UnityEditor;

[CustomEditor(typeof(DialogueUI))]
public class DialogUIEditor : Editor {

	private void OnEnable() {
		DialogueUI targetScript = (DialogueUI)target;
		targetScript.Editor_SetUIActive(true);
	}

	private void OnDisable() {
		DialogueUI targetScript = (DialogueUI)target;
		if(Selection.gameObjects.Length == 0) {
			targetScript.Editor_SetUIActive(false);
			return;
		}

		for(int i = 0; i < targetScript.transform.childCount; i++) {
			if(Selection.activeGameObject == targetScript.transform.GetChild(i).gameObject) {
				return;
			}
		}

		targetScript.Editor_SetUIActive(false);
	}

}