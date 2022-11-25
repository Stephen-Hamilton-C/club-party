using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ball {
    [RequireComponent(typeof(NetworkedAudio))]
    public class PlayerSounds : MonoBehaviour {

        [SerializeField] private bool debug;
        
        [SerializeField] private AudioClip[] hitSounds;
        [SerializeField] private AudioClip[] bounceSounds;

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
            PlayerState.OnStroke += PlayStrokeSound;
        }
        
        private void OnDisable() {
            _logger.Log("Dropping OnStroke...");
            PlayerState.OnStroke -= PlayStrokeSound;
        }
        
        private void PlayStrokeSound() {
            var clip = PickRandomSound(hitSounds);
            _logger.Log("Playing stroke sound: "+clip.name);
            _netAudio.PlayOneShot(clip);
        }

        private void OnCollisionEnter(Collision collision) {
            // TODO: Scale volume based on collision impulse
            
            var clip = PickRandomSound(bounceSounds);
            _logger.Log("Playing bounce sound: "+clip.name);
            _netAudio.PlayOneShot(clip);
        }

        private AudioClip PickRandomSound(AudioClip[] clips) {
            return clips[Random.Range(0, clips.Length)];
        }

    }
}
