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

	public IInput Service { get; private set; }

	private void Awake() {
		if(Service != null) { return; }
		Service = new WindowsInput();
	}

}