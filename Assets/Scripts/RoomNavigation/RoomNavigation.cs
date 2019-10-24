using UnityEngine;

public class RoomNavigation : MonoBehaviour {

	private static RoomNavigation instance;
	public static RoomNavigation Instance {
		get {
			if(instance == null) {
				instance = new GameObject("Room Navigation").AddComponent<RoomNavigation>();
			}
			return instance;
		}
	}

	public delegate void RoomEnterDelegate(Room nextRoom);
	public event RoomEnterDelegate OnRoomEntered;

	private Room currentRoom;

	public void EnterNextRoom(Room previous, Room next) {
		if(previous != currentRoom && currentRoom != null) {
			Debug.LogWarning("Invalid previous room: " + previous.name);
			return;
		}

		next.Prepare(previous);
		OnRoomEntered.Invoke(next);
		currentRoom = next;
	}
	
}