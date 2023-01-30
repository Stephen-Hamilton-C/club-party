using Photon.Pun;
using UnityEngine;
using Logger = SHamilton.Util.Logger;

namespace SHamilton.ClubParty.Network {
    /// <summary>
    /// Networks an AudioSource while leaving external methods simple and familiar
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(PhotonView))]
    public class NetworkedAudio : MonoBehaviour, IPunObservable {

        [SerializeField] private bool debug;
    
        public float Volume {
            get => _source.volume;
            set => _source.volume = value;
        }

        public float Pitch {
            get => _source.pitch;
            set => _source.pitch = value;
        }

        public AudioClip Clip {
            get => _source.clip;
            set => _source.clip = value;
        }

        private Logger _logger;
        private AudioSource _source;
        private PhotonView _view;

        private void Awake() {
            _logger = new(this, debug);
            _source = GetComponent<AudioSource>();
            _view = GetComponent<PhotonView>();
        }
    
        /// <summary>
        /// Sync the volume, pitch, and current clip
        /// </summary>
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsReading) {
                Volume = (float) stream.ReceiveNext();
                Pitch = (float)stream.ReceiveNext();
                Clip = (AudioClip) stream.ReceiveNext();
            }
            else {
                stream.SendNext(Volume);
                stream.SendNext(Pitch);
                stream.SendNext(Clip);
            }
        }

        /// <summary>
        /// Plays the AudioSource across all clients
        /// </summary>
        public void Play() {
            _logger.Log("Client called Play");
            _view.RPC("PlayRPC", RpcTarget.All);
        }

        /// <summary>
        /// Plays the AudioSource on just this client
        /// </summary>
        public void PlayLocal() {
            _logger.Log("Non-networked Play");
            PlayRPC();
        }

        /// <summary>
        /// Plays the given AudioClip across all clients
        /// </summary>
        /// <param name="clip">The clip to play. Must be in Resources/Sounds</param>
        public void PlayOneShot(AudioClip clip) {
            _logger.Log("Client called PlayOneShot with "+clip);
            _view.RPC("PlayOneShotRPC", RpcTarget.All, clip);
        }
    
        /// <summary>
        /// Plays the given AudioClip on just this client
        /// </summary>
        /// <param name="clip">The clip to play</param>
        public void PlayOneShotLocal(AudioClip clip) {
            _logger.Log("Non-networked PlayOneShot with "+clip);
            PlayOneShotRPC(clip);
        }

        /// <summary>
        /// Plays the given AudioClip across all clients at the given volume
        /// </summary>
        /// <param name="clip">The clip to play. Must be in Resources/Sounds</param>
        /// <param name="volumeScale">The volume to play the clip at</param>
        public void PlayOneShot(AudioClip clip, float volumeScale) {
            _logger.Log("Client called PlayOneShot with "+clip+" and "+volumeScale);
            _view.RPC("PlayOneShotRPC", RpcTarget.All, clip, volumeScale);
        }

        /// <summary>
        /// Plays the given AudioClip on just this client at the given volume
        /// </summary>
        /// <param name="clip">The clip to play</param>
        /// <param name="volumeScale">The volume to play the clip at</param>
        public void PlayOneShotLocal(AudioClip clip, float volumeScale) {
            _logger.Log("Non-networked PlayOneShot with "+clip+" and "+volumeScale);
            PlayOneShotRPC(clip, volumeScale);
        }
    
        /// <summary>
        /// Stops the AudioSource across all clients
        /// </summary>
        public void Stop() {
            _logger.Log("Client called Stop");
            _view.RPC("StopRPC", RpcTarget.All);
        }

        /// <summary>
        /// Stops the AudioSource on just this client
        /// </summary>
        public void StopLocal() {
            _logger.Log("Non-networked Stop");
            StopRPC();
        }

        [PunRPC]
        private void PlayRPC() {
            _logger.Log("RPC received for Play");
            _source.Play();
        }

        [PunRPC]
        private void PlayOneShotRPC(AudioClip clip) {
            _logger.Log("RPC received for PlayOneShot with "+clip);
            _source.PlayOneShot(clip);
        }

        [PunRPC]
        private void PlayOneShotRPC(AudioClip clip, float volumeScale) {
            _logger.Log("RPC received for PlayOneShot with "+clip+" and "+volumeScale);
            _source.PlayOneShot(clip, volumeScale);
        }

        [PunRPC]
        private void StopRPC() {
            _logger.Log("RPC received for Stop");
            _source.Stop();
        }

    }
}
