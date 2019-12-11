using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerDetector : MonoBehaviour {

	public delegate void PlayerTouchDelegate();
	public event PlayerTouchDelegate OnPlayerEnter;
	public event PlayerTouchDelegate OnPlayerExit;

	public bool PlayerIsTouching { get { return player != null; } }
	public Vector2 PlayerPosition { get { return player.position; } }

	private Transform player;

	private void OnTriggerEnter2D(Collider2D collider) {
		if(collider.tag == "Player") {
			player = collider.transform;
			OnPlayerEnter?.Invoke();
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "Player") {
			player = null;
			OnPlayerExit?.Invoke();
		}
	}

}