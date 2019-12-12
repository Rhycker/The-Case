using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Fire : InteractableObject {

    [SerializeField] private AudioClip interactionSound;
    public bool IsLit { get; private set; }

	[AssetDropdown("Items", typeof(Item))] [SerializeField] private Item requiredItem;
	private Animator animator;

	public override bool UseItem(Item item) {
		if(item != requiredItem) { return false; }

        // Ignite fire
        Inventory.Instance.RemoveItem(item);
		IsLit = true;
		animator.enabled = true;
		CanInteract = false;
		ShowInteractIcon(false);

        // Ignite fire sound
        SoundManager.Instance.PlaySound(interactionSound);
		// Play looping fire audio:
		GetComponent<AudioSource>().Play();

		return true;
	}

	protected override void Awake() {
		base.Awake();
		animator = GetComponent<Animator>();
		animator.enabled = false;
	}

}