using System;
using UnityEngine;
using UnityEngine.Networking;


namespace WORLDGAMEDEVELOPMENT
{
    public abstract class Charachter : NetworkBehaviour
    {
        #region Fields

        [SyncVar] protected Vector3 _serverPosition;
        [SyncVar] protected Quaternion _serverRotation;

        #endregion


        #region Properties

        protected abstract FireAction _fireAction { get; set; }
        protected Action _onUpdateAxtion { get; set; }

        #endregion


        #region UnityMethods

        private void Update()
        {
            OnUpdate();
        }

        #endregion


        #region Methods

        protected virtual void Initiate()
        {
            _onUpdateAxtion += Movement;
        }

        private void OnUpdate()
        {
            _onUpdateAxtion?.Invoke();
        }

        public abstract void Movement();

        [Command]
        protected void CmdUpdateTransform(Vector3 position, Quaternion rotation)
        {
            _serverPosition = position;
            _serverRotation = rotation;
        }

        #endregion
    }
}
