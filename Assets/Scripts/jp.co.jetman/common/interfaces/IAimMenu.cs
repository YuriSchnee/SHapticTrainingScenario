using UnityEngine;

namespace jp.co.jetman.common.interfaces
{
    public interface IAimMenu
    {
        bool inGame { get; }
        bool playing { get; }

        void Play();
        void Reset();
        void Aim(TargetBehaviour _other);
        void Shot();
        void Hit(TargetBehaviour _other);

        void Show();
        void Hide();
    }
}