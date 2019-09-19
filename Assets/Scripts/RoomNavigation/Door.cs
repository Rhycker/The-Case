using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

	[SerializeField] private Room room;

	public void Interact() {
		if(room == null) {
			Debug.LogWarning("There is no room available for this door...", transform);
			return;
		}

		room.Enter();
	}
	
}