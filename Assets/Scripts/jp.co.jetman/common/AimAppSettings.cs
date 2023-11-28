using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jp.co.jetman.common.gameInstances;

namespace jp.co.jetman.common
{
    public enum GameMode
    {
        NKills
    }
    public enum EnemyMode
    {
        Object
    }

    public class AimAppSettings : SingletonMonoBehaviour<AimAppSettings>
    {
        public readonly int TARGET_FRAME_RATE = 60;
        public readonly GameMode GAME_MODE = GameMode.NKills;

        [Header("[Parameters(Common)]")]

        [Space(20.0f)]
        [SerializeField]
        public NKillsGameParameters N_KILLS_GAME_PARAMS;
        public object GetGameParams()
        {
            switch (GAME_MODE)
            {
                case GameMode.NKills:
                    return N_KILLS_GAME_PARAMS;
                default:                    
                    throw new Exception();
            }
        }

        [Space(20.0f)]
        [SerializeField]
        public EnemyMode ENEMY_MODE = EnemyMode.Object;
        [SerializeField]
        public GameObject PREFAB_TARGET_OBJECT;

        public readonly bool PLAYER_CAN_MOVE = false;
        public readonly bool CURSOR_CHANGE_COLOR = true;
    }
}
