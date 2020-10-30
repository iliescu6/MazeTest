using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintObject : MonoBehaviour
{
    public Material material;
    public string colorString;
    public Dictionary<string, Material> color = new Dictionary<string, Material>();
    // Start is called before the first frame update
    void Start()
    {
        color[colorString] = material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
