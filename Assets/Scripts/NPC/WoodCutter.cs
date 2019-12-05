using UnityEngine;

public class WoodCutter : InteractableObject {

	[SerializeField] private Fire fire;
	[AssetDropdown("Items", typeof(Item))] [SerializeField] private Item rewardItem;

	public override void Interact() {
		if(fire.IsLit && rewardItem != null) {
			Inventory.Instance.AddItem(rewardItem);
			rewardItem = null;
		}
	}

	protected override void Awake() {
		base.Awake();
    }

}