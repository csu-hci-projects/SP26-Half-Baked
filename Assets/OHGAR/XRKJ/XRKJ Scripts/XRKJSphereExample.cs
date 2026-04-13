using UnityEngine;

namespace OHGAR
{
    public class XRKJSphereExample : MonoBehaviour
    {
        bool _isActive;

        public float Force;

        Vector3 _initPos;
        Vector3 _adjInput;

        Rigidbody _rigidbody;

        XRKJReceiver _receiver;

        private void Awake()
        {
            _receiver = GetComponent<XRKJReceiver>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _initPos = transform.position;
        }

        private void Update()
        {
            if (_isActive)
            {
                _adjInput = new(_receiver.Input.x, 0f, _receiver.Input.z);
            }
        }

        private void FixedUpdate()
        {
            if (_isActive)
            {
                _rigidbody.linearVelocity = _adjInput * Force;
            }
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
            transform.SetPositionAndRotation(_initPos, Quaternion.identity);
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}


