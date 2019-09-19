using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	[SerializeField] private float movementSpeed;
	[SerializeField] private SpriteRenderer visuals;

	private void Update() {
		float movementInput = GameInput.Instance.InputService.Horizontal();
		if(movementInput == 0) { return; }

		visuals.flipX = movementInput < 0f;
		Vector2 movement = new Vector2();
		movement.x = movementInput * movementSpeed * Time.deltaTime;
		transform.Translate(movement);
	}

}