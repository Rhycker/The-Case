using UnityEngine;

public class Room : MonoBehaviour {

	public Vector3 OriginPosition {
		get {
			Vector3 spawnPosition = transform.position;
			spawnPosition.y = floorHeightOrigin.position.y;
			return spawnPosition;
		}
	}
	public float PlayerSpawnPositionX { get; private set; }

	[SerializeField] private Transform floorHeightOrigin;

	private Door[] doors;

	private void Awake() {
		doors = GetComponentsInChildren<Door>();
	}

	public void Prepare(Room previous) {
		foreach(Door door in doors) {
			if(door.TargetRoom == previous) {
				PlayerSpawnPositionX = door.transform.position.x;
				return;
			}
		}

		Debug.LogWarning("Room(" + transform.name + ") does not have a door that enters the previous(" + previous.name + ") room", transform);
	}

}