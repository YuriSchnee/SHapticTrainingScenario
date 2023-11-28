
namespace jp.co.jetman.common.gameInstances
{
    class CountDownGameInstance : GameInstanceAbstract
    {
        #region Constructor
        public CountDownGameInstance() {}
        #endregion

        #region Private Methods
        #endregion

        #region GameInstanceAbstract
        // override public void Init(MonoBehaviour _context)
        // {
            // base.Init(_context);
        // }
        override public void Reset()
        {
            // base.Reset();

            if (needReset && !isPlaying)
            {
                TimerBehaviour.instance.Reset(((CountDownGameParameters) AimAppSettings.instance.GetGameParams()).TIMER_DURATION);
                ScorerBehaviour.instance.Reset();

                spawner.Reset();

                _needReset = false;
            }
        }
        override public void Play()
        {
            // base.Play();

            if (!needReset && !isPlaying)
            {
                spawner.Spawn();
                _needReset = true;
                _isPlaying = true;
                TimerBehaviour.instance.Play();
            }
        }
        override public void Stop()
        {
            // base.Stop();

            if (isPlaying)
                TimerBehaviour.instance.Stop();
        }
        override public void Update()
        {
            if (isPlaying)
            {
                _isPlaying = TimerBehaviour.instance.isRunning;
            }
        }
        // override public void Aim(RaycastHit _hit)
        // {
            // base.Aim(_hit);
        // }
        // override public void Hit(RaycastHit _hit)
        // {
            // base.Hit(_hit);
        // }
        #endregion
    }
}