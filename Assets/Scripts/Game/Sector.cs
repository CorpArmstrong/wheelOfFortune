using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Sector : MonoBehaviour
    {
        private readonly int _WinSectorHash = Animator.StringToHash("Prize");
        [SerializeField] private TextMesh _prizeLabel;

        private int _prize;
        private Animator _animator;

        public int Prize
        {
            get { return _prize; }

            set
            {
                _prizeLabel.text = string.Format(Constants.SectorPrizeTemplate, value);
                _prize = value;
            }
        }

        public void PlayWon()
        {
            _animator.SetTrigger(_WinSectorHash);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
    }
}
