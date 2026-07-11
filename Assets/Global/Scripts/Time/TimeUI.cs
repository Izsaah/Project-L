using System;
using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Core.GameMaster;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
namespace ProjectL.Global.Script.Times
{

    public class TimeUI : MonoBehaviour
    {
        private TextMeshProUGUI clockText;

        private void Awake()
        {

            clockText = GetComponent<TextMeshProUGUI>();

        }
        //please let hope it fix PLEASEEEEEEEEEEEEEEEEEEEEE

        private IEnumerator Start()
        {
            while (GameManager.Instance == null || GameManager.Instance.timeManager == null)
            {
                yield return null;
            }
            GameManager.Instance.timeManager.OntimeChanged += UpdateClockDisplay;
            UpdateClockDisplay(GameManager.Instance.timeManager.currentTime);
        }

        private void UpdateClockDisplay(int totalMinutes)
        {
            // Keep time inside 24 hours in case totalMinutes goes over 1440
            totalMinutes = totalMinutes % 1440;

            int hours24 = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            string amPm = hours24 >= 12 ? "PM" : "AM";

            int hours12 = hours24 % 12;
            if (hours12 == 0)
            {
                hours12 = 12;
            }

            clockText.text = $"{hours12:00}:{minutes:00} {amPm}";
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null && GameManager.Instance.timeManager != null)
            {
                GameManager.Instance.timeManager.OntimeChanged -= UpdateClockDisplay;
            }
        }
    }

}