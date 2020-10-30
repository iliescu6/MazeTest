using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Colors :MonoBehaviour
{
    //Dictionary<string, Material> materials = new Dictionary<string, Material>();  
    public List<CustomColor> colors = new List<CustomColor>();
    private void Start()
    {

    }
}

[System.Serializable]
public class CustomColor
{
    [SerializeField]
    string color;
    [SerializeField]
    Material materialColor;
}
