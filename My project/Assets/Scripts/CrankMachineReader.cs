using UnityEngine;

public class CrankMachineReader : MonoBehaviour
{
    private IndustrialMachineData IMD;
    private HingeJoint j;
    private JointMotor m;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IMD = GetComponent<IndustrialMachineData>();
        j = GetComponent<HingeJoint>();
        m = j.motor;
    }

    // Update is called once per frame
    void Update()
    {
        IMD = GetComponent<IndustrialMachineData>();
        j = GetComponent<HingeJoint>();
        m = j.motor;
        if (IMD.status != "In Betrieb")
        {
            m.targetVelocity = 0;
        }

        else
        {
            m.targetVelocity = 60;
        }

        j.motor = m;
    }
}
