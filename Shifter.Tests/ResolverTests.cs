using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shifter.Exceptions;
using Shifter.Tests.Interfaces;
using Shifter.Tests.MockClasses;

namespace Shifter.Tests
{
    [TestClass]
    public class ResolverTests
    {
        [TestMethod]
        public void ShifterAddInstance_AddInstanceReturnsShiftContainer_TheShiftContainerIsReturned()
        {
            var shifter = new ShifterContainer();
            IShifterContainer container = shifter.AddInstance(new object());

            Assert.AreSame(shifter, container);

            container = shifter.AddInstance<IResolveTest>(new ImplementsResolveTest());

            Assert.AreSame(shifter, container);

            container = shifter.AddInstance(typeof(ResolveTestClass), new ResolveTestClass());
            
            Assert.AreSame(shifter, container);

            container = shifter.AddType(typeof (IComparable), typeof (String));

            Assert.AreSame(shifter, container);

            container = shifter.AddType<IResolveTest2, ImplementsResolveTest2>();

            Assert.AreSame(shifter, container);
        }

        [TestMethod]
        public void ShifterAddType_AdditionOfTypeWithDefaultConstructor_TheTypeIsResolved()
        {
            var shifter = new ShifterContainer();
            var resolvedClass = shifter
                                    .AddType(typeof(IResolveTest), typeof(DefaultConstructorResolveTest))
                                    .Resolve<IResolveTest>();

            Assert.IsNotNull(resolvedClass);
            Assert.IsInstanceOfType(resolvedClass, typeof(DefaultConstructorResolveTest));
        }

        [TestMethod]
        public void ShifterAddType_AdditionOfTypeWithConstructorWithParameters_TheTypeIsResolved()
        {
            const string injectedValue = "Tron";

            var shifter = new ShifterContainer();
            var resolvedClassWithOneParam = (ImplementsResolveTestWithParameters)shifter.AddInstance(injectedValue)
                                                                                        .AddType(typeof(IResolveTest), typeof(ImplementsResolveTestWithParameters))
                                                                                        .Resolve<IResolveTest>();

            Assert.IsNotNull(resolvedClassWithOneParam);
            Assert.IsInstanceOfType(resolvedClassWithOneParam, typeof(ImplementsResolveTestWithParameters));
            Assert.AreEqual(injectedValue, resolvedClassWithOneParam.Param1);
            Assert.AreEqual(injectedValue, resolvedClassWithOneParam.FieldToInject);

            var resolvedClass = shifter.AddInstance(typeof(ImplementsResolveTestWithParameters), resolvedClassWithOneParam)
                                       .AddType<ImplementsResolveTestWith2Parameters, ImplementsResolveTestWith2Parameters>()
                                       .Resolve<ImplementsResolveTestWith2Parameters>();

            Assert.IsNotNull(resolvedClass);
            Assert.IsInstanceOfType(resolvedClass, typeof(ImplementsResolveTestWith2Parameters));
            Assert.AreEqual(injectedValue, resolvedClass.Param1);
            Assert.AreSame(resolvedClassWithOneParam, resolvedClass.Param2);
        }

        [TestMethod]
        public void ShifterResolve_FieldIsInjected_TheFieldIsResolved()
        {
            const string injectedValue = "Tron";

            var shifter = new ShifterContainer();
            var resolvedClassWithOneParam = (ImplementsResolveTestWithParameters)shifter.AddInstance(injectedValue)
                                                                                        .AddType(typeof(IResolveTest), typeof(ImplementsResolveTestWithParameters))
                                                                                        .Resolve<IResolveTest>();

            Assert.AreEqual(injectedValue, resolvedClassWithOneParam.FieldToInject);
        }

        [TestMethod]
        public void ShifterResolve_MethodIsInjected_TheMethodIsResolved()
        {
            const string injectedValue = "Tron";

            var shifter = new ShifterContainer();
            var resolvedClassWithOneParam = (ImplementsResolveTestWithParameters)shifter.AddInstance(injectedValue)
                                                                                        .AddType(typeof(IResolveTest), typeof(ImplementsResolveTestWithParameters))
                                                                                        .Resolve<IResolveTest>();

            Assert.AreEqual(injectedValue, resolvedClassWithOneParam.MethodFieldInjected);
        }

        [TestMethod]
        public void ShifterResolve_PropertyIsInjected_ThePropertyIsResolved()
        {
            const string injectedValue = "Tron";

            var shifter = new ShifterContainer();
            var resolvedClassWithOneParam = (ImplementsResolveTestWithParameters)shifter.AddInstance(injectedValue)
                                                                                        .AddType(typeof(IResolveTest), typeof(ImplementsResolveTestWithParameters))
                                                                                        .Resolve<IResolveTest>();

            Assert.AreEqual(injectedValue, resolvedClassWithOneParam.PropertyInjected);
        }

        [TestMethod]
        [ExpectedException(typeof(TypeResolvingFailedException))]
        public void ShifterAddType_AdditionOfInterfaceAsTypeToBeResolved_AnExceptionIsThrown()
        {
            var shifter = new ShifterContainer();
            var resolvedClass = shifter
                                    .AddType(typeof(IResolveTest), typeof(IResolveTest))
                                    .Resolve<ResolveTestClass>();
        }

        [TestMethod]
        [ExpectedException(typeof(TypeResolvingFailedException))]
        public void ShifterAddType_AdditionOfAbstractClassAsTypeToBeResolved_AnExceptionIsThrown()
        {
            var shifter = new ShifterContainer();
            var resolvedClass = shifter
                                    .AddType(typeof(IResolveTest), typeof(AbstractClass))
                                    .Resolve<ResolveTestClass>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShifterAddType_FirstParameterIsNull_AnExceptionIsThrown()
        {
            var shifter = new ShifterContainer();
            shifter.AddType(null, typeof (string));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShifterAddType_SecondParameterIsNull_AnExceptionIsThrown()
        {
            var shifter = new ShifterContainer();
            shifter.AddType(typeof (IShifterContainer), null);
        }

        [TestMethod]
        public void ShifterAddInstance_SameInterfaceAndClassAreAdded_AnExcpetionIsNotThrown()
        {
            Action action = () =>
            {
                var shifter = new ShifterContainer();
                shifter.AddInstance<IResolveTest>(new ImplementsResolveTest());
                shifter.AddInstance(typeof(IResolveTest), new ImplementsResolveTest());
            };

            action.Should().NotThrow<ArgumentException>("because this cannot occur.");
        }

        [TestMethod]
        public void ShifterGenericRegisterType_ARegisteredClassCanBeResolved_TheClassIsResolved()
        {
            var shifter = new ShifterContainer();
            shifter.AddInstance(new ResolveTestClass());
            var resolvedClass = shifter.Resolve(typeof(ResolveTestClass)) as ResolveTestClass;

            Assert.IsNotNull(resolvedClass);
            Assert.IsInstanceOfType(resolvedClass, typeof(ResolveTestClass));
        }

        [TestMethod]
        public void ShifterGenericResolve_ARegisteredClassIsResolved_TheClassIsResolved()
        {
            var shifter = new ShifterContainer();
            shifter.AddInstance(new ResolveTestClass());
            var resolvedClass = shifter.Resolve<ResolveTestClass>();

            Assert.IsNotNull(resolvedClass);
            Assert.IsInstanceOfType(resolvedClass, typeof(ResolveTestClass));
        }

        [TestMethod]
        [ExpectedException(typeof(TypeResolvingFailedException))]
        public void ShifterGenericRegisterType_NoRegisteredClassIsRegistered_AnExceptionIsThrown()
        {
            new ShifterContainer().Resolve<ResolveTestClass>();
        }

        [TestMethod]
        [ExpectedException(typeof(TypeResolvingFailedException))]
        public void ShifterContainerResolve_NoRegisteredClassIsRegistered_ExceptionIsThrown()
        {
            new ShifterContainer().Resolve(typeof(ResolveTestClass));
        }

        [TestMethod]
        public void ShifterResolve_ARegisteredClassIsResolved_TheClassIsResolved()
        {
            var shifter = new ShifterContainer();
            shifter.AddInstance(new ResolveTestClass());
            var resolvedClass = shifter.Resolve(typeof(ResolveTestClass)) as ResolveTestClass;

            Assert.IsNotNull(resolvedClass);
            Assert.IsInstanceOfType(resolvedClass, typeof(ResolveTestClass));
        }

        [TestMethod]
        public void ShifterResolve_ARegisteredInterfaceIsResolved_TheInterfaceIsResolvedToADependedClass()
        {
            var shifter = new ShifterContainer();
            shifter.AddInstance(typeof(IResolveTest), new DefaultConstructorResolveTest());
            var resolvedClass = shifter.Resolve(typeof(IResolveTest)) as DefaultConstructorResolveTest;

            Assert.IsNotNull(resolvedClass);
            Assert.IsInstanceOfType(resolvedClass, typeof(DefaultConstructorResolveTest));
        }

        [TestMethod]
        public void ShifterResolve_ARegisteredClassIsResolved_TheClassIsResolvedToADependedClass()
        {
            var shifter = new ShifterContainer();
            shifter.AddInstance(new RegisterClassBase().GetType(), new RegisterClassDerived());
            var resolvedClass = shifter.Resolve(typeof(RegisterClassBase)) as RegisterClassDerived;

            Assert.IsNotNull(resolvedClass);
            Assert.IsInstanceOfType(resolvedClass, typeof(RegisterClassDerived));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShifterRegister2Params_FirstParamIsNull_AnExceptionIsThrown()
        {
            var shifter = new ShifterContainer();
            shifter.AddInstance(null, new ResolveTestClass());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShifterRegister2Params_SecondParamIsNull_AnExceptionIsThrown()
        {
            var shifter = new ShifterContainer();
            shifter.AddInstance(new RegisterClassBase().GetType(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(NoInheritanceDependencyException))]
        public void ShifterResolve_AInterfaceIsRegisteredWithDependedClassThatDoesImplementIt_AnExceptionIsThrown()
        {
            var shifter = new ShifterContainer();
            shifter.AddInstance(typeof(IResolveTest), new ResolveTestClass());
            shifter.AddInstance(typeof(String), new ResolveTestClass());
        }

        [TestMethod]
        [ExpectedException(typeof(NoInheritanceDependencyException))]
        public void ShifterResolve_AClassIsRegisteredWithDependedClassThatDoesImplementIt_AnExceptionIsThrown()
        {
            var shifter = new ShifterContainer();
            shifter.AddInstance(new RegisterClassBase().GetType(), new ResolveTestClass());
            shifter.AddInstance(typeof(String), new ResolveTestClass());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "classToRegister")]
        public void ShifterContainerRegisterType_NullIsRegistered_ExceptionIsThrown()
        {
            new ShifterContainer().AddInstance(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "type")]
        public void ShifterContainerResolve_NullIsResolved_ExceptionIsThrown()
        {
            new ShifterContainer().Resolve((Type)null);
        }

        [TestMethod]
        public void ShifterContainerResolve_PropertyInjected_ThePropertyIsInjctedWithTheClassProvided()
        {
            const string valueToInject = "hello world";

            var shifter = new ShifterContainer();
            var resolvedClass = shifter
                                    .AddInstance(typeof(string), valueToInject)
                                    .AddInstance(typeof(ImplementsResolveTest), new ImplementsResolveTest())
                                    .Resolve<ImplementsResolveTest>();

            Assert.AreEqual(valueToInject, resolvedClass.InjectedProperty);
        }

        [TestMethod]
        public void WhenAllInstancesOfATypeAreResolvedItShouldReturnAllInstances()
        {
            // Arrange
            var expectedItemsList = new List<string> { "All", "Instances", "Are", "Retrieved" };
            var container = new ShifterContainer();
            container
                .AddInstance(typeof(int), 1)
                .AddInstance(typeof(string), "All")
                .AddInstance(typeof(string), "Instances")
                .AddInstance(typeof(string), "Are")
                .AddInstance(typeof(string), "Retrieved");

            // Act
            var resolveAll = container.ResolveAll(typeof(string));

            // Assert 
            resolveAll.Count().Should().Be(4, "because there are 4 instances registered.");
            resolveAll.Should().Contain(expectedItemsList);
        }

        [TestMethod]
        public void WhenAllInstancesOfATypeAreResolvedGenericallyItShouldReturnAllInstances()
        {
            // Arrange
            var expectedItemsList = new List<string> { "All", "Instances", "Are", "Retrieved" };
            var container = new ShifterContainer();
            container
                .AddInstance(typeof(int), 1)
                .AddInstance(typeof(string), "All")
                .AddInstance(typeof(int), 1)
                .AddInstance(typeof(string), "Instances")
                .AddInstance(typeof(int), 1)
                .AddInstance(typeof(string), "Are")
                .AddInstance(typeof(int), 1)
                .AddInstance(typeof(string), "Retrieved");

            // Act
            var resolveAll = container.ResolveAll<String>();

            // Assert 
            resolveAll.Count().Should().Be(4, "because there are 4 instances registered.");
            resolveAll.Should().Contain(expectedItemsList);
        }
    }
}
