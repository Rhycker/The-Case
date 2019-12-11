using UnityEngine;

public abstract class InteractableObject : MonoBehaviour {

	public bool CanInteract { get; protected set; }

	[SerializeField] private GameObject interactionIcon;

	protected virtual void Awake() {
		if (interactionIcon != null) {
			interactionIcon.SetActive(false);
		}
		CanInteract = true;
	}

	public void ShowInteractIcon(bool show) {
		if (interactionIcon != null) {
			interactionIcon.SetActive(show);
		}
	}

	public virtual void Interact() { }
	public virtual void UseItem(Item item) { }

}