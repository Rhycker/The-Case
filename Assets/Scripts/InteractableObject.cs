using UnityEngine;

public abstract class InteractableObject : MonoBehaviour {

	[SerializeField] private GameObject interactionIcon;

	protected virtual void Awake() {
		interactionIcon.SetActive(false);
	}

	public void ShowInteractIcon(bool show) {
		interactionIcon.SetActive(show);
	}

	public virtual void Interact() { }
	public virtual void UseItem(Item item) { }

}