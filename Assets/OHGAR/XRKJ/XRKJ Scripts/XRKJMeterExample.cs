using UnityEngine;

namespace OHGAR
{
    public class XRKJMeterExample : MonoBehaviour
    {
        bool _isActive;

        Quaternion _initLocalRot;

        XRKJReceiver _receiver;

        private void Awake()
        {
            _receiver = GetComponent<XRKJReceiver>();
        }

        private void Start()
        {
            _initLocalRot = transform.localRotation;
        }

        private void Update()
        {
            if (_isActive)
            {
                Quaternion rot = Quaternion.Euler(_receiver.Input.y, 0f, 0f);
                transform.localRotation = rot;
            }
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
            transform.localRotation = _initLocalRot;
        }
    }
}
