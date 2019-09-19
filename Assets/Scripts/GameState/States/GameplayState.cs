using UnityEngine;

public class GameplayState : State {

	[Header("References")]
	[SerializeField] private Room startRoom;

	public override void Enter() {
		startRoom.Enter();
	}

	public override void Exit() {
	}

	private void Awake() {
	}

}