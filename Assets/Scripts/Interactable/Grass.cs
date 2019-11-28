using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Animator))]
public class Grass : MonoBehaviour {

	[SerializeField] private float waveDuration = 1f;

	private float remainingWaveDuration;
	private Animator animator;

	private void Awake() {
		animator = GetComponent<Animator>();
		animator.SetFloat("offset", Random.value);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		remainingWaveDuration = waveDuration;
		animator.SetBool("wave", true);
	}

	private void OnTriggerExit2D(Collider2D other) {
		remainingWaveDuration = waveDuration;
		animator.SetBool("wave", true);
	}

	private void Update() {
		if(remainingWaveDuration <= 0f) { return; }
		remainingWaveDuration -= Time.deltaTime;
		if (remainingWaveDuration <= 0f) {
			animator.SetBool("wave", false);
		}
	}

}