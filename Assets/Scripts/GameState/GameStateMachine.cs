using UnityEngine;

public class GameStateMachine : MonoBehaviour {

	[Header("References")]
	[SerializeField] private State currentState;

	[Header("Settings")]
	[SerializeField] private bool debugLogStateTransitions;

	private State[] gameStates;

	public void EnterState(State nextState) {
		if (nextState == null) {
			Debug.LogWarning("[GameStateMachine.EnterState] Cannot enter a state that is 'null'", transform);
			return;
		}

		if (currentState != null) {
			currentState.Exit();
		}

		currentState = nextState;
		currentState.Enter();

		if (debugLogStateTransitions) {
			Debug.Log("Entering next state: " + nextState.GetType().Name);
		}
	}

	private void Awake() {
		gameStates = GetComponents<State>();
		foreach(State state in gameStates) {
			state.Initialize(this);
		}

		if(currentState == null) {
			currentState = gameStates[0];
		}
	}

	private void Start() {
		currentState.Enter();
	}

}