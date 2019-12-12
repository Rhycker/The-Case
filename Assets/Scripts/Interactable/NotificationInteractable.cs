using UnityEngine;

public class NotificationInteractable : MonoBehaviour {

	[SerializeField] private Sprite notificationSprite;

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Player") {
			Notification.Instance.Show(notificationSprite);
		}
	}

}