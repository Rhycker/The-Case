using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public static Inventory Instance { get; private set; }

	public bool IsShowing { get { return inventoryPanel.IsActive; } }
	public delegate void ItemDelegate(Item item);
	public event ItemDelegate OnItemObtained;

	[SerializeField] private InventoryPanel inventoryPanel;
	
	private List<Item> items;

	public bool ContainsItem(Item item) {
		return items.Contains(item);
	}

	public void AddItem(Item item) {
		items.Add(item);
		inventoryPanel.AddItemWidget(item);
		OnItemObtained?.Invoke(item);
	}

	public void RemoveItem(Item item) {
		items.Remove(item);
		inventoryPanel.RemoveItemWidget(item);
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
			AddItem(item);
		}
	}

}