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
	private float remainingStepDuration;

	private Ladder currentLadder;
	private int currentStep;
	private int targetStep;
	private Vector2 targetStepPosition;

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
		if (verticalInput == 0 && climbingState == ClimbingState.HoldStill) { return; }

		if(remainingStepDuration > 0f) {
			remainingStepDuration -= Time.deltaTime;
			if(remainingStepDuration > 0f) { return; }

			// target step reached
			rigidbody.MovePosition(targetStepPosition);
			currentStep = targetStep;
			if (verticalInput == 0) {
				climbingState = ClimbingState.HoldStill;
				return;
			}
		}

		// start climbing the new step, or stop climbing if we reached the end
		Vector2 endPosition = Vector2.zero;
		if (currentLadder.StopClimbing(currentStep, verticalInput, out endPosition)) {
			StopClimbingLadder(endPosition);
		}
		else {
			ContinueClimbing(verticalInput);
		}
	}

	private void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		climbSpeed = stepHeight / climbAnimation.length;
		climbingState = ClimbingState.None; 
	}

	private void ContinueClimbing(float verticalInput) {
		if (verticalInput == 0f) {
			Debug.LogWarning("Cant climb if the iput is zero! Aborting...");
			return;
		}

		if (verticalInput > 0f) {
			climbingState = ClimbingState.ClimbUp;
			targetStep = currentStep + 1;
		}
		else {
			climbingState = ClimbingState.ClimbDown;
			targetStep = currentStep - 1;
		}

		float targetStepYPosition = currentLadder.BottomYPosition + targetStep * stepHeight;
		targetStepPosition = new Vector2(transform.position.x, targetStepYPosition);		
		remainingStepDuration = climbAnimation.length;
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
			Vector2 startPosition = currentLadder.GetStartPosition(transform.position, out currentStep);
			transform.position = startPosition;
			climbingState = ClimbingState.HoldStill;
		});
	}

	private void StopClimbingLadder(Vector2 endPosition) {
		Action stopClimbing = () => {
			Debug.Log("Stop climbing");
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