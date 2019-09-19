using UnityEngine;

public class GameInput : MonoBehaviour {

	private static GameInput instance;
	public static GameInput Instance {
		get {
			if (instance == null) {
				instance = new GameObject("GameInput").AddComponent<GameInput>();
			}

			return instance;
		}
	}

	public IInput InputService { get; private set; }

	private void Awake() {
		if(InputService != null) { return; }
		InputService = new KeyboardInput();
	}

}