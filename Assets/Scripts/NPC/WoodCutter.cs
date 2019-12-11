using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WoodCutter : InteractableObject {
	
	[SerializeField] private GameObject fireRequestIcon;
	[SerializeField] private Fire fire;
	[SerializeField] private PlayerDetector playerCloseDetector;
	[Space]
	[AssetDropdown("Items", typeof(Item))] [SerializeField] private Item rewardItem;

	private Animator animator;

	public override void Interact() {
		Debug.Log("Interact me");
		if (fire.IsLit && rewardItem != null) {
			CanInteract = false;
			Inventory.Instance.AddItem(rewardItem);
			rewardItem = null;
		}
		else if(!fire.IsLit) {
			fireRequestIcon.SetActive(true);
		}
	}

	protected override void Awake() {
		base.Awake();
		animator = GetComponent<Animator>();
		playerCloseDetector.OnPlayerEnter += OnPlayerCameClose;
		playerCloseDetector.OnPlayerExit += OnPlayerNoLongerClose;
		fireRequestIcon.SetActive(false);
	}

	private void Update() {
		if (!playerCloseDetector.PlayerIsTouching) { return; }
		bool playerIsStandingRight = playerCloseDetector.PlayerPosition.x > transform.position.x;
		animator.SetBool("playerOnRightHand", playerIsStandingRight);
	}

	private void OnPlayerCameClose() {
		animator.SetBool("lookAtPlayer", true);
	}

	private void OnPlayerNoLongerClose() {
		animator.SetBool("lookAtPlayer", false);
		animator.SetBool("playerOnRightHand", false);
		fireRequestIcon.SetActive(false);
	}

}