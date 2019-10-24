using UnityEngine;
using UnityEngine.UI;

public class ItemWidget : MonoBehaviour {

	public Item Item { get; private set; }

	[SerializeField] private Image iconImage;

	private InventoryPanel inventoryPanel;

	public void Initialize(InventoryPanel inventoryPanel, Item item) {
		this.inventoryPanel = inventoryPanel;
		Item = item;
		iconImage.sprite = item.Icon;
	}

	public void Button_Item() {
		inventoryPanel.InteractItemWidget(this);
	}

}