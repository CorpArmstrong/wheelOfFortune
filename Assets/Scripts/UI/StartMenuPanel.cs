using Assets.Scripts.Persistance;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class StartMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private TMP_Text _previousScoreLabel;

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(StartNewGame);
            _exitGameButton.onClick.AddListener(ExitGame);

            UpdatePreviousPrize();
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(StartNewGame);
            _exitGameButton.onClick.RemoveListener(ExitGame);
        }

        private void StartNewGame()
        {
            Generator.GenerateNumbers(this, Constants.NumSectors, LoadGameScene);
        }

        private void ExitGame()
        {
            Application.Quit(0);
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene(1);
        }

        private void UpdatePreviousPrize()
        {
            int previousScore = SessionRepository.LoadScore();

            _previousScoreLabel.gameObject.SetActive(previousScore > 0);
            _previousScoreLabel.text = string.Format(Constants.PreviousScoreTemplate, previousScore.ConvertK());
        }
    }
}
