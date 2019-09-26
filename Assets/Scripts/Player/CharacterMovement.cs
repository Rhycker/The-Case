using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	[SerializeField] private float movementSpeed;
	[SerializeField] private SpriteRenderer visuals;
	[SerializeField] private float floorYOffset;

	private void Awake() {
		RoomNavigation.Instance.OnRoomEntered += OnRoomEntered;
	}

	private void OnRoomEntered(Room nextRoom) {
		Vector3 targetPosition = new Vector3(nextRoom.PlayerSpawnPositionX, nextRoom.OriginPosition.y + floorYOffset, transform.position.z);
		transform.position = targetPosition;
	}

	private void Update() {
		float movementInput = GameInput.Instance.Service.Horizontal();
		if(movementInput == 0) { return; }

		visuals.flipX = movementInput < 0f;
		Vector2 movement = new Vector2();
		movement.x = movementInput * movementSpeed * Time.deltaTime;
		transform.Translate(movement);
	}

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		Gizmos.color = Color.gray;
		Vector3 floorHitPosition = transform.position - Vector3.up * floorYOffset;
		Gizmos.DrawLine(transform.position, floorHitPosition);
	}
#endif

}