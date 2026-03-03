using UnityEngine;




public class RayCastHighlight : MonoBehaviour
{
    public Material mat1;
    public Material highlightMat;
    
    public void Highlight(GameObject go, int mode)
    {
        if (go == null)
        {
            return;
        }
        Renderer rend = go.GetComponent<Renderer>();

        switch (mode)
        {
            case 1:
                rend.material = highlightMat;
                break;
            default:
                rend.material = mat1;
                break;
        }
    }

    public void StopHighlight(GameObject go)
    {
        Highlight(go, 0);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
