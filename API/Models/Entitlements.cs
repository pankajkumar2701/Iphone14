using System;

namespace Iphone14
{
    /// <summary> 
    /// Represents different entitlements for operations.
    /// </summary>

    public enum Entitlements
    {
        /// <summary> 
        /// Allows reading operations.
        /// </summary>

        Read = 1,
        /// <summary> 
        /// Allows creating operations.
        /// </summary>

        Create = 2,
        /// <summary> 
        /// Allows updating operations.
        /// </summary>

        Update = 3,
        /// <summary> 
        /// Allows deleting operations.
        /// </summary>

        Delete = 4
    }
}