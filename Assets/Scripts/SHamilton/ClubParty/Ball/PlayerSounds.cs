using Photon.Pun;
using SHamilton.ClubParty.Network;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Ball {
    /// <summary>
    /// Handles all sounds that the player character creates
    /// </summary>
    [RequireComponent(typeof(NetworkedAudio))]
    public class PlayerSounds : MonoBehaviour {

        [SerializeField] private bool debug;
        
        [Tooltip("All the hit sounds to pick from when the player ")]
        [SerializeField] private AudioClip[] hitSounds;
        [Tooltip("All the bounce sounds to pick from when the player ")]
        [SerializeField] private AudioClip[] bounceSounds;
        [Tooltip("The inverse scale factor for bouncing sounds. Scaled by impact force.")]
        [SerializeField] private float volumeScale = 10f;
        [Tooltip("The maximum volume allowed for any sound")]
        [SerializeField] private float maxVolume = 10f;

        private Logger _logger;
        private NetworkedAudio _netAudio;
        private PhotonView _view;

        private void Awake() {
            _logger = new(this, debug);
            _view = GetComponent<PhotonView>();
            _netAudio = GetComponent<NetworkedAudio>();
        }

        private void OnEnable() {
            // Only subscribe on local character
            if (!_view.IsMine) return;
            _logger.Log("Subscribing to OnStroke...");
            LocalPlayerState.OnStroke += PlayStrokeSound;
        }
        
        private void OnDisable() {
            // Only drop on local character
            if (!_view.IsMine) return;
            _logger.Log("Dropping OnStroke...");
            LocalPlayerState.OnStroke -= PlayStrokeSound;
        }
        
        private void PlayStrokeSound() {
            // Play stroke sound to all players
            var clip = PickRandomSound(hitSounds);
            
            _logger.Log("Playing stroke sound: "+clip.name);
            _netAudio.Pitch = Random.Range(0.9f, 1.1f);
            _netAudio.PlayOneShot(clip, 1);
        }

        private void OnCollisionEnter(Collision collision) {
            // Calculate collision sound volume
            var volume = collision.relativeVelocity.sqrMagnitude / volumeScale;
            var clampedVolume = Mathf.Clamp(volume, 0, maxVolume);
            var clip = PickRandomSound(bounceSounds);
            
            // Play collision sound only on client
            _logger.Log("Playing bounce sound: "+clip.name+" with "+clampedVolume+" volume");
            _netAudio.Pitch = 1;
            _netAudio.PlayOneShotLocal(clip, clampedVolume);
        }

        private AudioClip PickRandomSound(AudioClip[] clips) {
            return clips[Random.Range(0, clips.Length)];
        }

    }
}
