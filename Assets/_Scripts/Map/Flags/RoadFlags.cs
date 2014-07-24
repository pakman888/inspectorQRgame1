using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class RoadFlags : MonoBehaviour {
	public static int SIGN_COUNT_OFFSET = 131;
	public static int RDF_NOISE_MASK_L = 0x00000003;
	public static int RDF_NOISE_100_L = 0x00000000;
	public static int RDF_NOISE_50_L = 0x00000001;
	public static int RDF_NOISE_0_L	= 0x00000002;
	
	public static int RDF_NOISE_MASK_R = 0x0000000C;
	public static int RDF_NOISE_100_R = 0x00000000;
	public static int RDF_NOISE_50_R = 0x00000004;
	public static int RDF_NOISE_0_R = 0x00000008;
	
	public static int RDF_TRANS_MASK_L = 0x00000030;
	public static int RDF_TRANS_16_L = 0x00000000;
	public static int RDF_TRANS_8_L =0x00000010;
	public static int RDF_TRANS_4_L = 0x00000020;
	
	public static int RDF_TRANS_MASK_R = 0x000000C0;
	public static int RDF_TRANS_16_R = 0x00000000;
	public static int RDF_TRANS_8_R	= 0x00000040;
	public static int RDF_TRANS_4_R	= 0x00000080;
	
	public static int RDF_SDWALK_MASK_L = 0x00000300;
	public static int RDF_SDWALK_8_L = 0x00000000;
	public static int RDF_SDWALK_12_L = 0x00000100;
	public static int RDF_SDWALK_2_L = 0x00000200;
	public static int RDF_SDWALK_0_L = 0x00000300;
	
	public static int RDF_SDWALK_MASK_R	= 0x00000C00;
	public static int RDF_SDWALK_8_R = 0x00000000;
	public static int RDF_SDWALK_12_R = 0x00000400;
	public static int RDF_SDWALK_2_R = 0x00000800;
	public static int RDF_SDWALK_0_R = 0x00000C00;

	// reuse flags for terrain only quad length
	public static int RDF_TO_QUAD_MASK = 0x00000C00;
	public static int RDF_TO_QUAD_4 = 0x00000000;
	public static int RDF_TO_QUAD_16 = 0x00000400;
	public static int RDF_TO_QUAD_12 = 0x00000800;
	
	public static int RDF_STATE_BORDER = 0x00001000;
	public static int RDF_NO_RANDOM_SIGNS = 0x00002000;
	public static int RDF_RANDOM_CRACKS = 0x00004000;
	
	public static int RDF_TER_ONLY = 0x00010000;
	public static int RDF_INVERT_RAIL_R = 0x00020000;
	public static int RDF_INVERT_RAIL_L = 0x00040000;
	public static int RDF_CITY_ROAD = 0x00080000;
	public static int RDF_SHIFT_MODEL_R = 0x00100000;
	public static int RDF_SHIFT_MODEL_L = 0x00200000;
	public static int RDF_NO_TRAFFIC = 0x00400000;
	public static int RDF_INSIDE_CITY = 0x00800000;
	public static int RDF_HALF_STEP = 0x01000000;
	public static int RDF_NO_IN_UI_MAP = 0x02000000;
	public static int RDF_NO_SPEED_SIGNS = 0x04000000;
	public static int RDF_NO_BOUND_COLL = 0x08000000;
	
	
	public static float SHOULDER_SIZE = 1.0f;
	public static float SIDEWALK_KERB_SIZE = 1.0f;
    public static float SIDEWALK_KERB_SIZE_H = 0.5f;
    public static float SIDEWALK_HEIGHT = 0.2f;
	
	public static float SEG_LENGTH1 = 5.0f;
	public static float SEG_LENGTH1_INV = (1.0f/SEG_LENGTH1);
	public static float SEG_LENGTH1_HALF = (0.5f*SEG_LENGTH1);
	
	public static float SEG_LENGTH2 = 15.0f;
	public static float SEG_LENGTH2_INV = (1.0f/SEG_LENGTH2);
	public static float SEG_LENGTH2_HALF = (0.5f*SEG_LENGTH2);
	public static float ROAD_MODEL_DIST = 40.0f;
	public static float ROAD_MODEL_DIST_HALF = (ROAD_MODEL_DIST * 0.5f);
}
