using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectL.Global.Script.interaction
{
    public class RandomSpawnPos : MonoBehaviour
    {
        [Header("PEEKABOO HAHAAHAHAHAHA")]
        [Tooltip("so empty objects = spawn location : ) good luck lol")]
        public Transform[] possibleLocations;

        private void Awake()
        {
            if (possibleLocations == null || possibleLocations.Length == 0) return;

            int rI = Random.Range(0, possibleLocations.Length);

            transform.position = possibleLocations[rI].position;
            transform.rotation = possibleLocations[rI].rotation;
        }

    }
}