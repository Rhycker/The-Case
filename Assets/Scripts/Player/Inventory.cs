using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public static Inventory Instance { get; private set; }

	public bool IsShowing { get { return inventoryPanel.IsActive; } }

	[SerializeField] private InventoryPanel inventoryPanel;

	public void AddItem(Item item) {
		inventoryPanel.AddItemWidget(item);
	}

	private void Awake() {
		Instance = this;
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