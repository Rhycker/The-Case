using System.Collections.Generic;
using UnityEngine;

public class ScreenStateMachine : MonoBehaviour {

	public bool AnyScreenIsShowing { get { return activeScreenState != null; } }

	private List<GameScreen.GameScreenState> states;
	private static GameScreen.GameScreenState activeScreenState;

	public void RegisterScreenState(GameScreen.GameScreenState state) {
		if (states == null) {
			states = new List<GameScreen.GameScreenState>();
		}

		states.Add(state);
	}

	public void ActivateScreen(GameScreen.GameScreenState nextState) {
		if (activeScreenState == nextState) { return; }

		foreach (GameScreen.GameScreenState state in states) {
			if (state == nextState) {
				state.Show();
			}
			else if (state.IsShowing) {
				state.Hide();
			}
		}

		activeScreenState = nextState;
	}

	public void DeactivateScreen(GameScreen.GameScreenState state) {
		if (activeScreenState != state) { return; }
		state.Hide();
		activeScreenState = null;
	}

	public static bool PointerClickIsBlockedByActiveScreen(Vector2 position) {
		if (activeScreenState == null) { return false; }
		return activeScreenState.IsBlockingInteraction(position);
	}

	private void Awake() {
		states = new List<GameScreen.GameScreenState>();
		activeScreenState = null;

		GameScreen[] gameScreens = GetComponentsInChildren<GameScreen>(true);
		foreach(GameScreen screen in gameScreens) {
			states.Add(screen.Initialize());
		}
	}

}