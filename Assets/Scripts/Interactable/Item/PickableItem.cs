using UnityEngine;

public class PickableItem : InteractableObject {

	[AssetDropdown("Items", typeof(Item))][SerializeField] private Item item;

	public override void Interact() {
		Inventory.Instance.AddItem(item);
		Destroy(gameObject);
	}
	
}