using UnityEngine;

public abstract class State : MonoBehaviour {

	protected GameStateMachine stateMachine { get; private set; }

	public void Initialize(GameStateMachine stateMachine) {
		this.stateMachine = stateMachine;
	}

	public abstract void Enter();
	public virtual void Exit() { }

}