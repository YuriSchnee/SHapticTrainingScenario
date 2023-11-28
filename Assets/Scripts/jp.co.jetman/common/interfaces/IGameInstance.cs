using UnityEngine;

namespace jp.co.jetman.common.interfaces
{
    public interface IGameInstance
    {
        bool isPlaying { get; }
        bool needReset { get; }

        void Init(MonoBehaviour _context, ISpawner _spawner);
        void Reset();
        void Play();
        void Stop();
        void Update();
        void Aim(TargetBehaviour _other);
        void Hit(TargetBehaviour _other);
    }
}
