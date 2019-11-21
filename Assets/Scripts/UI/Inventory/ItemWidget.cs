using UnityEngine;
using UnityEngine.UI;

public class ItemWidget : MonoBehaviour {

	public Item Item { get; private set; }

	[SerializeField] private Image iconImage;

	public void Initialize() {
		iconImage.sprite = null;
		iconImage.enabled = false;
	}

	public void BindItem(Item item) {
		Item = item;
		iconImage.sprite = item.Icon;
		iconImage.enabled = true;
	}

	public void Clear() {
		Item = null;
		iconImage.sprite = null;
		iconImage.enabled = false;
	}
	
}