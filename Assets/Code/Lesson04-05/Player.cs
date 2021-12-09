using UnityEngine;
using UnityEngine.Networking;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Player : NetworkBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _playerPrefab;
        private GameObject _playerCharachter;

        #endregion


        #region UnityMethods

        private void Start()
        {
            SpawnCharachter();
        }

        #endregion


        #region Methods

        public void SpawnCharachter()
        {
            if (!isServer) return;

            _playerCharachter = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
            NetworkServer.SpawnWithClientAuthority(_playerCharachter, connectionToClient);
        }

        #endregion
    }
}
