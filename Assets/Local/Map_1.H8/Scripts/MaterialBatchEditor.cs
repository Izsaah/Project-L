using UnityEngine;
using UnityEditor;

public class MaterialBatchEditor : MonoBehaviour
{
    [MenuItem("Tools/Remove Smoothness and Metallic")]
    static void RemoveShinyAndMetal()
    {
        // Loops through all objects you currently have selected
        foreach (GameObject obj in Selection.gameObjects)
        {
            // Gets all renderers, including children
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                foreach (Material m in r.sharedMaterials)
                {
                    // Removes Smoothness (shininess)
                    if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", 0f);
                    if (m.HasProperty("_Glossiness")) m.SetFloat("_Glossiness", 0f);
                    
                    // Removes Metallic (metalness)
                    if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", 0f);
                }
            }
        }
        Debug.Log("Smoothness AND Metallic nuked from selected objects!");
    }
}