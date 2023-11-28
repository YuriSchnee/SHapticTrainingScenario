using System;
using jp.co.jetman.common.gameInstances;
using jp.co.jetman.common.interfaces;

using jp.co.jetman.common;

namespace jp.co.jetman.common
{
    public class GameInstanceFactory
    {
        #region Constructor
        public GameInstanceFactory()
        {
        }
        #endregion

        #region Public Methods
        public IGameInstance Create()
        {
            switch (AimAppSettings.instance.GAME_MODE)
            {
                case GameMode.NKills:
                    return new NKillsGameInstance();
                default:
                    throw new Exception();
            }
        }
        #endregion
    }
}