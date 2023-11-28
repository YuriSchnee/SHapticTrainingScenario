

using System.Diagnostics;

namespace jp.co.jetman.common.gameInstances
{
    class NKillsGameInstance : GameInstanceAbstract
    {
        #region Constructor
        public NKillsGameInstance() {}
        #endregion

        #region GameInstanceAbstract
        // override public void Init(MonoBehaviour _context)
        // {
            // base.Init(_context);
        // }
        override public void Reset()
        {
            // base.Reset();

            if (needReset)
            {
                TimerBehaviour.instance.Reset();
                ScorerBehaviour.instance.Reset(((NKillsGameParameters) AimAppSettings.instance.GetGameParams()).TOTAL_NUMBER_OF_ENEMIES_TO_KILL);

                spawner.Reset();

                _needReset = false;
            }
        }
        override public void Play()
        {
            // base.Play();

            if (!needReset && !isPlaying)
            {
                TimerBehaviour.instance.Play();

                spawner.Spawn();
                _needReset = true;
                _isPlaying = true;
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
                if (!(ScorerBehaviour.instance.anyEnemiesRemaining))
                {
                    TimerBehaviour.instance.Stop();
                }
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