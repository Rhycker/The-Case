﻿using UnityEngine;

public class CharacterInteractions : MonoBehaviour {

	private InteractableObject interactableObject;

	private void Update() {
		if(interactableObject == null) { return; }
		if (!GameInput.Instance.Service.InteractButtonDown()) { return; }
		interactableObject.Interact();
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if(collider.tag != "Interactable") { return; }

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