using UnityEngine;

public class Room : MonoBehaviour {

	[SerializeField] private Vector2 cameraStartPosition;

	public void Enter() {
		Vector3 cameraPosition = cameraStartPosition;
		cameraPosition.z = Camera.main.transform.position.z;
		Camera.main.transform.position = cameraPosition;
	}
	
}