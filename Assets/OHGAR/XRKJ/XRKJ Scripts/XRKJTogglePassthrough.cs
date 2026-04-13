using UnityEngine;

namespace OHGAR
{
    public class XRKJTogglePassthrough : MonoBehaviour
    {
        [SerializeField] MeshRenderer[] _meshRends;

        bool _isPass;

        public void TogglePassthrough()
        {
            if (_isPass)
            {
                _isPass = false;
                foreach(MeshRenderer meshRend in _meshRends)
                {
                    meshRend.enabled = true;
                }
                Camera.main.backgroundColor = new(0, 0, 0, 1);
            }
            else
            {
                _isPass = true;
                foreach (MeshRenderer meshRend in _meshRends)
                {
                    meshRend.enabled = false;
                }
                Camera.main.backgroundColor = new(0, 0, 0, 0);
            }
        }
    }
}
