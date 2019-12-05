﻿using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public static Inventory Instance { get; private set; }

	public bool IsShowing { get { return inventoryPanel.IsActive; } }

	[SerializeField] private InventoryPanel inventoryPanel;

	private List<Item> items;

	public bool ContainsItem(Item item) {
		return items.Contains(item);
	}

	public void AddItem(Item item) {
		items.Add(item);
		inventoryPanel.AddItemWidget(item);
	}

	public void RemoveItem(Item item) {
		items.Remove(item);
		inventoryPanel.RemoveItemWidget(item);
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