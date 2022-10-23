using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

 
public class LowLevelHelper {
    public static T As<T>(ref byte[] bytes) where T : struct
    {
        return UnsafeUtility.As<byte, T>(ref bytes[0]);
    }
}