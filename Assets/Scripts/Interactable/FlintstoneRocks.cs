using UnityEngine;

public class FlintstoneRocks : InteractableObject {

	[SerializeField] private Item pickaxe;
	[SerializeField] private Item flintstones;

	public override void UseItem(Item item) {
		if(item == pickaxe) {
			Inventory.Instance.AddItem(flintstones);
		}
	}

}