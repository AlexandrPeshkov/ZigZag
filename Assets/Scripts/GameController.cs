using UnityEngine;
using UnityEngine.UI;

namespace ZigZag
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private PlatformManager _platformManager;

        [SerializeField]
        private SphereController _sphereController;

        [SerializeField]
        private LoseMenu _loseMenu;

        [SerializeField]
        private Text _scoreLabel;

        private int _score = 0;

        private void Awake()
        {
            _platformManager.PlatfromComplete += OnPlatformComplete;
            _loseMenu.NewGamewClicked += OnNewGameClicked;
            _sphereController.Falling += OnGameFailed;
        }

        private void OnGameFailed()
        {
            _loseMenu.Show();
        }

        private void OnNewGameClicked()
        {
            RestartGame();
        }

        private void RestartGame()
        {
            AddScore(-_score);
            _sphereController.ResetSphere();
            _platformManager.ResetPlatforms();
        }

        private void OnPlatformComplete()
        {
            AddScore();
        }

        private void AddScore(int points = 1)
        {
            _score += points;
            _scoreLabel.text = _score.ToString();
        }
    }
}