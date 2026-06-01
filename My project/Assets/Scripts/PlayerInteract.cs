using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public bool canMove;
    public GameObject clickedObject;
    public RayCastHighlight Highlighting;

    private GameObject prevObject;
    private MachineDetailsPanel detailsPanel;

    void Start()
    {
        canMove = true;
        detailsPanel = MachineDetailsPanel.GetOrCreate();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(clickRay, out RaycastHit hitInfo))
            {
                prevObject = clickedObject;
                Highlighting.StopHighlight(prevObject);

                clickedObject = hitInfo.collider.gameObject;
                Highlighting.Highlight(clickedObject, 1);

                if (detailsPanel != null)
                {
                    detailsPanel.Show(clickedObject);
                }
            }
            else
            {
                Highlighting.StopHighlight(clickedObject);
                prevObject = clickedObject;
                clickedObject = null;

                if (detailsPanel != null)
                {
                    detailsPanel.Hide();
                }
            }
        }

        if (Input.GetKeyDown("l"))
        {
            canMove ^= true;
        }
    }
}
