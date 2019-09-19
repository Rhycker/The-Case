using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor {

	private void OnSceneGUI() {
		Camera mainCamera = Camera.main;
		float height = Camera.main.orthographicSize * 2;
		float width = Camera.main.aspect * height;
		Vector3 cameraViewSize = new Vector3(width, height, 0f);

		SerializedProperty positionProperty = serializedObject.FindProperty("cameraStartPosition");
		float cameraXOffset = positionProperty.vector2Value.x + width * 0.5f;
		float cameraYOffset = positionProperty.vector2Value.y + height * 0.5f;
		Vector3 localPosition = ((Room)target).transform.position;
		Vector3 cameraViewStart = new Vector3(localPosition.x + cameraXOffset, localPosition.y + cameraYOffset);

		Handles.color = Color.blue;
		Handles.DrawWireCube(cameraViewStart, cameraViewSize);
	}

}