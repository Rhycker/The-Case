﻿using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	[SerializeField] private float movementSpeed;
	
	private void Update() {
		float movementInput = GameInput.Instance.InputService.Horizontal();
		Vector2 movement = new Vector2();
		movement.x = movementInput * movementSpeed * Time.deltaTime;
		transform.Translate(movement);
	}

}