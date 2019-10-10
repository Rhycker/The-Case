using UnityEngine;

public class VIDEPlayer : MonoBehaviour {

	[SerializeField] private string displayName;

	private VIDE_Assign dialogueInTrigger;

	private void Awake() {
		DialogueUI.Instance.SetPlayerName(displayName);
	}

	private void Update() {
		if (!dialogueInTrigger) { return; }
		if (!GameInput.Instance.Service.InteractButtonDown()) { return; }
		DialogueUI.Instance.Interact(dialogueInTrigger);
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		VIDE_Assign interactable = other.GetComponent<VIDE_Assign>();
		if (interactable != null) {
			dialogueInTrigger = interactable;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		VIDE_Assign interactable = other.GetComponent<VIDE_Assign>();
		if(dialogueInTrigger == interactable) {
			dialogueInTrigger = null;
		}
	}

}