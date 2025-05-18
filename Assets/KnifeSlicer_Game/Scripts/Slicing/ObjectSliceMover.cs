using System.Collections.Generic;
using UnityEngine;
using KnifeSlicer.UnitySpriteCutter.Cutters;
using KnifeSlicer.Core;
using KnifeSlicer.Core.Events;
using KnifeSlicer.GameManagement;

namespace KnifeSlicer.Slicing
{
    /// <summary>
    /// Listens for new slices and shifts all stored slices by slideDistance on +X.
    /// </summary>
    public class ObjectSliceMover : BaseBehaviour
    {
        [Tooltip("World units to shift each slice along +X whenever a new one is cut.")]
        public float slideDistance = 1f;

        [SerializeField]
        private List<Transform> _slicedObjects = new List<Transform>();

        [Topic(GameEventIds.ON_KNIFE_CUT)]
        public void HandleObjectCut(object sender, Transform slice)
        {
            _slicedObjects.Add(slice);
            ShiftAllSlices();
        }

        private void ShiftAllSlices()
        {
            foreach (var tr in _slicedObjects)
            {
                tr.position += Vector3.right * slideDistance;
            }
        }
    }
}
