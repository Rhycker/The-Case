using UnityEngine;

public class BrokenLadder : InteractableObject {

	[SerializeField] private GameObject brokenLadderVisuals;
	[SerializeField] private GameObject repairedLadder;
	[AssetDropdown("Items", typeof(Item))] [SerializeField] private Item requiredRepairItem;

	public override bool UseItem(Item item) {
		if(item == requiredRepairItem) {
			brokenLadderVisuals.SetActive(false);
			repairedLadder.SetActive(true);
			Inventory.Instance.RemoveItem(item);
			CanInteract = false;
			return true;
		}

		return false;
	}

}