using System;

namespace EasyGames
{
    public interface ITweenButton
    {
        void PressedTween(Action callback = null);
        void ReleaseTween(Action callback = null);
    }
}