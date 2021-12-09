using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;


namespace WORLDGAMEDEVELOPMENT
{
    public abstract class FireAction : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _startAnimation = 20;

        private string _countBullet = string.Empty;
        protected Queue<GameObject> _bulletQueue = new Queue<GameObject>();
        protected Queue<GameObject> _ammunition = new Queue<GameObject>();
        protected bool _isReloading = false;

        public string CountBullet { get => _countBullet; protected set => _countBullet = value; }

        protected virtual void Start()
        {
            for (int i = 0; i < _startAnimation; i++)
            {
                GameObject bullet;
                if (_bulletPrefab == null)
                {
                    bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    bullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                }
                else
                {
                    bullet = Instantiate(_bulletPrefab);
                    bullet.SetActive(false);
                    _ammunition.Enqueue(bullet);
                }
            }
        }

        public virtual async void Reloading()
        {
            _bulletQueue = await Reload();
        }

        protected virtual void Shooting()
        {
            if (_bulletQueue.Count == 0)
            {
                Reloading();
            }
        }

        private async Task<Queue<GameObject>> Reload()
        {
            if (!_isReloading)
            {
                _isReloading = true;
                StartCoroutine(ReloadingAnim());
                return await Task.Run(() =>
                {
                    var cage = 10;
                    if (_bulletQueue.Count < cage)
                    {
                        Thread.Sleep(3000);
                        var bullets = this._bulletQueue;
                        while (bullets.Count > 0)
                        {
                            _ammunition.Enqueue(_bulletQueue.Dequeue());
                        }
                        cage = Mathf.Min(cage, _ammunition.Count);
                        if (cage > 0)
                        {
                            for (int i = 0; i < cage; i++)
                            {
                                var sphere = _ammunition.Dequeue();
                                bullets.Enqueue(sphere);
                            }
                        }
                    }
                    _isReloading = false;
                    return _bulletQueue;
                });
            }
            else
            {
                return _bulletQueue;
            }
        }

        private IEnumerator ReloadingAnim()
        {
            while (_isReloading)
            {
                CountBullet = " | ";
                yield return new WaitForSeconds(0.01f);
                CountBullet = @" \ ";
                yield return new WaitForSeconds(0.01f);
                CountBullet = "---";
                yield return new WaitForSeconds(0.01f);
                CountBullet = " / ";
                yield return new WaitForSeconds(0.01f);
            }
            CountBullet = _bulletQueue.Count.ToString();
            yield return null;
        }
    }
}
