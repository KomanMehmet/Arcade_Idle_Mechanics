using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ObjectControll
{
    public class ObjectSizeLibrary : MonoBehaviour
    {
        private static Vector3 _initialSlotPosition;

        public static (Vector3, int, int) ObjectSizeCalculation(Transform calculatedObject, Transform mainObject)
        {
            int rowCount = (int)((mainObject.localScale.z / (calculatedObject.localScale.z + 0.1f)) * (calculatedObject.localScale.z / calculatedObject.localScale.x));
            int columnCount = (int)((mainObject.localScale.x / (calculatedObject.localScale.x + 0.3f)) * (calculatedObject.localScale.x / calculatedObject.localScale.z));

            _initialSlotPosition = mainObject.position + new Vector3(-((mainObject.localScale.x / 2 - (calculatedObject.localScale.x / 2)) - (mainObject.localScale.x - columnCount * calculatedObject.localScale.x) / 2),
                (mainObject.localScale.y / 2 + calculatedObject.localScale.y / 2),
                (mainObject.localScale.z / 2 - (calculatedObject.localScale.z / 2)) - (mainObject.localScale.z - rowCount * calculatedObject.localScale.z) / 2);

            return (_initialSlotPosition, rowCount, columnCount);
        }
    }
}


