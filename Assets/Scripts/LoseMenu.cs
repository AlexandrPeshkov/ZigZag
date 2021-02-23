using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZigZag
{
    public class LoseMenu : MonoBehaviour
    {
        [SerializeField]
        private Button _newGameButton;

        public event Action NewGamewClicked;

        private void Awake()
        {
            _newGameButton.onClick.AddListener(OnNewGameClick);
        }

        private void OnNewGameClick()
        {
            NewGamewClicked?.Invoke();
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}