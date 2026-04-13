using UnityEngine;
using UnityEngine.Events;


namespace OHGAR
{
    public class XRKJController : MonoBehaviour
    {
        [Header("Automation")]
        [Tooltip("If true, rigid body and grab interactable settings will be changed OnStart via SetJoystick & SetKnob methods (Determined by Is Knob in knob parameters).")]
        [SerializeField] bool _autoSettings = true;
        [Tooltip("Use this to test joystick movent without XR (Move the grab interactable game object in the editor to see movement).\n\nWARNING! OnDeactivate won't be called if moving obj in editor.")]
        [SerializeField] bool _activateOnStart = false;

        [Header("Output")]
        [Tooltip("Will Debug.Log the group, output, and Dead Zone status.")]
        [SerializeField] bool _printOutput = false;
        [Tooltip("Use this to pair joysticks to specific receivers.")]
        [SerializeField] int _group = 0;

        [Space]
        [Tooltip("Required if using receiver component, otherwise optional.")]
        [SerializeField] bool _useDelegate = true;
        [Tooltip("Optional: Use this to avoid using the output Delegate, receiver componenet, and groups. A unique scriptable object will need to be created for each joystick.\n\nNOTE: Can be used in addition to Delegate.")]
        [SerializeField] XRKJOutputSO _outputScriptableObject;
        [Header("Joystick Parameters")]
        [Tooltip("How fast the pivot rotates (Not used as knob).")]
        [SerializeField][Min(0f)] float _sensitivity = 4f;
        [Tooltip("The amount the pivot can rotate.")]
        [SerializeField][Min(0f)] float _maxPivotRadius = .04f;

        [Space]
        [Tooltip("If true, output will equal Vector3.zero if Grab distance from this position is less than Dead Zone Radius")]
        [SerializeField] bool _useDeadZone = true;
        [SerializeField][Min(0.0001f)] float _deadZoneRadius;

        [Header("Knob Parameters")]
        [Tooltip("Will Output Grab Interactable's local euler angles.")]
        [SerializeField] bool _isKnob = false;

        [Header("References")]
        [Tooltip("Transform using XR Grab Interactable.")]
        [SerializeField] Transform _grab;
        [Tooltip("Rotation point (Rotates based on Grab local position).")]
        [SerializeField] Transform _pivot;    

        [Space]
        public UnityEvent OnActivate;
        public UnityEvent OnDeactivate;

        bool _isActive;
        bool _isNullRef;
        bool _isDeadZone;

        Quaternion _initPivitRot;

        Rigidbody _grabRb;
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabInteractable;

        public delegate void XRKinematicJoystickOutputDelegate(int group, Vector3 output);
        public static event XRKinematicJoystickOutputDelegate Output;

        private void Awake()
        {
            _grabRb = _grab.GetComponent<Rigidbody>();
            _grabInteractable = _grab.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }

        private void Start()
        {
            NullReferenceCheck();
            Initialize();
        }

        private void Update()
        {
            if (_isNullRef) return;

            if (_isActive)
            {
                Clamp();
                DeadZoneCheck();
                Move();
            }
        }

        private void Initialize()
        {
            if (_outputScriptableObject != null) { _outputScriptableObject.Value = Vector3.zero; }

            _initPivitRot = _pivot.rotation;

            if (_autoSettings)
            {
                if (_isKnob)
                {
                    SetKnob();
                }
                else
                {
                    SetJoystick();
                }
            }

            if (_activateOnStart) Activate();
        }

        public void SetJoystick()
        {
            _isKnob = false;

            _grabRb.constraints &= ~RigidbodyConstraints.FreezeAll;
            _grabRb.constraints = RigidbodyConstraints.FreezeRotation;

            if (_grabInteractable != null)
            {
                _grabInteractable.trackPosition = true;
                _grabInteractable.trackRotation = false;
            }
        }

        public void SetKnob()
        {
            _isKnob = true;

            _grabRb.constraints = RigidbodyConstraints.FreezeAll;
            _grabRb.constraints &= ~RigidbodyConstraints.FreezeRotationY;

            if (_grabInteractable != null)
            {
                _grabInteractable.trackPosition = false;
                _grabInteractable.trackRotation = true;
            }
        }

        private void Clamp()
        {
             _grab.position = transform.position + Vector3.ClampMagnitude(_grab.position - transform.position, _maxPivotRadius);
        }

        public void DeadZoneCheck()
        {
            if (!_useDeadZone) return;
            
            float distance = Vector3.Distance(transform.position, _grab.position);

            _isDeadZone = distance <= _deadZoneRadius;            
        }

        private void Move()
        {
            if (_isKnob)
            {
                _pivot.localRotation = new(0f, _grab.localRotation.y, 0f, 1f);
            }
            else
            {
                _pivot.localRotation = new(_grab.localPosition.z * _sensitivity, _grab.localPosition.y, -_grab.localPosition.x * _sensitivity, 1);
            }

            ProcOutput();
        }

        private void NullReferenceCheck()
        {
            if (_grab == null || _pivot == null)
            {
                _isNullRef = true;
            }
        }

        public void Activate()
        {
            _grab.parent = transform;
            _grab.rotation = transform.rotation;
            _isActive = true;
            OnActivate.Invoke();
        }

        public void Deactivate()
        {
            _grab.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            ProcOutput();
            _pivot.rotation = _initPivitRot;
            _isActive = false;
            OnDeactivate.Invoke();
        }

        private void ProcOutput()
        {
            if (_isKnob)
            {
                if (_outputScriptableObject != null) { _outputScriptableObject.Value = _grab.localEulerAngles; }

                if (_useDelegate)
                {
                    Output?.Invoke(_group, _grab.localEulerAngles);
                    if (_printOutput) Debug.Log($"{name}: Group = {_group} | Output = {_grab.localEulerAngles}");
                }
            }
            else
            {
                if ( _isDeadZone)
                {
                    if (_outputScriptableObject != null) { _outputScriptableObject.Value = Vector3.zero; }
                    if (_useDelegate) Output?.Invoke(_group, Vector3.zero);
                }
                else
                {
                    if (_outputScriptableObject != null) { _outputScriptableObject.Value = _grab.localPosition; }
                    if (_useDelegate) Output?.Invoke(_group, _grab.localPosition);
                }

                string output = _isDeadZone ? Vector3.zero.ToString() : _grab.localPosition.ToString();
                if (_printOutput) Debug.Log($"{name}: Group = {_group} | Output = {output} | Dead Zone = {_isDeadZone}");
            }
        }
    }
}