using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.Persistance;
using Assets.Scripts.Events;
using DG.Tweening;
using Assets.Scripts.Utils;

public class SpinWheel : MonoBehaviour
{
    private const float RotationTime = 2.5f;
    private const float RotationAngle = -15f;

    public List<AnimationCurve> animationCurves;

    [SerializeField] private List<int> _prizeList;
    [SerializeField] private List<Sector> _sectorList;

    private bool _isSpinning;	
	private float anglePerItem;	
	private int randomTime;
	private int itemNumber;
    
    private Sector _wonSector;
    private AnimationCurve _motionCurve;

    private Vector3 _extraRotationVector = new Vector3(0f, 0f, -Constants.DegreesInCircle);
    private Vector3 _rotateToVector = Vector3.zero;
    private Vector3 _rotationVector = Vector3.zero;

	private void Start()
    {
        AssignSectorValues();
		anglePerItem = Constants.DegreesInCircle / _prizeList.Count;
        EventBus.StartListening(EventConstants.SpinStarted, StartSpin);
	}

    private void OnDestroy()
    {
        EventBus.StopListening(EventConstants.SpinStarted, StartSpin);
    }

    private void AssignSectorValues()
    {
        _prizeList = SessionRepository.PrizeList;

        for (int i = 0, count = _sectorList.Count; i < count; i++)
        {
            _sectorList[i].Prize = _prizeList[i];
        }
    }

    private void StartSpin()
    {
        randomTime = Random.Range(1, 4);
        itemNumber = Random.Range(0, _prizeList.Count);
        float maxAngle = -Constants.DegreesInCircle + (itemNumber * anglePerItem);
        _wonSector = _sectorList[itemNumber];
        _motionCurve = animationCurves[1];

        StartCoroutine(SpinTheWheel(RotationTime * randomTime, maxAngle));
    }

    private IEnumerator SpinTheWheel(float spinTime, float maxAngle)
    {
        _isSpinning = true;
        float currentSpinTime = 0.0f;
        float startAngle = transform.eulerAngles.z;

        while (currentSpinTime < spinTime)
        {
            float angle = RotationAngle * _motionCurve.Evaluate(currentSpinTime / spinTime);
            _rotationVector.z = angle;
            transform.eulerAngles += _rotationVector;
            currentSpinTime += Time.deltaTime;
            yield return null;
        }

        var wonSectorZ = _wonSector.transform.eulerAngles.z;
        var circleZ = transform.localRotation.eulerAngles.z;
        var x = wonSectorZ >= circleZ ? Constants.DegreesInCircle : 0f;
        var endZ = circleZ + (x - wonSectorZ);

        _rotateToVector.z = endZ;
        bool isRight = GetRotateDirection(transform.rotation, Quaternion.Euler(_wonSector.transform.eulerAngles));

        _rotateToVector.z = isRight ? -Constants.DegreesInCircle + _rotateToVector.z : _rotateToVector.z;

        transform.DORotate(_rotateToVector + _extraRotationVector, RotationTime, RotateMode.FastBeyond360)
            .OnComplete(OnRotationCompleted)
            .SetEase(Ease.OutSine);
    }

    private bool GetRotateDirection(Quaternion from, Quaternion to)
    {
        float fromZ = from.eulerAngles.z;
        float toZ = to.eulerAngles.z;
        float clockWise = 0f;
        float counterClockWise = 0f;

        if (fromZ <= toZ)
        {
            clockWise = toZ - fromZ;
            counterClockWise = fromZ + (Constants.DegreesInCircle - toZ);
        }
        else
        {
            clockWise = (Constants.DegreesInCircle - fromZ) + toZ;
            counterClockWise = fromZ - toZ;
        }

        return (clockWise <= counterClockWise);
    }

    private void OnRotationCompleted()
    {
        _isSpinning = false;
        _wonSector.PlayWon();

        EventBus.TriggerEvent(EventConstants.SpinFinished);
        EventBus.TriggerEvent(EventConstants.SpinReward, _wonSector.Prize);
    }
}