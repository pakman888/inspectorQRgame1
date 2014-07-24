using UnityEngine;
using System.Collections;
 
public class ChassisData : ScriptableObject {
		
        /** The air resistance and the wheel rolling resistance force constant */
        public float cdrag;        

        public float ccr;

		/** The frontal area of the vehicle for the resistance force computing */
        public float frontal_area;

        public float cfm;
    
        public float erp;

        public float uplift;
}
