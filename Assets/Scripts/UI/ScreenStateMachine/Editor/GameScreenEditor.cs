using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameScreen), true)]
public class GameScreenEditor : Editor {

	private void Awake() {
		if (Application.isPlaying) { return; }

		GameScreen gameScreen = (GameScreen)target;
		GameScreen[] allScreens = gameScreen.transform.root.GetComponentsInChildren<GameScreen>(true);
		foreach (GameScreen screen in allScreens) {
			if (screen == gameScreen) {
				screen.gameObject.SetActive(true);
			}
			else {
				screen.gameObject.SetActive(false);
			}
		}
	}

}