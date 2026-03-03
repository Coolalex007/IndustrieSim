using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public bool canMove;

    public GameObject clickedObject;

    public RayCastHighlight Highlighting;

    private GameObject prevObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            //Raycaaast juhuuu
            Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(clickRay, out RaycastHit hitInfo))
            {
                //Debug Log zum auslesen
                Debug.Log("Hit coordinates: " + hitInfo.point);
            
                prevObject = clickedObject;
                Highlighting.StopHighlight(prevObject);

                clickedObject = hitInfo.collider.gameObject;
                Highlighting.Highlight(clickedObject, 1);

                
                
            }
            else
            {
                Highlighting.StopHighlight(clickedObject);
                prevObject = clickedObject;
                clickedObject = null;
            }
        }

        if (Input.GetKeyDown("l"))
        {
            canMove ^= true;
        }
    }


}
