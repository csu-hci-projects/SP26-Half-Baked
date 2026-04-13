using UnityEngine;

namespace OHGAR
{
    public class XRKJPixelExample : MonoBehaviour
    {
        public float PixelSpeed;
        [SerializeField] XRKJOutputSO _inputScriptableObject;

        bool _isActive;

        Vector3 _initPos;
        Vector3 _adjInput;

        private void Start()
        {
            _initPos = transform.position;
        }

        private void Update()
        {
            if (_isActive)
            {
                _adjInput = new(_inputScriptableObject.Value.x, _inputScriptableObject.Value.z, 0f);
                transform.Translate(PixelSpeed * Time.deltaTime * _adjInput);
            }
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
            transform.position = _initPos;
        }
    }
}