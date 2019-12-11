using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	private static SoundManager audioManager;
	public static SoundManager Instance {
		get {
			if (audioManager == null) {
				SoundManager prefab = Resources.Load<SoundManager>("SoundManager");
				audioManager = Instantiate(prefab);
			}
			return audioManager;
		}
	}

	private AudioSource source;

	public void PlaySound(AudioClip audioClip, float volumeScale = 1f) {
		source.PlayOneShot(audioClip, volumeScale);
		Debug.Log("Play shot");
	}

	private void Awake() {
		source = GetComponent<AudioSource>();
    }
	
}