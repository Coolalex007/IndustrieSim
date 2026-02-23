using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                
            }
        }

    }


}
