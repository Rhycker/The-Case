using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
	
	[Serializable]
	public class Bounds {
		public bool YMaxEnabled = false;
		public float YMaxValue;
		public bool YMinEnabled = false;
		public float YMinValue;
		public bool XMaxEnabled = false;
		public float XMaxValue;
		public bool XMinEnabled = false;
		public float XMinValue;
	}
	
	[SerializeField] private Transform target;
	[SerializeField] private float smoothTime;
	[Space]
	[SerializeField] private Room startRoom;
	[Space]
	[SerializeField] private SpriteRenderer itemNotificationRenderer;
	[SerializeField] private Animator itemNotificationAnimator;
	
	private Vector3 velocity = Vector3.zero;
	private new Camera camera;
	private Bounds currentBounds;

	private void Awake() {
		transform.SetParent(null);
		RoomNavigation.Instance.OnRoomEntered += OnRoomEntered;
		Inventory.Instance.OnItemObtained += OnItemObtained;
		itemNotificationRenderer.gameObject.SetActive(true);

		camera = GetComponent<Camera>();
		currentBounds = startRoom.CameraBounds;
	}

	private void OnItemObtained(Item item) {
		itemNotificationRenderer.sprite = item.NotificationSprite;
		itemNotificationAnimator.SetTrigger("show");
	}

	private void LateUpdate() {
		//target position
		Vector3 targetPos = target.position;

		//vertical 
		if (currentBounds.YMinEnabled && currentBounds.YMaxEnabled)
			targetPos.y = Mathf.Clamp(target.position.y, currentBounds.YMinValue, currentBounds.YMaxValue);
		else if (currentBounds.YMinEnabled)
			targetPos.y = Mathf.Clamp(target.position.y, currentBounds.YMinValue, target.position.y);
		else if (currentBounds.YMaxEnabled)
			targetPos.y = Mathf.Clamp(target.position.y, target.position.y, currentBounds.YMaxValue);

		//horizontal
		if (currentBounds.XMinEnabled && currentBounds.XMaxEnabled)
			targetPos.x = Mathf.Clamp(target.position.x, currentBounds.XMinValue, currentBounds.XMaxValue);
		else if (currentBounds.XMinEnabled)
			targetPos.x = Mathf.Clamp(target.position.x, currentBounds.XMinValue, target.position.x);
		else if (currentBounds.XMaxEnabled)
			targetPos.x = Mathf.Clamp(target.position.x, target.position.x, currentBounds.XMaxValue);

		//align the camera and target z position
		targetPos.z = transform.position.z;
		transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
	}

	private void OnRoomEntered(Room nextRoom) {
		camera.backgroundColor = nextRoom.BackgroundColor;
		currentBounds = nextRoom.CameraBounds;
	}

}