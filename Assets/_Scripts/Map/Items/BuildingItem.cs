using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BuildingItem : Item {
    public const int BYTE_CORE_SIZE = 89; //Core is the section before the idxs information, and the variant information.
    public int scheme;
    public List<int> nodes;
    public float length;
    public int firstBuilding;
    public int firstLook;
    public int randomSeed;
    public float scale;
    public List<int> buildingIndexDecomposed; // not sure wat these are (see typedef below) 
    public List<int> buildingIndexComposed;
    public List<int> variantIdxs; // same
	public List<Hookup> hookups; //Mostly tree leaves?

    public static int GetModelIndexFromComposedIndex(int index) {
        return index & 0xFF; // this is straight from the busdriver code. feel like it won't do anything to that index number, unless it is just returning the last byte.
    }
    public static int GetLookIndexFromComposedIndex(int index, int partGroup) {
        //assert that prt_grp is > 3
        //prt group is the number of parts in the building.
        //warning the cast could break things.

        return (index >> (int) (partGroup + 1) * 8) & 0xFF; //this is also from the busdriver code.
    }
}
/*
typedef array_dyn_t<unsigned>	unsigned_array_t;
		unsigned_array_t	building_idxs;		// building model/look indices
		unsigned_array_t	variant_idxs;		// building variant indices
		float			scale;			// global item scale to fit buildings exactly on the curve
*/