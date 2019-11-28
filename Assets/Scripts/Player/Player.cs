﻿using UnityEngine;

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
		float verticalInput = GameInput.Instance.Service.Vertical();
		if (!ladderClimbing.IsClimbingLadder) {
			ladderClimbing.StartClimbingLadder(verticalInput);
		}
		if (ladderClimbing.IsClimbingLadder) {
			ladderClimbing.TraverseLadder(verticalInput);
		}
		else {
			movement.MoveHorizontal();
		}
	}

	private void OnRoomEntered(Room nextRoom) {
		Vector3 targetPosition = new Vector3(nextRoom.PlayerSpawnPositionX, nextRoom.OriginPosition.y, transform.position.z);
		rigidbody.MovePosition(targetPosition);
	}

}