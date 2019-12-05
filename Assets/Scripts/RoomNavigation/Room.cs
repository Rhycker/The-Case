using UnityEngine;

public class Room : MonoBehaviour {
	
	public Vector3 PlayerSpawnPosition { get; private set; }
	public Color BackgroundColor { get { return backgroundColor; } }

	[SerializeField] private Color backgroundColor;

	private Door[] doors;

	private void Awake() {
		doors = GetComponentsInChildren<Door>();
	}

	public void Prepare(Room previous) {
		foreach(Door door in doors) {
			if(door.TargetRoom == previous) {
				PlayerSpawnPosition = door.transform.position;
				return;
			}
		}

		Debug.LogWarning("Room(" + transform.name + ") does not have a door that enters the previous(" + previous.name + ") room", transform);
	}

}