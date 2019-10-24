using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

	public Room TargetRoom { get { return targetRoom; } }

	[SerializeField] private Room targetRoom;
	[SerializeField] private DoorBlockingCharacter doorBlockingCharacter;
	[SerializeField] private GameEvent stopBlockingEvent;//only used if doorBlockingCharacter != null
	[SerializeField] private bool isLocked = false;

	private Room myRoom;

	public void Interact() {
		if(doorBlockingCharacter != null) {
			bool requireEventCompletion = stopBlockingEvent != null && Progression.Instance.EventIsCompleted(stopBlockingEvent);
			if (doorBlockingCharacter.BlockDoor || requireEventCompletion) {
				doorBlockingCharacter.ShowBlockDoorDialogue(OnAccessGranted);
				return;
			}
		}
		if (isLocked) { return; }

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

	private void OnAccessGranted() {
		RoomNavigation.Instance.EnterNextRoom(myRoom, targetRoom);
	}

}