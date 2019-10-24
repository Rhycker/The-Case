using System;
using UnityEngine;

[RequireComponent(typeof(VIDE_Assign))]
public class DoorBlockingCharacter : MonoBehaviour {

	public enum GrantPassageManner {
		SpecificStartNodeRequired,
		AlwaysAllowAfterDialogue
	}

	public bool BlockDoor {
		get {
			if(grantPassageManner == GrantPassageManner.SpecificStartNodeRequired) {
				return dialogue.overrideStartNode != requiredStartNode;
			}
			else {
				return !playerHasInteracted;
			}
		}
	}

	[SerializeField] private int doorBlockOverrideStartNode;
	[SerializeField] private GrantPassageManner grantPassageManner;
	[SerializeField] private int requiredStartNode;

	private VIDE_Assign dialogue;

	private bool playerHasInteracted;
	private Action onAccessGrantedCallback;

	private void Awake() {
		dialogue = GetComponent<VIDE_Assign>();
	}

	public void ShowBlockDoorDialogue(Action onAccessGranted) {
		playerHasInteracted = true;
		dialogue.overrideStartNode = doorBlockOverrideStartNode;
		onAccessGrantedCallback = onAccessGranted;
		DialogueUI.Instance.StartDialogue(dialogue, OnDialogueFinished);
	}

	private void OnDialogueFinished() {
		if (!BlockDoor) {
			onAccessGrantedCallback?.Invoke();
		}
	}

}