using UnityEngine;
using System.Collections;

public class TransmissionData : ScriptableObject {
    public float differential_ratio;
    /** Maximum allowed RPM value. All values over this will be clamped. */
    public float rpm_limit;
    /**
     * Transmission ratios.
     * 0 element is the reverse ratio, 1.. are the forward ratios.
     */
    public float[] ratios;
}
