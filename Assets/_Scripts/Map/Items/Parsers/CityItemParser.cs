using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CityItemParser : MonoBehaviour {
    public static CityItem Parse(BinaryReader reader) {
        //Kdop.write
        reader.ReadBytes(40);
        var flags = reader.ReadInt32();
        var distType = reader.ReadByte(); 

        var cityData = reader.ReadUInt64();
        var width = reader.ReadSingle();
        var height = reader.ReadSingle();
        var indexVal = reader.ReadInt32();

        CityItem ret = new CityItem {
            flags = flags,
            distType = distType,
            cityData = cityData,
            width = width,
            height = height,
            index = indexVal
        };
        return ret;
    }
}
