using OHGAR;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotationSpeed;
    [SerializeField] XRKJReceiver[] _receivers;

    bool _isMoving;
    bool _isRotating;
    [SerializeField] bool _isDropping;
    bool _isLifting;

    [SerializeField] float _defaultY = 2;
    [SerializeField] float _bottomY = 1.1f;
    [SerializeField] float _interpolationSpeed = 0.5f;
    float _startY;
    float _targetY;
    float _t;

    Vector3 _initPos;
    Quaternion _initRot;

    public ExperimentController controller;

    private void Start()
    {
        _initPos = transform.position;
        _initRot = transform.rotation;
        _isDropping = false;
    }

    private void Update()
    {
        if (!_isDropping)
        {
            if (_isMoving)
            {
                Vector3 posInput = new(_receivers[0].Input.x, 0f, _receivers[0].Input.z);
                posInput *= Time.deltaTime * _moveSpeed;
                transform.Translate(posInput);
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -0.45f, 0.45f), transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, -0.45f, 0.45f));
            }

            if (_isRotating)
            {
                Vector3 rotInput = _rotationSpeed * Time.deltaTime * _receivers[1].Input;
                transform.Rotate(rotInput.z, 0f, rotInput.x);
            }
        }
        else
        {
            float _currentY = Mathf.Lerp(_startY, _targetY, _t);
            transform.localPosition = new Vector3(transform.localPosition.x, _currentY, transform.localPosition.z);
            _t += _interpolationSpeed * Time.deltaTime;

            if(!_isLifting && (_currentY - _targetY < 0.05f))
            {
                _isLifting = true;
                _startY = _targetY;
                _targetY = _defaultY;
                _t = 0.0f;
            }

            if(_isLifting && (_targetY - _currentY < 0.05f))
            {
                transform.localPosition = new Vector3(transform.localPosition.x, _defaultY, transform.localPosition.z);
                _isLifting = false;
                _isDropping = false;
                OnDropEnded();
            }
        }
    }

    public void ActivateMovement()
    {
        _isMoving = true;
    }

    public void DeactivateMovement()
    {
        _isMoving = false;
        //transform.position = _initPos;
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

    public void Drop()
    {
        if (!_isDropping)
        {
            _startY = _defaultY;
            _targetY = _bottomY;
            _isDropping = true;
            _isLifting = false;
            _t = 0.0f;
        }
    }

    public void OnDropEnded()
    {
        controller.DropEnded();
    }
}
