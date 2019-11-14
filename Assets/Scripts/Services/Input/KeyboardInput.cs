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

	public float Vertical() {
		if (Input.GetKey(KeyCode.S)) {
			return -1f;
		}
		else if (Input.GetKey(KeyCode.W)) {
			return 1f;
		}
		else {
			return 0f;
		}
	}

	public bool InventoryButtonDown() {
		return Input.GetKeyDown(KeyCode.UpArrow);
	}

	public bool BackButtonDown() {
		return Input.GetKeyDown(KeyCode.RightArrow);
	}

	public bool InteractButtonDown() {
		return Input.GetKeyDown(KeyCode.DownArrow);
	}

	public bool PreviousChoiceButtonDown() {
		return Input.GetKeyDown(KeyCode.W);
	}

	public bool NextChoiceButtonDown() {
		return Input.GetKeyDown(KeyCode.S);

	}

}