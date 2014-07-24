using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class BusStopItemParser : MonoBehaviour {
    public static BusStopItem Parse(BinaryReader reader) {
        //KDOP.write fatness. 
        reader.ReadBytes(40);
        var flags = reader.ReadInt32();
        byte distType = reader.ReadByte();
        var modelType = reader.ReadUInt16();
        var stopUId = reader.ReadUInt32();
        var indexVal = reader.ReadInt32();
        BusStopItem ret = new BusStopItem {
            flags = flags,
            distType = (byte) distType,
            modelType = modelType,
            stopUID = (int) stopUId,
            index = indexVal
        };

        return ret;
    }
}
