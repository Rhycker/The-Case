using UnityEngine;

public class InteractableObject : MonoBehaviour {

	[SerializeField] private GameObject interactionIcon;

	private IInteractable interactableComponent;

	private void Awake() {
		interactableComponent = GetComponent<IInteractable>();
		if(interactableComponent == null) {
			Debug.LogWarning("No interactable component found on " + transform.name, transform);
		}

		interactionIcon.SetActive(false);
	}

	public void ShowInteractIcon(bool show) {
		interactionIcon.SetActive(show);
	}

	public void Interact() {
		interactableComponent.Interact();	
	}
	
}