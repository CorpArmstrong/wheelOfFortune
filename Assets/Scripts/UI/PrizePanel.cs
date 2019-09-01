using Assets.Scripts.Events;
using Assets.Scripts.Persistance;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PrizePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _prizeLabel;
        private int _currentScore;

        private void OnEnable()
        {
            EventBus.StartListening(EventConstants.SpinReward, UpdatePrize);
        }

        private void OnDisable()
        {
            EventBus.StopListening(EventConstants.SpinReward, UpdatePrize);
        }

        private void UpdatePrize(int score)
        {
            _currentScore += score;
            _prizeLabel.text = _currentScore.ConvertK();
            SessionRepository.SaveScore(_currentScore);
        }
    }
}
