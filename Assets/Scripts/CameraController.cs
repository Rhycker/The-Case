using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField] private float floorYOffset = 3;

	private void Awake() {
		RoomNavigation.Instance.OnRoomEntered += OnRoomEntered;    
    }

	private void OnRoomEntered(Room nextRoom) {
		Vector3 targetPosition = new Vector3(nextRoom.OriginPosition.x, nextRoom.OriginPosition.y + floorYOffset, transform.position.z);
		transform.position = targetPosition;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.gray;
		Vector3 floorOffsetPosition = transform.position + Vector3.up * floorYOffset;
		Gizmos.DrawLine(transform.position, floorOffsetPosition);
	}

}