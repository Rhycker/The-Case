using UnityEngine;

public class VIDEPlayer : MonoBehaviour {

	[SerializeField] private string displayName;

	private VIDE_Assign dialogueInTrigger;
	private bool dialogueStarted;

	private void Awake() {
		DialogueUI.Instance.SetPlayerName(displayName);
	}

	private void Update() {
		if (!dialogueInTrigger) { return; }
		if (dialogueStarted) { return; }

		if (GameInput.Instance.Service.InteractButtonDown()) {
			DialogueUI.Instance.StartDialogue(dialogueInTrigger);
			dialogueStarted = true;
		}
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		VIDE_Assign interactable = other.GetComponent<VIDE_Assign>();
		if (interactable != null) {
			dialogueInTrigger = interactable;
			dialogueStarted = false;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		VIDE_Assign interactable = other.GetComponent<VIDE_Assign>();
		if(dialogueInTrigger == interactable) {
			DialogueUI.Instance.StopDialogue(dialogueInTrigger);
			dialogueInTrigger = null;
		}
	}

}