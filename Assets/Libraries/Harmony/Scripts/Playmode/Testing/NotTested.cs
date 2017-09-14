using System;

namespace Harmony.Testing
{
    /// <summary>
    /// Annotation permettant d'identifier toute classe qui n'a pas à être testée unitairement. 
    /// Une raison doit être fournie.
    /// </summary>
    /// <seealso cref="Reason"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NotTested : Attribute
    {
        public NotTested(Reason reason, params Reason[] otherReasons)
        {
        }
    }

    /// <summary>
    /// Raisons pour lequels une classe ne serait pas testée unitairement. Ce sont les seules raisons
    /// valables.
    /// </summary>
    /// <seealso cref="NotTested"/>
    public enum Reason
    {
        /// <summary>
        /// Classe de configuration. Ne contient que des instructions pour configurer l'application
        /// avant son démarrage.
        /// </summary>
        Configuration,

        /// <summary>
        /// Wrapper autour d'une autre classe. Appelle directement l'autre classe.
        /// </summary>
        Wrapper,

        /// <summary>
        /// Classe utilisant directement la base de données, et qui donc, n'est pas testable unitairement.
        /// </summary>
        Database,

        /// <summary>
        /// Classe implémentant IEvent ou IEventChannel.
        /// </summary>
        EventChannel,

        /// <summary>
        /// Exception. Ne contient aucune logique applicative.
        /// </summary>
        Exception,

        /// <summary>
        /// Fabrique. Contient pas ou peu de logique applicative.
        /// </summary>
        Factory,

        /// <summary>
        /// Utilise des dépendances « sealed » (héritage impossible) ou des dépendances dont les membres (fonctions ou 
        /// propriétés) ne sont pas virtuelles (remplacement impossible). Il est donc impossible de substituer une 
        /// dépendance par une fausse pour faire des tests.
        /// </summary>
        ContainsUnmockable,

        /// <summary>
        /// Outil aidant à mener à bien des tests.
        /// </summary>
        TestingTool
    }
}