using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float movementForce = default;
	[SerializeField] private SpriteRenderer visuals = default;
	[SerializeField] private float floorYOffset = default;
	[SerializeField] private LayerMask obstacleLayerMask = default;	
	[SerializeField] private Vector2 colliderCheckBoxSize = default;

	private new Rigidbody2D rigidbody;
	private bool flipXViewRight;

	private void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
		flipXViewRight = visuals.flipX;
		RoomNavigation.Instance.OnRoomEntered += OnRoomEntered;
	}

	private void OnRoomEntered(Room nextRoom) {
		Vector3 targetPosition = new Vector3(nextRoom.PlayerSpawnPositionX, nextRoom.OriginPosition.y + floorYOffset, transform.position.z);
		transform.position = targetPosition;
	}

	private void FixedUpdate() {
		float movementInput = GameInput.Instance.Service.Horizontal();
		if (movementInput == 0) { return; }

		bool lookLeft = movementInput < 0f;
		if (lookLeft) {
			visuals.flipX = !flipXViewRight;
		}
		else {
			visuals.flipX = flipXViewRight;
		}

		Vector2 force = new Vector2(movementInput * movementForce, 0f);
		rigidbody.AddForce(force);
	}

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		Gizmos.color = Color.gray;
		Vector3 floorHitPosition = transform.position - Vector3.up * floorYOffset;
		Gizmos.DrawLine(transform.position, floorHitPosition);
	}
#endif

}