using UnityEngine;
using UnityEngine.Networking;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class MouseLook : NetworkBehaviour
    {
        #region Fields

        [Range(0.0f, 10.0f), SerializeField] private float _sensivity = 2.0f;
        [Range(-90.0f, 0.0f), SerializeField] private float _minVert = -45.0f;
        [Range(0.0f, 90.0f), SerializeField] private float _maxVert = 45.0f;

        [SerializeField] private Camera _camera;
        private float _rotationX = 0.0f;
        private float _rotationY = 0.0f;

        #endregion


        #region Properties

        public Camera PlayerCamera => _camera;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _camera = GetComponentInChildren<Camera>();
            var ridgidBody = GetComponentInChildren<Rigidbody>();

            if (ridgidBody != null)
            {
                ridgidBody.freezeRotation = true;
            }
        }

        #endregion


        #region Methods

        public void Rotation()
        {
            _rotationX -= Input.GetAxis("Mouse Y") * _sensivity;
            _rotationY += Input.GetAxis("Mouse X") * _sensivity;
            _rotationX = Mathf.Clamp(_rotationX, _minVert, _maxVert);
            transform.rotation = Quaternion.Euler(0, _rotationY, 0);
            _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        }

        #endregion
    }
}
