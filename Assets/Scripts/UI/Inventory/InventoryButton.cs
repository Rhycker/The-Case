using UnityEngine;

public class InventoryButton : MonoBehaviour {

	public void Button_OpenInventory() {
		InventoryPanel.Instance.Activate(true);
	}
	
}