using System;
using jp.co.jetman.sequentialTask.spawners;
using jp.co.jetman.common.interfaces;

namespace jp.co.jetman.sequentialTask
{
    public enum SpawnMode
    {
        Sequential
    }

    public class SpawnerFactory
    {
        #region Constructor
        public SpawnerFactory()
        {
        }
        #endregion

        #region Public Methods
        public ISpawner Create()
        {
            switch (SequentialTaskBehaviour.instance.SPAWN_MODE)
            {
                case SpawnMode.Sequential:
                    return new SequentialSpawner();
                default:
                    throw new Exception();
            }
        }
        #endregion
    } 
}