using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Notification : MonoBehaviour {
    [SerializeField] private AudioClip interactionSound;
    public static Notification Instance { get; private set; }

	private new SpriteRenderer renderer;
	private Animator animator;

	public void Show(Sprite notificationSprite) {
		renderer.sprite = notificationSprite;
        animator.SetTrigger("show");
        //Falling sound
        SoundManager.Instance.PlaySound(interactionSound);
    }

	private void Awake() {
		Instance = this;
		renderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		renderer.enabled = true;
	}
	
}