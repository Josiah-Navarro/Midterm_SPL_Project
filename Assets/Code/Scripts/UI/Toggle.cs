using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectKey
{
    public GameObject targetObject;
    public KeyCode toggleKey;
}

public class Toggle : MonoBehaviour
{
    public List<ObjectKey> objectKey = new List<ObjectKey>();

    void Update()
    {
        foreach (var pair in objectKey)
        {
            if (Input.GetKeyDown(pair.toggleKey) && pair.targetObject != null)
            {
                pair.targetObject.SetActive(!pair.targetObject.activeSelf);
            }
        }
    }
}
