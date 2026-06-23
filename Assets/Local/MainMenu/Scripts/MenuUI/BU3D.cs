using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectL.Local.Scripts.MenuUI
{
    [RequireComponent(typeof(Collider))]
    public class BU3D : MonoBehaviour
    {
        [Header("click this button")]
        public UnityEvent onClick;
        [Header("Hover effect (OPTIONAL)")]
        [Tooltip("What color/material shoudl it turn when the mouse is over it >")]
        public Material HMaterial;

        public Material OGMaterial;
        private MeshRenderer meshRenderer;

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                OGMaterial = meshRenderer.material;
            }
        }
        private void OnMouseEnter()
        {
            if (meshRenderer != null && HMaterial != null)
            {
                meshRenderer.material = HMaterial;

            }
        }
        private void OnMouseExit()
        {
            if (meshRenderer != null && OGMaterial != null)
            {
                meshRenderer.material = OGMaterial;
            }
        }
        private void OnMouseDown()
        {
            Debug.Log("3d EYES CLICKED!");
            onClick?.Invoke();
        }
    }
}
