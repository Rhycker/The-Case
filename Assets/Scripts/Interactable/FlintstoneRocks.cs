using UnityEngine;

public class FlintstoneRocks : InteractableObject {

	[SerializeField] private Item pickaxe;
	[SerializeField] private Item flintstones;

	public override bool UseItem(Item item) {
		if(item == pickaxe) {
			Inventory.Instance.AddItem(flintstones);
			return true;
		}
		return false;
	}

}