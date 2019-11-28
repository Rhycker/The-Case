using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(PlayerMovement), typeof(PlayerLadderClimbing), typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

	private PlayerMovement movement;
	private PlayerLadderClimbing ladderClimbing;
	private new Rigidbody2D rigidbody;

	private void Awake() {
		movement = GetComponent<PlayerMovement>();
		ladderClimbing = GetComponent<PlayerLadderClimbing>();
		rigidbody = GetComponent<Rigidbody2D>();
		RoomNavigation.Instance.OnRoomEntered += OnRoomEntered;
	}

	private void FixedUpdate() {
		if (Inventory.Instance.IsShowing) { return; }
		bool traverseLadder = ladderClimbing.TraverseLadder();
		if (traverseLadder) { return; }
		movement.MoveHorizontal();
	}

	private void OnRoomEntered(Room nextRoom) {
		Vector3 targetPosition = new Vector3(nextRoom.PlayerSpawnPositionX, nextRoom.OriginPosition.y, transform.position.z);
		rigidbody.MovePosition(targetPosition);
	}

}