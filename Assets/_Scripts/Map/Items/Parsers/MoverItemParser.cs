using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MoverItemParser : MonoBehaviour {
    public static MoverItem Parse( BinaryReader reader) {
        //kdop write 40 bytes
        reader.ReadBytes(40);
        var flags = reader.ReadInt32();
        var distType = reader.ReadByte();

        var modelId = reader.ReadUInt16();
        var speed = reader.ReadSingle();
        var endDelay = reader.ReadSingle();

        var lengthsCount = (int)reader.ReadUInt32();
        List<float> lengthsList = new List<float>();        
        for (int i = 0; i < lengthsCount; ++i) {
            lengthsList.Add(reader.ReadSingle());
        }

        var nodesCount = (int)reader.ReadUInt32();
        List<int> nodesList = new List<int>();        
        for (int i = 0; i < nodesCount; ++i) {
            nodesList.Add(reader.ReadInt32());
        }

        MoverItem ret = new MoverItem {
			flags = flags,
			distType = distType,
            modelId = modelId,
            speed = speed,
            endDelay = endDelay,
            lengthsCount = lengthsCount,
            lengthsList = lengthsList,
            nodeIndices = nodesList
        };

        return ret;
    }

}

