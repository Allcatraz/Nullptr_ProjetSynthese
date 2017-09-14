using System;
using Harmony.Testing;

namespace Harmony.Injection
{
    /// <summary>
    /// Lancée lorsqu'un dépendance nécessitant un wrapper a été bien été utilisé, mais c'est un type concret et non pas un type abstrait 
    /// qui fut demandé.
    /// </summary>
    /// <remarks>
    /// <para>
    /// La raison derrière cette exception est simple : pour isoler le code le plus possible de la plateforme utilisée (ici Unity),
    /// la logique applicative doit dépendre d'abstractions et non pas d'implémentations.
    /// </para>
    /// <para>
    /// Par exemple, considérez le code suivant :
    /// </para>
    /// <code>
    /// public delegate void OnPauseHandler();
    /// 
    /// public class GameInput : UnityScript
    /// {
    ///     public virtual event OnPauseHandler OnPause;
    /// 
    ///     private UnityKeyboardInput input;
    /// 
    ///     public void Inject([ApplicationScope] UnityKeyboardInput input)
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
    /// <para>
    /// Ce script utilise la classe <i>UnityKeyboardInput</i>. Cette dernière sert spécifiquement pour la plateforme Unity. Pourtant, 
    /// la logique applicative devrait être indépendante le plus possible de la plateforme utilisée. C'est donc l'abstraction, soit 
    /// <i>IKeyboardInput</i>, qui devrait être demandée.
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
    public class WrongWrapperUsedException : DependencyInjectionException
    {
        private const string MessageTemplate = "Asked for a dependency of type \"{0}\" for component \"{1}\" of GameObject " +
                                               "named \"{2}\" in scope \"{3}\". You must ask for a \"{4}\", not for a \"{0}\".";

        public WrongWrapperUsedException(UnityScript owner, Type dependencyType, Scope scope, Type wrapperType)
            : this(owner, dependencyType, scope, wrapperType, null)
        {
        }

        public WrongWrapperUsedException(UnityScript owner, Type dependencyType, Scope scope, Type wrapperType, Exception innerException)
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