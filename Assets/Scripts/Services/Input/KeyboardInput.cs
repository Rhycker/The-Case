using UnityEngine;

public class KeyboardInput : IInput {
	
	public float Horizontal() {
		if (Input.GetKey(KeyCode.A)) {
			return -1f;
		} else if (Input.GetKey(KeyCode.D)) {
			return 1f;
		} else {
			return 0f;
		}
	}

	public bool InventoryButtonUp() {
		return Input.GetKeyUp(KeyCode.UpArrow);
	}

	public bool BackButtonUp() {
		return Input.GetKeyUp(KeyCode.RightArrow);
	}

	public bool InteractButtonUp() {
		return Input.GetKeyUp(KeyCode.RightArrow);
	}

}