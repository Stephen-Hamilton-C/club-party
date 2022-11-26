using Network;
using Photon.Pun;
using UnityEngine;

namespace Ball {
    [RequireComponent(typeof(NetworkedAudio))]
    public class PlayerSounds : MonoBehaviour {

        [SerializeField] private bool debug;
        
        [SerializeField] private AudioClip[] hitSounds;
        [SerializeField] private AudioClip[] bounceSounds;
        [SerializeField] private float volumeScale = 20f;
        [SerializeField] private float maxVolume = 5f;

        private Logger _logger;
        private NetworkedAudio _netAudio;
        private PhotonView _view;

        private void Awake() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            if (!_view.IsMine)
                Destroy(this);
            
            _netAudio = GetComponent<NetworkedAudio>();
        }

        private void OnEnable() {
            _logger.Log("Subscribing to OnStroke...");
            LocalPlayerState.OnStroke += PlayStrokeSound;
        }
        
        private void OnDisable() {
            _logger.Log("Dropping OnStroke...");
            LocalPlayerState.OnStroke -= PlayStrokeSound;
        }
        
        private void PlayStrokeSound() {
            var clip = PickRandomSound(hitSounds);
            
            _logger.Log("Playing stroke sound: "+clip.name);
            _netAudio.Pitch = Random.Range(0.9f, 1.1f);
            _netAudio.PlayOneShot(clip, 1);
        }

        private void OnCollisionEnter(Collision collision) {
            var volume = collision.relativeVelocity.sqrMagnitude / volumeScale;
            var clampedVolume = Mathf.Clamp(volume, 0, maxVolume);
            var clip = PickRandomSound(bounceSounds);
            
            _logger.Log("Playing bounce sound: "+clip.name+" with "+clampedVolume+" volume");
            _netAudio.Pitch = 1;
            _netAudio.PlayOneShotLocal(clip, clampedVolume);
        }

        private AudioClip PickRandomSound(AudioClip[] clips) {
            return clips[Random.Range(0, clips.Length)];
        }

    }
}
