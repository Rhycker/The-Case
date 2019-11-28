using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float movementForce;
	[SerializeField] private Animator animator;

	private new Rigidbody2D rigidbody;

	public void MoveHorizontal() {
		float horizontalInput = GameInput.Instance.Service.Horizontal();
		if (horizontalInput == 0) { return; }

		Vector2 force = new Vector2(horizontalInput * movementForce, 0f);
		rigidbody.AddForce(force);
	}

	private void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		animator.SetFloat("horizontalMovement", rigidbody.velocity.x);
	}

}