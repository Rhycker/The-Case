using UnityEngine;

public class PlayerInteractions : MonoBehaviour {

	private InteractableObject interactableObject;

	public bool UseItem(Item item) {
		if(interactableObject == null) { return false; }
		return interactableObject.UseItem(item);
	}

	private void Update() {
		if (interactableObject == null) { return; }
		if (DialogueUI.Instance.IsActive) { return; }
		if (!GameInput.Instance.Service.InteractButtonDown()) { return; }
		interactableObject.Interact();
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if(collider.tag != "Interactable") { return; }

		if(interactableObject != null) {
			interactableObject.ShowInteractIcon(false);
		}
		interactableObject = collider.GetComponentInParent<InteractableObject>();
		if (!interactableObject.CanInteract) { return; }

		interactableObject.ShowInteractIcon(true);
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag != "Interactable") { return; }
		if (interactableObject == null) { return; }

		InteractableObject interactable = collider.GetComponentInParent<InteractableObject>();
		if (interactableObject != interactable) { return; }

		interactableObject.ShowInteractIcon(false);
		interactableObject = null;
	}

}