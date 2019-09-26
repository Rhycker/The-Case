using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	[SerializeField] private float movementSpeed = default;
	[SerializeField] private SpriteRenderer visuals = default;
	[SerializeField] private float floorYOffset = default;
	[SerializeField] private LayerMask obstacleLayerMask = default;	
	[SerializeField] private Vector2 colliderCheckBoxSize = default;

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
		Vector2 newPosition = (Vector2)transform.position + movement;
		bool willHitCollider = Physics2D.OverlapBox(newPosition, colliderCheckBoxSize, 0f, obstacleLayerMask);
		if (!willHitCollider) {
			transform.position = newPosition;
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		Gizmos.color = Color.gray;
		Vector3 floorHitPosition = transform.position - Vector3.up * floorYOffset;
		Gizmos.DrawLine(transform.position, floorHitPosition);
	}
#endif

}