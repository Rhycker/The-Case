using UnityEngine;

public class Ladder : MonoBehaviour {
	
	public float BottomYPosition { get { return climbingStartBottom.position.y; } }

	[SerializeField] private Transform climbingStartTop;
	[SerializeField] private Transform climbingStartBottom;
	[SerializeField] private Transform climbingDestinationTop;
	[SerializeField] private int steps;
	[SerializeField] private int topEndingStep;

	public Vector3 GetStartPosition(Vector3 position, out int startStep) {
		float distanceToTop = Mathf.Abs(position.y - climbingStartTop.position.y);
		float distanceToBottom = Mathf.Abs(position.y - climbingStartBottom.position.y);

		if (distanceToBottom > distanceToTop) {
			startStep = steps + 1;
			return climbingStartTop.position;
		}
		else {
			startStep = 0;
			return climbingStartBottom.position;
		}
	}

	public bool StopClimbing(int currentStep, float climbInput, out Vector2 endPosition) {
		if (currentStep == 0 && climbInput < 0f) {
			endPosition = climbingStartBottom.transform.position;
			return true;
		}
		else if(currentStep == topEndingStep && climbInput > 0f) {
			endPosition = climbingDestinationTop.transform.position;
			return true;
		}

		endPosition = Vector2.zero;
		return false;
	}
	
}