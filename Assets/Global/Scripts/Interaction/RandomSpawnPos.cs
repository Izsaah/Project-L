using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Core.GameMaster;
using UnityEngine;
namespace ProjectL.Global.Script.interaction
{
    public class RandomSpawnPos : MonoBehaviour
    {
        [Header("PEEKABOO HAHAAHAHAHAHA")]
        [Tooltip("so empty objects = spawn location : ) good luck lol")]
        public Transform[] possibleLocations;

        [Header("WHO ARE YOU BROTHER")]
        [Tooltip("FUCK YOU BROTHER good luc ktho kakakaakak")]
        public string UIID;

        private void Awake()
        {
            if (possibleLocations == null || possibleLocations.Length == 0) return;
            if (string.IsNullOrEmpty(UIID)) return;

            if (GameManager.Instance != null && GameManager.Instance.worldState != null)
            {
                int sI = GameManager.Instance.worldState.GetOrSetSpawnIndex(UIID, possibleLocations.Length);

                transform.position = possibleLocations[sI].position;
                transform.rotation = possibleLocations[sI].rotation;
            }
            else
            {
                int rI = Random.Range(0, possibleLocations.Length);

                transform.position = possibleLocations[rI].position;
                transform.rotation = possibleLocations[rI].rotation;
            }
        }

    }
}