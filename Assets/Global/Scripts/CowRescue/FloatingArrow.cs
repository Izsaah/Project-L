using UnityEngine;

namespace ProjectL.Global.Script.CowRescue
{
    public class FloatingArrow : MonoBehaviour
    {
        public float floatSpeed = 2f;
        public float floatAmount = 0.2f;
        public float rotationSpeed = 50f;

        private Vector3 startPos;

        private void Start()
        {
            startPos = transform.localPosition;
        }

        private void Update()
        {
            // Bob up and down
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            transform.localPosition = new Vector3(startPos.x, newY, startPos.z);

            // Optional: Rotate slowly
            // transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
