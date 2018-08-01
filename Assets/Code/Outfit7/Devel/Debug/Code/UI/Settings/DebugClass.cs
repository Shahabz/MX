﻿//
//   Copyright (c) 2016 Outfit7. All rights reserved.
//

using System;

namespace Outfit7.Devel.O7Debug {
    
#if STRIP_DBG_SETTINGS
    [System.Diagnostics.ConditionalAttribute("FALSE")]
#endif
    [AttributeUsage(AttributeTargets.Class)]
    public class DebugClass : Attribute {
        public string TabName;

        public DebugClass(string tabName) {
            TabName = tabName;
        }
    }

}
