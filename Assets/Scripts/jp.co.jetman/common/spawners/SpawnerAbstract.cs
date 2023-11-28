using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jp.co.jetman.common;
using jp.co.jetman.common.interfaces;

namespace jp.co.jetman.common.spawners
{
    /*
◆フロー◆

初期配置時
Spawn() > makeTargetObjects() > makeTargetObject()

１体破壊時
Respawn() > [delay] > respawn() [> makeTargetObject()]
    */
    abstract public class SpawnerAbstract : ISpawner
    {
        protected readonly float OFFSET_Y_AXIS_DEGREE = 90.0f;

        protected MonoBehaviour context = null;
        protected int currentId = 1;

        protected List<TargetBehaviour> targets = new List<TargetBehaviour>();

        #region Constructor
        public SpawnerAbstract() {}
        #endregion

        #region Private Methods

        protected GameObject getPrefab()
        {
            switch (AimAppSettings.instance.ENEMY_MODE)
            {
                case EnemyMode.Object:
                    return AimAppSettings.instance.PREFAB_TARGET_OBJECT;
                default:
                    throw new Exception();
            }
        }
        protected void deleteAll()
        {
            var comps = context.gameObject.GetComponentsInChildren<TargetBehaviour>();
            foreach (var comp in comps)
            {
                GameObject.Destroy(comp.gameObject);
            }
        }
        protected IEnumerator delayMethod(float _delay, Action _action)
        {
            yield return new WaitForSeconds(_delay);
            _action();
        }

        // common
        abstract protected void makeTargets();
        abstract protected void makeTarget(int _id, Vector3 _position);
        // object
        abstract protected void makeTargetObjects();
        abstract protected void makeTargetObject(int _id, Vector3 _position);
        // humanoid
        abstract protected void makeTargetHumanoids();
        abstract protected void makeTargetHumanoid(int _id, Vector3 _position);
        // respawn
        abstract protected void respawn(int _id, Vector3 _position);
        #endregion

        #region ISpawner
        virtual public void Init(MonoBehaviour _context)
        {
            context = _context;
        }
        virtual public void Reset()
        {
            deleteAll();
        }
        virtual public void Spawn()
        {
            makeTargets();

            // if you need the logic separated, use below code at sub-class via override.
            // 
            // switch (AimAppSettings.instance.ENEMY_MODE)
            // {
            //     case EnemyMode.Object:
            //         makeTargetObjects();
            //         break;
            //     case EnemyMode.Humanoid:
            //         makeTargetHumanoids();
            //         break;
            // }
        }
        abstract public void Respawn(TargetBehaviour _other);
        #endregion
    }
}