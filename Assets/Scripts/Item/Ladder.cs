using UnityEngine;

public class Ladder : MonoBehaviour {

	[SerializeField] private Transform climbingStartTop;
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

	public bool StopClimbing(Vector3 position, out Vector2 endPosition) {
		endPosition = Vector2.zero;

		if(position.y < climbingStartBottom.position.y) {
			endPosition = climbingStartBottom.transform.position;
			return true;
		}
		else if (position.y > climbingStartTop.position.y) {
			endPosition = climbingDestinationTop.transform.position;
			return true;
		}

		return false;
	}
	
}