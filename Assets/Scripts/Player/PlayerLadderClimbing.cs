using UnityEngine;

public class PlayerLadderClimbing : MonoBehaviour {

	public bool isClimbingLadder { get; private set; }

	[SerializeField] private float ladderClimbSpeed;
	[SerializeField] private float minLadderClimbDistance;

	private new Rigidbody2D rigidbody;
	private Ladder currentLadder;

	public bool TraverseLadder() {
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

		Vector2 newPosition = (Vector2)transform.position + Vector2.up * verticalInput * ladderClimbSpeed * Time.fixedDeltaTime;
		Vector2 endPosition = Vector2.zero;
		if (currentLadder.StopClimbing(newPosition, out endPosition)) {
			rigidbody.MovePosition(endPosition);
			StopClimbingLadder();
			return false;
		}

		rigidbody.MovePosition(newPosition);
		return true;//moet ook nog bewegen en kunnen stoppen
	}

	private void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
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

	private void OnTriggerEnter2D(Collider2D collider) {
		Ladder ladder = collider.GetComponent<Ladder>();
		if (ladder == null) { return; }
		currentLadder = ladder;
	}

	private void OnTriggerExit2D(Collider2D collider) {
		Ladder ladder = collider.GetComponent<Ladder>();
		if (ladder == null) { return; }
		if (ladder != currentLadder) { return; }
		currentLadder = null;
	}

}