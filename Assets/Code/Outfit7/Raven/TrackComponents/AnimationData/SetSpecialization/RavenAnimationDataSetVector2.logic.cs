﻿using UnityEngine;

namespace Starlite.Raven {

    public sealed partial class RavenAnimationDataSetVector2 {

        protected override Vector2 GetValueFromParameterCallback(RavenParameter parameter) {
            return parameter.m_ValueVector;
        }
    }
}