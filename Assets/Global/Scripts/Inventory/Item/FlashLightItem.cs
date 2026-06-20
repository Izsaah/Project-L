using System.Collections;
using System.Collections.Generic;
using ProjectL.Scripts.Interface;
using UnityEngine;
namespace ProjectL.Global.Script.Item
{
    public class FlashLightItem : MonoBehaviour
    {
        [Header("Light")]
        public Light spotLight;

        private IInputProvider inputProvider;

        private void Start()
        {
            inputProvider = GetComponentInParent<IInputProvider>();
        }

        private void Update()
        {
            if (inputProvider != null && inputProvider.GetFlashLight())
            {
                if (spotLight != null)
                {
                    spotLight.enabled = !spotLight.enabled;
                    Debug.Log("LIGHT");
                }
            }
        }
    }
}
