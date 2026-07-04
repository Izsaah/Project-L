using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addmeshcolider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter[] allMeshesInFolder = GetComponentsInChildren<MeshFilter>();

        // 2. Loop through every single shape it found
        foreach (MeshFilter childMesh in allMeshesInFolder)
        {
            // 3. Make sure the object doesn't already have a collider so we don't double up
            if (childMesh.gameObject.GetComponent<Collider>() == null)
            {
                // 4. Add the Mesh Collider directly to the child piece!
                childMesh.gameObject.AddComponent<MeshCollider>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
