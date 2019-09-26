using System.Collections.Generic;
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

	public void EnterNextRoom(Room previous, Room next) {
		next.Prepare(previous);
		OnRoomEntered.Invoke(next);
	}
	
}