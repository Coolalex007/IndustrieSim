using UnityEngine;

public class Crankrotate : MonoBehaviour
{
    public GameObject Crank;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = Crank.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddTorque(Vector3.forward * 10f);
    }
}
