using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

	public Room TargetRoom { get { return targetRoom; } }

	[SerializeField] private Room targetRoom;
	[SerializeField] private bool isOpen = true;

	private Room myRoom;

	public void Interact() {
		if (!isOpen) { return; }

		if(targetRoom == null) {
			Debug.LogWarning("There is no room available for this door...", transform);
			return;
		}

		RoomNavigation.Instance.EnterNextRoom(myRoom, targetRoom);
	}

	private void Awake() {
		myRoom = GetComponentInParent<Room>();
		if(myRoom == null) {
			Debug.LogWarning(transform.name + " does not have a Room parent! Please make sure this door is child of a room", transform);
		}
	}

}