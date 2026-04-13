using UnityEngine;

namespace OHGAR
{
    public class XRKJCubeExample : MonoBehaviour
    {
        [SerializeField] float _moveSpeed;
        [SerializeField] float _rotationSpeed;
        [SerializeField] XRKJReceiver[] _receivers;

        bool _isMoving;
        bool _isRotating;

        Vector3 _initPos;
        Quaternion _initRot;

        private void Start()
        {
            _initPos = transform.position;
            _initRot = transform.rotation;
        }

        private void Update()
        {
            if (_isMoving)
            {
                Vector3 posInput = new(_receivers[0].Input.x, 0f, _receivers[0].Input.z);
                posInput *= Time.deltaTime * _moveSpeed;
                transform.Translate(posInput);
            }

            if (_isRotating)
            {
                Vector3 rotInput = _rotationSpeed * Time.deltaTime * _receivers[1].Input;
                transform.Rotate(rotInput.z, 0f, rotInput.x);
            }
        }

        public void ActivateMovement()
        {
            _isMoving = true;
        }

        public void DeactivateMovement()
        {
            _isMoving = false;
            transform.position = _initPos;
        }

        public void ActivateRotation()
        {
            _isRotating = true;
        }

        public void DeactivateRotation()
        {
            _isRotating = false;
            transform.rotation = _initRot;
        }
    }
}

