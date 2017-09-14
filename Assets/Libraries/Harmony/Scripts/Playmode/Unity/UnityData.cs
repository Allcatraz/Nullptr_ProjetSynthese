using Harmony.Testing;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Extension des ScriptableObject de Unity. Servent principalement à contenir de données de configuration
    /// dans des fichiers « .asset ».
    /// </summary>
    [NotTested(Reason.Wrapper)]
    public abstract class UnityData : ScriptableObject, IData
    {
    }
}