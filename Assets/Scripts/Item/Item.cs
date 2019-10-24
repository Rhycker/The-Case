using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject {

	public enum ItemUseType {
		Use,
		Wear
	}

	public ItemUseType ItemUse { get { return itemUse; } }
	public Sprite Icon { get { return icon; } }
	public string ExaminationText { get { return examinationText; } }

	[SerializeField] private Sprite icon;
	[SerializeField] private ItemUseType itemUse;
	[SerializeField] private string examinationText;
	[SerializeField] private Item firstIngredient;
	[SerializeField] private Item secondIngredient;

	private static Item[] allItems;

	public Item CombineWithOther(Item otherItem) {
		if(allItems == null) {
			allItems = Resources.LoadAll<Item>("Items");
		}

		foreach(Item item in allItems) {
			if(item == this) { continue; }
			if(item == otherItem) { continue; }
			if(item.firstIngredient == null) { continue; }
			if(item.secondIngredient == null) { continue; }

			if (firstIngredient == this && secondIngredient == otherItem) {
				return item;
			}
			if(firstIngredient == otherItem && secondIngredient == this) {
				return item;
			}
		}

		return null;
	}
	
}