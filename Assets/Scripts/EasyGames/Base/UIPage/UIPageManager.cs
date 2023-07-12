using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyGames
{
    public class UIPageManager : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<UIPageType, UIPage> _pages = new();

        public UIPageType LastPageType { get; set; } = UIPageType.MainMenu;

        private UIPageType _focusPageType = UIPageType.MainMenu;
        public UIPageType FocusPageType
        {
            get => _focusPageType;
            set
            {
                LastPageType = _focusPageType;
                _focusPageType = value;
            }
        }

        // public void ShowPage(UIPageType pageType, bool instant)
        // {
        //     if (!_pages.ContainsKey(pageType))
        //         return;
        //     _pages[pageType].Show(instant);
        // }
        //
        // public void HidePage(UIPageType pageType, bool instant)
        // {
        //     if (!_pages.ContainsKey(pageType))
        //         return;
        //     _pages[pageType].Hide(instant);
        // }

        public void ShowPageFocus(UIPageType pageType, bool instant)
        {
            foreach (var page in _pages)
            {
                if (page.Key != pageType)
                {
                    page.Value.Hide(instant);
                    continue;
                }
                page.Value.Show(instant);
                FocusPageType = pageType;
            }
        }
    }
}
