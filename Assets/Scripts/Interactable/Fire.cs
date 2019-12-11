using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Fire : InteractableObject {

    [SerializeField] private AudioClip interactionSound;
    public bool IsLit { get; private set; }

	[AssetDropdown("Items", typeof(Item))] [SerializeField] private Item requiredItem;
	private Animator animator;

	public override void UseItem(Item item) {
		if(item == requiredItem) {
            // Ignite fire sound
            SoundManager.Instance.PlaySound(interactionSound);
            // Ignite fire
            Inventory.Instance.RemoveItem(item);
			IsLit = true;
			animator.enabled = true;
			CanInteract = false;
			ShowInteractIcon(false);

			// Play audio:
			GetComponent<AudioSource>().Play();
		}
	}

	protected override void Awake() {
		base.Awake();
		animator = GetComponent<Animator>();
		animator.enabled = false;
	}

}