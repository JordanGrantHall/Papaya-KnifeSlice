﻿using System;
using UnityEngine;

namespace KnifeSlicer.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ConstDropdownAttribute : PropertyAttribute
    {
        public Type TargetType { get; private set; }

        public ConstDropdownAttribute(Type targetType)
        {
            if (!targetType.IsAbstract || !targetType.IsSealed)
            {
                throw new ArgumentException($"{targetType.Name} is not a static class.");
            }

            TargetType = targetType;
        }
    }
}