using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

namespace Network {
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

        public void Play() {
            _logger.Log("Client called Play");
            _view.RPC("PlayRPC", RpcTarget.All);
        }

        public void PlayLocal() {
            _logger.Log("Non-networked Play");
            PlayRPC();
        }

        public void PlayOneShot(AudioClip clip) {
            _logger.Log("Client called PlayOneShot with "+clip);
            _view.RPC("PlayOneShotRPC", RpcTarget.All, clip);
        }
    
        public void PlayOneShotLocal(AudioClip clip) {
            _logger.Log("Non-networked PlayOneShot with "+clip);
            PlayOneShotRPC(clip);
        }

        public void PlayOneShot(AudioClip clip, float volumeScale) {
            _logger.Log("Client called PlayOneShot with "+clip+" and "+volumeScale);
            _view.RPC("PlayOneShotRPC", RpcTarget.All, clip, volumeScale);
        }

        public void PlayOneShotLocal(AudioClip clip, float volumeScale) {
            _logger.Log("Non-networked PlayOneShot with "+clip+" and "+volumeScale);
            PlayOneShotRPC(clip, volumeScale);
        }
    
        public void Stop() {
            _logger.Log("Client called Stop");
            _view.RPC("StopRPC", RpcTarget.All);
        }

        public void StopLocal() {
            _logger.Log("Non-networked Stop");
            StopRPC();
        }

        [PunRPC]
        [UsedImplicitly]
        private void PlayRPC() {
            _logger.Log("RPC received for Play");
            _source.Play();
        }

        [PunRPC]
        [UsedImplicitly]
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
        [UsedImplicitly]
        private void StopRPC() {
            _logger.Log("RPC received for Stop");
            _source.Stop();
        }

    }
}
