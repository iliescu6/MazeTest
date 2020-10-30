using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private List<string> colors = new List<string>();
    public List<string> Colors
    {
        get { return colors; }
        set { colors = value; }
    }
    Renderer renderer;

    [SerializeField]
    Colors colorAsset;

    private void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            PaintObject po = other.gameObject.GetComponent<PaintObject>();
            if (po != null)
            {
                if (!colors.Contains(po.colorString))
                {
                    colors.Add(po.colorString);
                }
            }
        }
    }

    void AddColor(string newColor)
    {

    }
}
