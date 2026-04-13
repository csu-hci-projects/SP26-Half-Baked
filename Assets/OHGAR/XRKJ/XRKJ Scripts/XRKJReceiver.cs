using UnityEngine;

namespace OHGAR
{
    public class XRKJReceiver : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] bool _printInput;
        [Header("ID")]
        [SerializeField] int _group;

        Vector3 _input; public Vector3 Input => _input;

        private void OnEnable()
        {
            XRKJController.Output += OnInput;
        }

        private void OnInput(int group, Vector3 output)
        {
            if (group != _group) return;

            _input = output;
            if (_printInput) Debug.Log($"Group: {_group} | Input: {Input}");
        }

        private void OnDisable()
        {
            XRKJController.Output -= OnInput;
        }
    }
}