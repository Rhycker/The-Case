using UnityEngine;

[SelectionBase]
public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float movementForce = default;
	[SerializeField] private float ladderClimbSpeed = default;
	[SerializeField] private SpriteRenderer visuals = default;
	[SerializeField] private float floorYOffset = default;
	[SerializeField] private LayerMask obstacleLayerMask = default;	
	[SerializeField] private Vector2 colliderCheckBoxSize = default;
	[SerializeField] private float minLadderClimbDistance = default;

	private new Rigidbody2D rigidbody;
	private bool flipXViewRight;

	private Ladder currentLadder;
	private bool isClimbingLadder;

	private void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
		flipXViewRight = visuals.flipX;
		RoomNavigation.Instance.OnRoomEntered += OnRoomEntered;
	}

	private void OnRoomEntered(Room nextRoom) {
		Vector3 targetPosition = new Vector3(nextRoom.PlayerSpawnPositionX, nextRoom.OriginPosition.y + floorYOffset, transform.position.z);
		rigidbody.MovePosition(targetPosition);
	}

	private void FixedUpdate() {
		bool traverseLadder = TraverseLadder();
		if (traverseLadder) { return; }
		MoveHorizontal();
	}

	private bool TraverseLadder() {
		float verticalInput = GameInput.Instance.Service.Vertical();
		if (currentLadder == null) { return false; }
		if (isClimbingLadder && GameInput.Instance.Service.Horizontal() != 0) {
			StopClimbingLadder();
			return false;
		}
		if (verticalInput == 0) { return isClimbingLadder; }

		if (!isClimbingLadder) {
			Vector2 ladderPos = currentLadder.transform.position;
			if (Mathf.Abs(ladderPos.x - transform.position.x) > minLadderClimbDistance) { return false; }
			if (verticalInput > 0 && ladderPos.y < transform.position.y) { return false; }
			if (verticalInput < 0 && ladderPos.y > transform.position.y) { return false; }

			StartClimbingLadder();
			return true;
		}

		Vector2 newPosition = (Vector2)transform.position + Vector2.up * verticalInput * ladderClimbSpeed * Time.deltaTime;
		Vector2 endPosition = Vector2.zero;
		if (currentLadder.StopClimbing(newPosition, out endPosition)) {
			rigidbody.MovePosition(endPosition);
			StopClimbingLadder();
			return false;
		}

		rigidbody.MovePosition(newPosition);
		return true;//moet ook nog bewegen en kunnen stoppen
	}

	private void StartClimbingLadder() {
		isClimbingLadder = true;
		rigidbody.isKinematic = true;
		rigidbody.velocity = Vector2.zero;
		Vector2 startPosition = currentLadder.GetStartPosition(transform.position);
		rigidbody.MovePosition(startPosition);
	}

	private void StopClimbingLadder() {
		isClimbingLadder = false;
		rigidbody.isKinematic = false;
	}

	private void MoveHorizontal() {
		float horizontalInput = GameInput.Instance.Service.Horizontal();
		if (horizontalInput == 0) { return; }

		bool lookLeft = horizontalInput < 0f;
		if (lookLeft) {
			visuals.flipX = !flipXViewRight;
		}
		else {
			visuals.flipX = flipXViewRight;
		}

		Vector2 force = new Vector2(horizontalInput * movementForce, 0f);
		rigidbody.AddForce(force);
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		Ladder ladder = collider.GetComponent<Ladder>();
		if(ladder == null) { return; }
		currentLadder = ladder;
	}

	private void OnTriggerExit2D(Collider2D collider) {
		Ladder ladder = collider.GetComponent<Ladder>();
		if (ladder == null) { return; }
		if (ladder != currentLadder) { return; }
		currentLadder = null;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		Gizmos.color = Color.gray;
		Vector3 floorHitPosition = transform.position - Vector3.up * floorYOffset;
		Gizmos.DrawLine(transform.position, floorHitPosition);
	}
#endif

}