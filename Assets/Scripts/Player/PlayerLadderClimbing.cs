using UnityEngine;

public class PlayerLadderClimbing : MonoBehaviour {

	private enum ClimbingState {
		None,
		HoldStill,
		ClimbUp,
		ClimbDown
	}

	public bool IsClimbingLadder { get { return climbingState != ClimbingState.None; } }

	[SerializeField] private float climbSpeed;
	[SerializeField] private float minDistanceToLadder;
	[SerializeField] private float stepHeight;

	private ClimbingState climbingState;
	private new Rigidbody2D rigidbody;
	private Ladder currentLadder;
	private int currentStep;
	private float currentStepYPosition;
	private int targetStep;
	private float targetStepYPosition;

	public bool StartClimbingLadder(float verticalInput) {
		if (IsClimbingLadder) { return false; }
		if (verticalInput == 0) { return false; }
		if (currentLadder == null) { return false; }
		Vector2 ladderPos = currentLadder.transform.position;
		if (Mathf.Abs(ladderPos.x - transform.position.x) > minDistanceToLadder) { return false; }
		if (verticalInput > 0 && ladderPos.y < transform.position.y) { return false; }
		if (verticalInput < 0 && ladderPos.y > transform.position.y) { return false; }
		
		rigidbody.isKinematic = true;
		rigidbody.velocity = Vector2.zero;
		Vector2 startPosition = currentLadder.GetStartPosition(transform.position, out currentStep);
		rigidbody.MovePosition(startPosition);
		bool stepUp = verticalInput > 0;
		StartTraversingStep(stepUp);

		return true;
	}

	public void TraverseLadder(float verticalInput) {
		if (verticalInput == 0 && climbingState == ClimbingState.HoldStill) { return; }
		if (verticalInput != 0 && climbingState == ClimbingState.HoldStill) {
			UpdateClimbingState(verticalInput);
		}

		float climbDirection = climbingState == ClimbingState.ClimbUp ? 1f : -1f;
		Vector2 newPosition = (Vector2)transform.position + Vector2.up * climbDirection * climbSpeed * Time.fixedDeltaTime;
		Vector2 endPosition = Vector2.zero;
		if (currentLadder.StopClimbing(newPosition, out endPosition)) {
			rigidbody.MovePosition(endPosition);
			StopClimbingLadder();
			return;
		}
		
		if(climbingState == ClimbingState.ClimbUp && newPosition.y >= targetStepYPosition) {
			Debug.Log("DetermineDirection");
			currentStep = targetStep;
			UpdateClimbingState(verticalInput);
		}
		else if(climbingState == ClimbingState.ClimbDown && newPosition.y <= targetStepYPosition) {
			currentStep = targetStep;
			UpdateClimbingState(verticalInput);
		}

		if (climbingState == ClimbingState.None) {
			newPosition.y = targetStepYPosition;
		}
		rigidbody.MovePosition(newPosition);
	}

	private void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void UpdateClimbingState(float verticalInput) {
		if (verticalInput == 0f) {
			climbingState = ClimbingState.HoldStill;
		}
		else if (verticalInput > 0f) {
			StartTraversingStep(true);
		}
		else {
			StartTraversingStep(false);
		}
	}

	private void StartTraversingStep(bool stepUp) {
		Debug.Log("Start traversing step");
		targetStep = stepUp ? currentStep + 1 : currentStep - 1;
		climbingState = stepUp ? ClimbingState.ClimbUp : ClimbingState.ClimbDown;
		currentStepYPosition = currentLadder.BottomYPosition + currentStep * stepHeight;
		targetStepYPosition = currentLadder.BottomYPosition + targetStep * stepHeight;
	}

	private void StopClimbingLadder() {
		climbingState = ClimbingState.None;
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