using UnityEngine;

public class PickableItem : InteractableObject {

	[AssetDropdown("Items", typeof(Item))][SerializeField] private Item item;

	public override void Interact() {
		InventoryPanel.Instance.AddItemWidget(item);
		Destroy(gameObject);
	}
	
}