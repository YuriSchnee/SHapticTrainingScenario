using UnityEngine;
using jp.co.jetman.common.interfaces;

namespace jp.co.jetman.common.gameInstances
{
    abstract public class GameInstanceAbstract : IGameInstance
    {
        protected bool _isPlaying = false;
        public bool isPlaying
        {
            get
            {
                return _isPlaying;
            }
        }
        protected bool _needReset = true;
        public bool needReset
        {
            get
            {
                return _needReset;
            }
        }

        protected ISpawner spawner;

        #region Constructor
        public GameInstanceAbstract()
        {
            //
        }
        #endregion

        #region IGameInstance
        virtual public void Init(MonoBehaviour _context, ISpawner _spawner)
        {
            spawner = _spawner;
            spawner.Init(_context);
        }
        virtual public void Reset()
        {
            if (needReset)
            {
                spawner.Reset();

                _needReset = false;
            }
        }
        virtual public void Play()
        {
            if (!needReset && !isPlaying)
            {
                spawner.Spawn();

                _needReset = true;
                _isPlaying = true;
            }
        }
        virtual public void Stop()
        {
            _isPlaying = false;
        }
        abstract public void Update();
        virtual public void Aim(TargetBehaviour _other)
        {
            // idle
        }
        virtual public void Hit(TargetBehaviour _other)
        {
            spawner.Respawn(_other);
        }
        #endregion
    }
}