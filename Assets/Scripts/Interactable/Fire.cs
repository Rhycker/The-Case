using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Fire : InteractableObject {

	public bool IsLit { get; private set; }

	[AssetDropdown("Items", typeof(Item))] [SerializeField] private Item requiredItem;
	private Animator animator;

	public override void UseItem(Item item) {
		if(item == requiredItem) {
			Inventory.Instance.RemoveItem(item);
			IsLit = true;
			animator.enabled = true;
			ShowInteractIcon(false);
			Destroy(this);
		}
	}

	protected override void Awake() {
		base.Awake();
		animator = GetComponent<Animator>();
		animator.enabled = false;
	}

}