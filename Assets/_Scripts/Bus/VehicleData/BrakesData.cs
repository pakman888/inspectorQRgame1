using UnityEngine;
using System.Collections;

public class BrakesData : ScriptableObject  {
    /** Maximum torque that is generated during braking */
    public float torque;
    /** Brakes balance (0 = full rear ... 1 = full front) */
    public float balance;
}
