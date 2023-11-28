using UnityEngine;

namespace jp.co.jetman.common.interfaces
{
    public interface ISpawner
    {
        void Init(MonoBehaviour _context);
        void Reset();
        void Spawn();
        void Respawn(TargetBehaviour _other);
    }
}