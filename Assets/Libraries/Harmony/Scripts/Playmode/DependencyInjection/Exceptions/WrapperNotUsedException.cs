using System;
using Harmony.Testing;

namespace Harmony.Injection
{
    /// <summary>
    /// Lancée lorsqu'une dépendance marquée comme nécessitant un Wrapper n'a pas été demandée avec son Wrapper.
    /// </summary>
    /// <remarks>
    /// <para>
    /// La raison derrière cette exception est simple. Pour isoler le code le plus possible de la plateforme utilisée (ici Unity),
    /// les jeux possèdent souvent des Wrappers, sortes de ponts entre la logique applicative et la plateforme.
    /// </para>
    /// <para>
    /// Pour préserver la séparation entre le jeu et la plateforme, l'injecteur de dépendance vérifie que ce sont bien les wrappers 
    /// qui sont demandés et non pas les objets de la plateforme. De cette façon, il est plus simple de changer de plateforme
    /// en cours de développement.
    /// </para>
    /// <para>
    /// Par exemple, considérez le code suivant :
    /// </para>
    /// <code>
    /// public delegate void OnPauseHandler();
    /// 
    /// public class GameInput : MonoBehaviour
    /// {
    ///     public virtual event OnPauseHandler OnPause;
    /// 
    ///     public void Update()
    ///     {
    ///         if (Input.GetKeyDown(KeyCode.Escape))
    ///         {
    ///             if (OnTogglePause != null) OnTogglePause();
    ///         }
    ///     }
    /// }
    /// </code>
    /// <para>
    /// Ce script utilise la classe statique <i>Input</i> de la plateforme. Pourtant, il existe un Wrapper autour de cette classe 
    /// statique, soit <i>IKeyboardInput</i>. Si l'on veux conserver une bonne séparation entre la logique applicative et la 
    /// plateforme, c'est lui qui devrait donc être demandé.
    /// </para>
    /// <para>
    /// Reprenons l'exemple précédent, modifié en utilisant <i>IKeyboardInput</i> :
    /// </para>
    /// <code>
    /// public delegate void OnPauseHandler();
    /// 
    /// public class GameInput : UnityScript
    /// {
    ///     public virtual event OnPauseHandler OnPause;
    /// 
    ///     private IKeyboardInput input;
    /// 
    ///     public void Inject([ApplicationScope] IKeyboardInput input)
    ///     {
    ///         this.input = input;
    ///     }
    /// 
    ///     public void Awake()
    ///     {
    ///         InjectDependencies();
    ///     }
    /// 
    ///     public void Update()
    ///     {
    ///         if (input.GetKeyDown(KeyCode.Escape))
    ///         {
    ///             if (OnTogglePause != null) OnTogglePause();
    ///         }
    ///     }
    /// }
    /// </code>
    /// </remarks>
    [NotTested(Reason.Exception)]
    public class WrapperNotUsedException : DependencyInjectionException
    {
        private const string MessageTemplate = "Asked for a dependency of type \"{0}\" for component \"{1}\" of GameObject " +
                                               "named \"{2}\" in scope \"{3}\". You should instead ask for a \"{4}\". That way, " +
                                               "your code will be unit testable.";

        public WrapperNotUsedException(UnityScript owner, Type dependencyType, Scope scope, Type wrapperType)
            : this(owner, dependencyType, scope, wrapperType, null)
        {
        }

        public WrapperNotUsedException(UnityScript owner, Type dependencyType, Scope scope, Type wrapperType, Exception innerException)
            : base(String.Format(MessageTemplate,
                                 dependencyType.Name,
                                 owner.GetType().Name,
                                 owner.name,
                                 scope.GetType().Name,
                                 wrapperType.Name),
                   innerException)
        {
        }
    }
}