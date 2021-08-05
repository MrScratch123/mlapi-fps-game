using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rendererToggle : MonoBehaviour
{
    [SerializeField] 
    GameObject parent;
    Renderer parentRenderer;
    Renderer myRenderer;
    void Start()
    {
        parentRenderer = parent.GetComponent<Renderer>();
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        if (parentRenderer.enabled) 
        {
            myRenderer.enabled = true;
        }
        else
        {
            myRenderer.enabled = false;
        }
    }
}
