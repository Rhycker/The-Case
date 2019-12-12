using UnityEngine;

public class Ladder : MonoBehaviour {
	
	public float BottomYPosition { get { return climbingStartBottom.position.y; } }

	[SerializeField] private Transform climbingStartTop;
	[SerializeField] private Transform climbingEndTop;
	[SerializeField] private Transform climbingStartBottom;
	[SerializeField] private Transform climbingDestinationTop;

	public Vector3 GetStartPosition(Vector3 position) {
		float distanceToTop = Mathf.Abs(position.y - climbingStartTop.position.y);
		float distanceToBottom = Mathf.Abs(position.y - climbingStartBottom.position.y);

		if (distanceToBottom > distanceToTop) {
			return climbingStartTop.position;
		}
		else {
			return climbingStartBottom.position;
		}
	}

	public bool StopClimbing(float yPosition, float climbInput, out Vector2 endPosition) {
		if (yPosition <= BottomYPosition && climbInput < 0f) {
			endPosition = climbingStartBottom.transform.position;
			return true;
		}
		else if(yPosition >= climbingEndTop.position.y && climbInput > 0f) {
			endPosition = climbingDestinationTop.transform.position;
			return true;
		}

		endPosition = Vector2.zero;
		return false;
	}
	
}