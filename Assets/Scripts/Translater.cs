using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translater : MonoBehaviour
{
    public Material eng;
    public Material rus;
    
    void Start()
    {
        if (DataManager.Instance != null)
        {

            if (DataManager.Instance.eng)
            {
                gameObject.GetComponent<Material>().CopyPropertiesFromMaterial(eng);
            }
            else
            {
                gameObject.GetComponent<Material>().CopyPropertiesFromMaterial(rus);
            }
        }
    }
}
