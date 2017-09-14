using UnityEngine;
using System.Collections.Generic;
using Harmony;
using Harmony.Injection;
using Harmony.Testing;
using Harmony.Unity;
using Harmony.XInput;
using UnityEngine.UI;

namespace ProjetSynthese
{
    /// <summary>
    /// Représente un contexte d'injection pour l'application.
    /// </summary>
    /// <inheritdoc cref="IInjectionContext"/>
    [NotTested(Reason.Configuration)]
    public class ApplicationInjectionContext : IInjectionContext
    {
        private readonly IList<WrapperFactory> dependencyWrappers;
        private readonly IList<object> staticComponents;

        public ApplicationInjectionContext()
        {
            dependencyWrappers = new List<WrapperFactory>();
            staticComponents = new List<object>();

            //*******************************************************
            //*************************UNITY*************************
            //*******************************************************

            //Concrete types
            AddDependencyWrapper<AudioSource, IAudioSource>(dependency => new UnityAudioSource(dependency as AudioSource));
            AddDependencyWrapper<Button, IButton>(dependency => new UnityButton(dependency as Button));
            AddDependencyWrapper<Camera, ICamera>(dependency => new UnityCamera(dependency as Camera));
            AddDependencyWrapper<Canvas, ICanvas>(dependency => new UnityCanvas(dependency as Canvas));
            AddDependencyWrapper<InputField, ITextInput>(dependency => new UnityTextInput(dependency as InputField));
            AddDependencyWrapper<LineRenderer, ILineRenderer>(dependency => new UnityLineRenderer(dependency as LineRenderer));
            AddDependencyWrapper<PolygonCollider2D, IPolygonCollider2D>(dependency => new UnityPolygonCollider2D(dependency as PolygonCollider2D));
            AddDependencyWrapper<Rigidbody2D, IRigidbody2D>(dependency => new UnityRigidbody2D(dependency as Rigidbody2D));
            AddDependencyWrapper<SpriteRenderer, ISpriteRenderer>(dependency => new UnitySpriteRenderer(dependency as SpriteRenderer));
            AddDependencyWrapper<Text, ITextView>(dependency => new UnityTextView(dependency as Text));
            AddDependencyWrapper<Transform, ITransform>(dependency => new UnityTransform(dependency as Transform));

            //Abstract types
            AddDependencyWrapper<Collider2D, ICollider2D>(dependency => new UnityCollider2D(dependency as Collider2D));
            AddDependencyWrapper<Renderer, IRenderer>(dependency => new UnityRenderer(dependency as Renderer));
            AddDependencyWrapper<Selectable, ISelectable>(dependency => new UnitySelectable(dependency as Selectable));

            //Static types
            AddStaticDependencyWrapper<UnityApplication>();
            AddStaticDependencyWrapper<UnityActivityMenuStack>();
            AddStaticDependencyWrapper<UnityCoroutineExecutor>();
            AddStaticDependencyWrapper<UnityHierachy>();
            AddStaticDependencyWrapper<UnityKeyboardInput>();
            AddStaticDependencyWrapper<XInputGamepadInput>();
            AddStaticDependencyWrapper<UnityPhysics2D>();
            AddStaticDependencyWrapper<UnityPrefabFactory>();
            AddStaticDependencyWrapper<UnityRandom>();
            AddStaticDependencyWrapper<UnityTime>();
            AddStaticDependencyWrapper<UnityMenuState>();

            //******************************************************
            //************************XINPUT************************
            //******************************************************

            //Static types
            AddStaticDependencyWrapper<XInputGamepadInput>();
        }

        public IEnumerable<WrapperFactory> DependencyWrappers
        {
            get { return dependencyWrappers; }
        }

        public IEnumerable<object> StaticComponents
        {
            get { return staticComponents; }
        }

        public IList<GameObject> FindGameObjectsWithTag(string tag)
        {
            return new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));
        }

        private void AddDependencyWrapper<WrappedType, WrapperType>(WrapperFactoryCallback wrapperFactoryCallback)
        {
            dependencyWrappers.Add(new WrapperFactory(typeof(WrappedType), typeof(WrapperType), wrapperFactoryCallback));
        }

        private void AddStaticDependencyWrapper<StaticWrapperType>() where StaticWrapperType : new()
        {
            staticComponents.Add(new StaticWrapperType());
        }
    }
}