using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectL.Global.Script.interaction;
namespace ProjectL.Global.Core.GameMaster
{
    [RequireComponent(typeof(DroppedItemManager))]
    public class DroppedItemRadar : MonoBehaviour
    {
        [Header("Radar Optimization")]
        public float spawnDistance = 100f;
        public int checksPerFrame = 50;
        public float radarPingInterval = 1f;

        private DroppedItemManager manager;
        private Transform playerTracker;

        private void Start()
        {
            manager = GetComponent<DroppedItemManager>();
            StartCoroutine(RadarLoop());
        }

        private IEnumerator RadarLoop()
        {
            while (true)
            {
                if (playerTracker == null)
                {
                    GameObject p = GameObject.FindGameObjectWithTag("Player");
                    if (p != null) playerTracker = p.transform;
                }
                if (playerTracker != null && manager.allDropItems.Count > 0)
                {
                    yield return StartCoroutine(PIR());
                }
                yield return new WaitForSeconds(radarPingInterval);
            }
        }
        private IEnumerator PIR()
        {
            for (int i = 0; i < manager.allDropItems.Count; i++)
            {
                PSI(manager.allDropItems[i]);
                if ((i + 1) % checksPerFrame == 0)
                {
                    yield return null;
                }

            }
        }
        private void PSI(DroppedItemData drop)
        {
            float dis = Vector3.Distance(playerTracker.position, drop.dropPos);
            bool iCE = dis < spawnDistance;
            if (iCE && drop.phy3Dmodel == null)
            {
                drop.phy3Dmodel = Instantiate(drop.itemInf.fab, drop.dropPos, Quaternion.identity);
                ItemPickup pickup = drop.phy3Dmodel.GetComponent<ItemPickup>();
                if (pickup != null)
                {
                    pickup.runtimeDropID = drop.dropId;
                }
            }
            else if (!iCE && drop.phy3Dmodel != null)
            {
                Destroy(drop.phy3Dmodel);
                drop.phy3Dmodel = null;
            }
        }

    }

}