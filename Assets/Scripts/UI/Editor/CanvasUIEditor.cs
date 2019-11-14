using UnityEditor;

[CustomEditor(typeof(CanvasUI), true)]
public class CanvasUIEditor : Editor {

	private void OnEnable() {
		if (EditorApplication.isPlaying) { return; }
		CanvasUI targetScript = (CanvasUI)target;
		targetScript.ShowCanvas(true);
	}

	private void OnDisable() {
		CanvasUI targetScript = (CanvasUI)target;
		if(targetScript == null) { return; }
		if(Selection.gameObjects.Length == 0) {
			targetScript.ShowCanvas(false);
			return;
		}

		if(Selection.activeTransform.root != targetScript.transform) {
			targetScript.ShowCanvas(false);
		}
	}

}