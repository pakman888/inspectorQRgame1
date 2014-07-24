using UnityEngine;
using System.Collections;

/**
* @brief Pacejka data.
*
* The Pacejka constants are stored in the array of the size of 7.
* Peak value(D) constants (usually noted as A1 and A2) are stored in a[0] and a[1] elements.
* Stiffness factor(B) constants - a[2] and a[3].
* Shape factor (C) constant - a[4].
* Curvature factor (E) - a[5] and a[6].
*/
//vehicle_pacejk_u
[System.Serializable]
public class VehiclePacejka  {
	public float[]	a;
}
