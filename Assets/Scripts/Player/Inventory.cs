using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public static Inventory Instance { get; private set; }

	[SerializeField] private InventoryPanel inventoryPanel;

	private List<Item> items;

	public void AddItem(Item item) {
		items.Add(item);
		inventoryPanel.AddItemWidget(item);
	}

	public bool CombineItems(Item itemA, Item itemB) {
		Item combinedItem = itemA.Combine(itemB);
		if (combinedItem == null) { return false; }

		items.Remove(itemA);
		items.Remove(itemB);
		AddItem(combinedItem);
		return true;
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