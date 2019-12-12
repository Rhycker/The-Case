using UnityEngine;

public class OldMan : InteractableObject {

	[AssetDropdown("Items", typeof(Item))] [SerializeField] private Item rewardItem;

	public override void Interact() {
		Inventory.Instance.AddItem(rewardItem);
		CanInteract = false;
		ShowInteractIcon(false);
	}

}