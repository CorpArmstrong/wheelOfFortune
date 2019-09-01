using Assets.Scripts.Persistance;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class Generator
    {
        private const int MinRandomNumber = 1000;
        private const int MaxRandomNumber = 1000000;

        private static List<int> _randomNumberList;
        private static Action _cachedAction;

        public static void GenerateNumbers(MonoBehaviour obj, int count, Action action)
        {
            _cachedAction = action;
            obj.StartCoroutine(GenerateRandomNumbers(count));
        }

        private static IEnumerator GenerateRandomNumbers(int count)
        {
            _randomNumberList = new List<int>(count);
            int randomNumber = 0;

            for (int i = 0; i < count; i++)
            {
                do
                {
                    randomNumber = UnityEngine.Random.Range(MinRandomNumber, MaxRandomNumber);
                }
                while (!IsFairNumber(randomNumber));

                _randomNumberList.Add(randomNumber);
                yield return null;
            }

            SessionRepository.PrizeList = _randomNumberList;
            _cachedAction?.Invoke();
        }

        private static bool IsFairNumber(int number)
        {
            bool isUnique = !_randomNumberList.Contains(number);
            bool isAliquot = number % 100 == 0;

            return isUnique && isAliquot && !IsValueCloseToRange(number);
        }

        private static bool IsValueCloseToRange(int number)
        {
            for (int i = 0, count = _randomNumberList.Count; i < count; i++)
            {
                if (Mathf.Abs(_randomNumberList[i] - number) < MinRandomNumber)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
