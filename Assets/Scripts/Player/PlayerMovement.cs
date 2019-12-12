using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float movementForce;
	[SerializeField] private Animator animator;
	[SerializeField] private AnimationClip leanOverAnimation;

	private bool isLeaningOver;
	private new Rigidbody2D rigidbody;

	public void MoveHorizontal() {
		float horizontalInput = GameInput.Instance.Service.Horizontal();
		if (horizontalInput == 0) { return; }
		if (isLeaningOver) { return; }

		Vector2 force = new Vector2(horizontalInput * movementForce, 0f);
		rigidbody.AddForce(force);
	}

	private void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		animator.SetFloat("horizontalMovement", rigidbody.velocity.x);
	}

	private void OnCollisionStay2D(Collision2D collision) {
		if (collision.collider.tag != "Edge") { return; }
		bool hitPointIsRight = collision.contacts[0].point.x > transform.position.x;
		float horizontalInput = GameInput.Instance.Service.Horizontal();
		if ((hitPointIsRight && horizontalInput > 0f) || (!hitPointIsRight && horizontalInput < 0f)) {
			animator.SetBool("leanOver", true);
			isLeaningOver = true;
			CoroutineHelper.WaitOneFrame(() => {
				animator.SetBool("leanOver", false);
			});
			CoroutineHelper.WaitForSeconds(leanOverAnimation.length, () => {
				isLeaningOver = false;
			});
		}
	}

}