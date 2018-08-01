//
//   Copyright (c) 2015 Outfit7. All rights reserved.
//

#if NETFX_CORE
using System;

namespace System.Reflection {
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Delegate,
                    AllowMultiple = true,
                    Inherited = false)]
    public sealed class ObfuscationAttribute: Attribute {
        private bool m_strip = true;
        private bool m_exclude = true;
        private bool m_applyToMembers = true;
        private string m_feature = "all";

        public ObfuscationAttribute() {
        }

        public bool StripAfterObfuscation {
            get {
                return m_strip;
            }
            set {
                m_strip = value;
            }
        }

        public bool Exclude {
            get {
                return m_exclude;
            }
            set {
                m_exclude = value;
            }
        }

        public bool ApplyToMembers {
            get {
                return m_applyToMembers;
            }
            set {
                m_applyToMembers = value;
            }
        }

        public string Feature {
            get {
                return m_feature;
            }
            set {
                m_feature = value;
            }
        }
    }
}
#endif
