using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor {

	private void OnSceneGUI() {
		SerializedProperty positionProperty = serializedObject.FindProperty("cameraStartPosition");
		Vector3 localPosition = ((Room)target).transform.position;
		float cameraXOffset = localPosition.x + positionProperty.vector2Value.x;
		float cameraYOffset = localPosition.y + positionProperty.vector2Value.y;
		Vector3 cameraViewStart = new Vector3(cameraXOffset, cameraYOffset);

		Camera mainCamera = Camera.main;
		float cameraHeight = Camera.main.orthographicSize * 2;
		float cameraWidth = Camera.main.aspect * cameraHeight;
		Vector3 cameraViewSize = new Vector3(cameraWidth, cameraHeight, 0f);

		Handles.color = Color.blue;
		Handles.DrawWireCube(cameraViewStart, cameraViewSize);
	}

}