using UnityEngine;
using System.Collections;


public abstract class BaseVehicleData : ScriptableObject {
	    //sstring_t name;
        /*we use "name" from monobehavior*/
    
        public GameObject model;
            
        public GameObject animated_model;
            
        public string look_name;
            
        public string variant_name;

        //TODO[PP]: assuming these are colliders and will be unity colliders */  
		//resource_tie_t<model_coll_u>	collision;
             
        public GameObject rwheel_model;

        public float wheel_mass;

        public Vector3[] rwheel_positions;

        public float wheel_radius;
		
        /*  trailer hook! */
        public Vector3 thook_position;

		//Is this vehicle available in this stage of the game? 
        public float  game_points;

        /* protected:
		        inline		void	invalidate_model	(void);
                virtual		void	load_locators		(void);
				bool		get_locator_position	(float3 *const position, const model_template_u *const instance, const string &locator_name);
				bool		get_locator_placement	(placement_t *const placement, const model_template_u *const instance, const string &locator_name);
        */

}
