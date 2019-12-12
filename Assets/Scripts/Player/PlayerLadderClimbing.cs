using System;
using UnityEngine;

public class PlayerLadderClimbing : MonoBehaviour {

	// Those values are synced with the animator
	private enum ClimbingState {
		None = -2,
		Transition = -1,
		HoldStill = 0,
		ClimbUp = 1,
		ClimbDown = 2
	}

	public bool IsClimbingLadder { get { return climbingState != ClimbingState.None; } }

	[SerializeField] private AnimationClip climbStartAnimation;
	[SerializeField] private AnimationClip climbStopAnimation;
	[SerializeField] private AnimationClip climbAnimation;
	[SerializeField] private float minDistanceToLadder;
	[SerializeField] private float stepHeight;

	private ClimbingState _climbingState;
	private ClimbingState climbingState {
		get { return _climbingState; }
		set {
			animator.SetInteger("climbingState", (int)value);
			_climbingState = value;
		}
	}

	private Animator animator;
	private new Rigidbody2D rigidbody;
	
	private float climbSpeed;

	private Ladder currentLadder;

	public bool StartClimbingLadder(float verticalInput) {
		if (IsClimbingLadder) { return false; }
		if (verticalInput == 0) { return false; }
		if (currentLadder == null) { return false; }
		Vector2 ladderPos = currentLadder.transform.position;
		if (Mathf.Abs(ladderPos.x - transform.position.x) > minDistanceToLadder) { return false; }
		if (verticalInput > 0 && ladderPos.y < transform.position.y) { return false; }
		if (verticalInput < 0 && ladderPos.y > transform.position.y) { return false; }

		bool stepUp = verticalInput > 0;
		StartClimbingLadder(stepUp);

		return true;
	}

	public void TraverseLadder(float verticalInput) {
		if (climbingState == ClimbingState.Transition) { return; }
		if (verticalInput == 0) {
			if (climbingState != ClimbingState.HoldStill) {
				rigidbody.velocity = Vector2.zero;
				climbingState = ClimbingState.HoldStill;
			}
			return;
		}

		// start climbing the new step, or stop climbing if we reached the end
		Vector2 endPosition = Vector2.zero;
		if (currentLadder.StopClimbing(transform.position.y, verticalInput, out endPosition)) {
			StopClimbingLadder(endPosition);
			return;
		}

		climbingState = verticalInput > 0 ? ClimbingState.ClimbUp : ClimbingState.ClimbDown;
		rigidbody.velocity = new Vector2(0f, verticalInput * climbSpeed);
	}

	private void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		climbSpeed = stepHeight / climbAnimation.length * 2;
		climbingState = ClimbingState.None; 
	}

	private void StartClimbingLadder(bool stepUp) {
		climbingState = ClimbingState.Transition;
		string transitionTrigger = "startClimbingDown";
		if (stepUp) {
			bool startRightFromLadder = transform.position.x > currentLadder.transform.position.x;
			transitionTrigger = startRightFromLadder ? "startClimbingUpFromRight" : "startClimbingUpFromLeft";
		}
		animator.SetTrigger(transitionTrigger);

		CoroutineHelper.WaitForSeconds(climbStartAnimation.length, () => {
			rigidbody.velocity = Vector2.zero;
			rigidbody.isKinematic = true;
			Vector2 startPosition = currentLadder.GetStartPosition(transform.position);
			transform.position = startPosition;
			climbingState = ClimbingState.HoldStill;
		});
	}

	private void StopClimbingLadder(Vector2 endPosition) {
		rigidbody.velocity = Vector2.zero;
		Action stopClimbing = () => {
			rigidbody.MovePosition(endPosition);
			climbingState = ClimbingState.None;
			rigidbody.isKinematic = false;
		};

		if (currentLadder.transform.position.y > transform.position.y) {
			stopClimbing();
		}
		else {
			animator.SetTrigger("stopClimbingUp");
			climbingState = ClimbingState.Transition;
			CoroutineHelper.WaitForSeconds(climbStopAnimation.length, stopClimbing);
		}
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