using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	private new Camera camera;

	private void Awake() {
		RoomNavigation.Instance.OnRoomEntered += OnRoomEntered;
		camera = GetComponent<Camera>();
	}

	private void OnRoomEntered(Room nextRoom) {
		camera.backgroundColor = nextRoom.BackgroundColor;
	}

}