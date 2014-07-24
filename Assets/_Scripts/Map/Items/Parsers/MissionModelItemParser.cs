using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class MissionModelItemParser : MonoBehaviour {

    /** The bit used to store additional information in token_t value. */
    const UInt64 TOKEN_MARK =  0x8000000000000000;
    /** The mask of the non-mark bits in the token_t value. */
    const UInt64 TOKEN_MASK =  0x7fffFFFFffffFFFF;
    const int TOKEN_LENGTH = 12;
    const int SET_SIZE = 38;
    static char[] u6_to_char = {
    '\0', '0', '1', '2', '3', '4', '5', '6',
    '7', '8', '9', 'a', 'b', 'c', 'd', 'e',
    'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
    'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
    'v', 'w', 'x', 'y', 'z', '_'
    };

    public static MissionModelItem Parse( BinaryReader reader) {
        //kdop:: write fatness
	    reader.ReadBytes(40);
        var flags = reader.ReadInt32();
        var distType = reader.ReadByte();
        var modelId = reader.ReadInt32();
        var lookId = reader.ReadInt32();
        var index = reader.ReadInt32();
        var rotationVec = new Vector3(
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle()
            );
        var scaleVec = new Vector3(
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle()
            );
        var missionCount = reader.ReadUInt32();
        List<string> missionList = new List<string>();
        for ( int i = 0; i < missionCount; ++i ) {
            var token =  (UInt64) reader.ReadInt64();
            var output = TokenToString(token);
            missionList.Add(output); 
        }
        MissionModelItem ret = new MissionModelItem {
            flags = flags,
            distType = distType,
            modelId = modelId,
            model_look_id = lookId,
            index = index,
            rotation = rotationVec,
            scale = scaleVec,
            missionCount = (int) missionCount,
            missionList = missionList
        };
        return ret;
    }
    static string TokenToString(UInt64 input) {
        StringBuilder builder = new StringBuilder(); 
        UInt64 tmp = input & TOKEN_MASK;
        for (int i = 0; i < (TOKEN_LENGTH + 1); ++i) {
            UInt64 index = tmp % SET_SIZE;
            builder.Append(u6_to_char[index]);
            if (index == 0) {
                break;
            }
            tmp /= SET_SIZE;
        }
        return builder.ToString();
    }
}
