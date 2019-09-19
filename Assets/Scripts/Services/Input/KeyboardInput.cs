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

}