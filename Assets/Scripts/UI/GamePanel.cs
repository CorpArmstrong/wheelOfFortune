using Assets.Scripts.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private Button _spinButton;
        [SerializeField] private Button _backButton;

        private void OnEnable()
        {
            _spinButton.onClick.AddListener(SpinButtonClicked);
            _backButton.onClick.AddListener(BackButtonClicked);
            EventBus.StartListening(EventConstants.SpinFinished, SpinFinished);
        }

        private void OnDisable()
        {
            _spinButton.onClick.RemoveListener(SpinButtonClicked);
            _backButton.onClick.RemoveListener(BackButtonClicked);
            EventBus.StopListening(EventConstants.SpinFinished, SpinFinished);
        }

        private void SpinButtonClicked()
        {
            _spinButton.interactable = false;
            _backButton.interactable = false;
            EventBus.TriggerEvent(EventConstants.SpinStarted);
        }

        private void BackButtonClicked()
        {
            SceneManager.LoadScene(0);
        }

        private void SpinFinished()
        {
            _spinButton.interactable = true;
            _backButton.interactable = true;
        }
    }
}
