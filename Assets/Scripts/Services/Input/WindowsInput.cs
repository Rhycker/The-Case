using UnityEngine;

public class WindowsInput : IInput {
	
	public float Horizontal() {
		float joystickInput = Input.GetAxis("JoystickHorizontal");
		if(joystickInput != 0) {
			return joystickInput;
		}

		if (Input.GetKey(KeyCode.A)) {
			return -1f;
		} else if (Input.GetKey(KeyCode.D)) {
			return 1f;
		} else {
			return 0f;
		}
	}

	public float Vertical() {
		float joystickInput = Input.GetAxis("JoystickVertical");
		if (joystickInput != 0) {
			return joystickInput;
		}

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

	public float InventoryHorizontal() {
		float joystickInput = Input.GetAxis("JoystickInventoryHorizontal");
		return joystickInput == 0 ? Horizontal() : joystickInput;
	}

	public float InventoryVertical() {
		float joystickInput = Input.GetAxis("JoystickInventoryVertical");
		return joystickInput == 0 ? Horizontal() : joystickInput;
	}

	public bool InventoryButtonDown() {
		bool joystickButtonDown = Input.GetKeyDown("joystick button 3");
		return joystickButtonDown || Input.GetKeyDown(KeyCode.UpArrow);
	}

	public bool BackButtonDown() {
		bool joystickButtonDown = Input.GetKeyDown("joystick button 1");
		return joystickButtonDown || Input.GetKeyDown(KeyCode.RightArrow);
	}

	public bool InteractButtonDown() {
		bool joystickButtonDown = Input.GetKeyDown("joystick button 0");
		return joystickButtonDown || Input.GetKeyDown(KeyCode.DownArrow);
	}

	public bool PreviousChoiceButtonDown() {
		return Input.GetKeyDown(KeyCode.W);
	}

	public bool NextChoiceButtonDown() {
		return Input.GetKeyDown(KeyCode.S);

	}

}