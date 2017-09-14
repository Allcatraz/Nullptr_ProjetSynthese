using System;

namespace Harmony.Util
{
    /// <summary>
    /// Indique que la fonction annotée est appellée par réflexion en dehors du code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CalledOutsideOfCode : Attribute
    {
    }
}