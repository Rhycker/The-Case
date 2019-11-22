using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public static Inventory Instance { get; private set; }

	public bool IsShowing { get { return inventoryPanel.IsActive; } }
	public int ItemCount { get { return items.Count; } }

	[SerializeField] private InventoryPanel inventoryPanel;

	private List<Item> items;// This should be bound to the inventory panel items

	public bool AddItem(Item item) {
		if(items.Count >= inventoryPanel.MaxItemCount) { return false; }
		items.Add(item);
		inventoryPanel.AddItemWidget(item);
		return true;
	}

	public void CombineItems(Item itemA, Item itemB, Item combinedItem) {
		items.Remove(itemA);
		items.Remove(itemB);
		AddItem(combinedItem);
	}

	private void Awake() {
		Instance = this;
		items = new List<Item>();
	}

	private void Update() {
		if (GameInput.Instance.Service.InventoryButtonDown()) {
			inventoryPanel.Toggle();
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			Item[] allItems = Resources.LoadAll<Item>("Items");
			Item item = allItems[Random.Range(0, allItems.Length)];
			Debug.Log("Add random item: " + item.name);
			AddItem(item);
		}
	}

}