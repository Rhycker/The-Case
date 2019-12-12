using UnityEngine;
using System.Collections.Generic;

public class Teleport : MonoBehaviour {

	[SerializeField] private Transform teleportTarget;

	private List<Transform> teleportLocations;
	private int teleportLocationIndex;

	private void Awake() {
		teleportLocations = new List<Transform>(GetComponentsInChildren<Transform>());
		teleportLocations.Remove(transform);
	}

	private void Update() {
		if (Input.GetKeyUp(KeyCode.LeftBracket)) {
			teleportLocationIndex--;
		}
		else if (Input.GetKeyUp(KeyCode.RightBracket)) {
			teleportLocationIndex++;
		}
		else {
			return;
		}

		if (teleportLocationIndex >= teleportLocations.Count) {
			teleportLocationIndex = 0;
		}
		else if (teleportLocationIndex < 0) {
			teleportLocationIndex = teleportLocations.Count - 1;
		}
		teleportTarget.position = teleportLocations[teleportLocationIndex].position;
	}

}