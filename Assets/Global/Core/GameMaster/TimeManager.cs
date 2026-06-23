using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectL.Global.Core.GameMaster
{

    public class TimeManager : MonoBehaviour
    {
        [Header("Time Settings (Minutes)")]
        [Tooltip("480 = 8:00 AM start time (you can change time here)")]
        public int currentTime = 480;
        [Tooltip("1080=6:00 PM End time")]
        public int dTime = 1080;

        public event Action<int> OntimeChanged;
        public event Action OnDeadlineReached;

        private bool deadlineHit = false;

        private void Start()
        {
            OntimeChanged?.Invoke(currentTime);
        }

        public void UpdateTime(int minute)
        {
            if (deadlineHit) return;
            currentTime += minute;
            Debug.Log($"advanced time by {minute} current time is {currentTime}");

            OntimeChanged?.Invoke(currentTime);

            if (currentTime >= dTime)
            {
                deadlineHit = true;
                Debug.Log("no more time left");
                OnDeadlineReached?.Invoke();
            }
        }
    }

}