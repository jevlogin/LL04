using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class PlayerCharachter : Charachter
    {
        #region PrivateFields

        private const float _gravity = -9.8f;

        #endregion


        #region Fields

        [SerializeField] private MouseLook _mouseLook;
        [Range(0.5f, 10.0f), SerializeField] private float _movingSpeed = 8.0f;
        [SerializeField] private float _movingRotation;
        [SerializeField] private float _acceleration = 3.0f;
        [Range(0, 100), SerializeField] private int _health = 100;

        protected override FireAction _fireAction { get; set; }
        private CharacterController _characterController;
        private Vector3 _currentVelocity;

        #endregion


        #region UnityMethods

        private void Start()
        {
            Initiate();
        }

        #endregion


        #region Methods

        protected override void Initiate()
        {
            base.Initiate();
            _fireAction = gameObject.AddComponent<RayShooter>();
            _fireAction.Reloading();

            _characterController = GetComponentInChildren<CharacterController>();
            _characterController ??= gameObject.AddComponent<CharacterController>();

            _mouseLook = GetComponentInChildren<MouseLook>();
            _mouseLook ??= gameObject.AddComponent<MouseLook>();
        }

        public override void Movement()
        {
            if (_mouseLook != null && _mouseLook.PlayerCamera != null)
            {
                _mouseLook.PlayerCamera.enabled = hasAuthority;
            }
            if (hasAuthority)
            {
                var moveX = Input.GetAxis("Horizontal") * _movingSpeed;
                var moveZ = Input.GetAxis("Vertical") * _movingSpeed;
                var movement = new Vector3(moveX, 0.0f, moveZ);
                movement = Vector3.ClampMagnitude(movement, _movingSpeed);
                movement *= Time.deltaTime;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    movement *= _acceleration;
                }
                movement.y = _gravity;
                movement = transform.TransformDirection(movement);

                _characterController.Move(movement);
                _mouseLook.Rotation();

                CmdUpdateTransform(transform.position, transform.rotation);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, _serverPosition, ref _currentVelocity, _movingSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, _serverRotation, _movingRotation * Time.deltaTime);
            }
        }

        #endregion


        #region OnGUI

        private void OnGUI()
        {
            if (Camera.main == null)
            {
                return;
            }

            var info = $"Health: {_health}\nClip: {_fireAction.CountBullet}";
            var size = 12;
            var bulletCountSize = 50;
            var posX = Camera.main.pixelWidth / 2 - size / 4;
            var posY = Camera.main.pixelHeight / 2 - size / 2;
            var posXBul = Camera.main.pixelWidth - bulletCountSize * 2;
            var posYBul = Camera.main.pixelHeight - bulletCountSize;
            GUI.Label(new Rect(posX, posY, size, size), "+");
            GUI.Label(new Rect(posXBul, posYBul, bulletCountSize * 2, bulletCountSize * 2), info);
        }

        #endregion
    }
}
