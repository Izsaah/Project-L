using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.EditorTools;
using UnityEngine;
namespace ProjectL.Global.Script.Monologue
{
    public class MonologueUI : MonoBehaviour
    {
        [Header("Localization")]
        [Tooltip("Drag the db here")]
        public TextDatabase cLanguage;

        [Header("UI Setting")]
        [Tooltip("How long you wanna feel it :)")]
        public float dTime = 3f;
        [Tooltip("IM COMING FAST UHHHHHHHH")]
        public float fadeDuration = 0.5f;


        private TextMeshProUGUI monologueText;

        private void Awake()
        {
            monologueText = GetComponent<TextMeshProUGUI>();

            SetTextAlpha(0f);
        }



        public void ST(string txK)
        {
            if (monologueText != null && cLanguage != null)
            {
                string finalSentence = cLanguage.GetText(txK);

                monologueText.text = finalSentence;

                StopAllCoroutines();
                StartCoroutine(FadeS());
            }
        }

        // private IEnumerator CAT()
        // {
        //     yield return new WaitForSeconds(dTime);
        //     if (monologueText != null)
        //     {
        //         monologueText.text = "";
        //     }
        // }
        private IEnumerator FadeS()
        {
            float eTime = 0f;
            while (eTime < fadeDuration)
            {
                eTime += Time.deltaTime;
                float a = Mathf.Clamp01(eTime / fadeDuration);
                SetTextAlpha(a);
                yield return null;
            }
            SetTextAlpha(1f);

            yield return new WaitForSeconds(dTime);
            eTime = 0f;
            while (eTime < fadeDuration)
            {
                eTime += Time.deltaTime;
                float alpha = 1f - Mathf.Clamp01(eTime / fadeDuration);
                SetTextAlpha(alpha);
                yield return null;
            }
            SetTextAlpha(0f);
            monologueText.text = "";

        }
        private void SetTextAlpha(float v)
        {
            Color cColor = monologueText.color;
            cColor.a = v;
            monologueText.color = cColor;
        }
    }

}