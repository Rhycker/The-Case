using UnityEngine;

public class WoodCutter : InteractableObject {

	[SerializeField] private GameObject talkIcon;
	[SerializeField] private Fire fire;
	[AssetDropdown("Items", typeof(Item))] [SerializeField] private Item rewardItem;

	public override void Interact() {
		talkIcon.SetActive(false);
		if (fire.IsLit && rewardItem != null) {
			Inventory.Instance.AddItem(rewardItem);
			rewardItem = null;
		}
	}

	protected override void Awake() {
		base.Awake();
    }

}