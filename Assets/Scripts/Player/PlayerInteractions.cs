using UnityEngine;

public class PlayerInteractions : MonoBehaviour {

	private InteractableObject interactableObject;

	public void UseItem(Item item) {
		if(interactableObject == null) { return; }
		interactableObject.UseItem(item);
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
		interactableObject = collider.GetComponent<InteractableObject>();
		if(interactableObject == null) {
			Debug.LogWarning("No interactable object on " + collider.name);
			return;
		}

		interactableObject.ShowInteractIcon(true);
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag != "Interactable") { return; }

		InteractableObject interactable = collider.GetComponent<InteractableObject>();
		if (interactableObject != interactable) { return; }

		interactableObject.ShowInteractIcon(false);
		interactableObject = null;
	}

}