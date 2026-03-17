using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    

    public Camera cam;
    public LayerMask machineLayer;
    public MachineInfoUI infoUI;
    private GameObject selectedObject;

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

<<<<<<< Updated upstream
=======
        if (Input.GetKeyDown("l"))
        {
            canMove ^= true;
        }

        //David UI
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, machineLayer))
            {
                GameObject clickedObject = hit.collider.gameObject;

                // Alte Auswahl zurücksetzen
                if (selectedObject != null)
                {
                    SetYellow(selectedObject, false);
                }

                selectedObject = clickedObject;
                SetYellow(selectedObject, true);

                MachineData data = selectedObject.GetComponent<MachineData>();
                if (data != null)
                {
                    infoUI.ShowMachineInfo(data);
                }
            }
            else
            {
                // Nichts getroffen
                if (selectedObject != null)
                {
                    SetYellow(selectedObject, false);
                    selectedObject = null;
                }

                infoUI.HidePanel();
            }

            void SetYellow(GameObject obj, bool active)
            {
                Renderer rend = obj.GetComponent<Renderer>();
                if (rend != null)
                {
                    if (active)
                        rend.material.color = Color.yellow;
                    else
                        rend.material.color = Color.white;
                }
            }
        }
>>>>>>> Stashed changes
    }

        
}