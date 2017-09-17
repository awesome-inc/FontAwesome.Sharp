using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontAwesome.Sharp
{
    /// <summary>
    /// Flip flags
    /// </summary>
    [Flags]
    public enum IconFlip : byte
    {
        /// <summary>
        /// Flip off
        /// </summary>
        None = 0x0,
        /// <summary>
        /// Horizontal flip
        /// </summary>
        Horizontal = 0x1,
        /// <summary>
        /// Vertical flip
        /// </summary>
        Vertical = 0x2,
        /// <summary>
        /// Full flip - same as Rotate(180)
        /// </summary>
        Full = 0x4
    }
}
