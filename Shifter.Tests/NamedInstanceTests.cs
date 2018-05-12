using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shifter.Exceptions;

namespace Shifter.Tests
{
    [TestClass]
    public class NamedInstanceTests
    {
        private ShifterContainer container;

        [TestInitialize]
        public void SetupTest()
        {
            this.container = new ShifterContainer();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNamedInstance_EmptyNameParamIsAdded_ANullArgumentExceptionIsThrown()
        {
            container.AddNamedInstance("", "TestResult");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNamedInstance_NullNameParamIsAdded_ANullArgumentExceptionIsThrown()
        {
            container.AddNamedInstance(null, "TestResult");
        }

        [TestMethod]
        public void AddNamedInstance_AnObjectIsRegisteredUnderAName_TheRegisteredObjectIsReturned()
        {
            container.AddNamedInstance("test", "Hello");

            Assert.AreSame("Hello", container.Resolve<string>("test"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Resolve_NullNameOfInstanceIsEntered_AnExpectionIsThrown()
        {
            container.Resolve((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Resolve_EmptyStringForNameOfInstanceIsEntered_AnExpectionIsThrown()
        {
            container.Resolve("");
        }

        [TestMethod]
        [ExpectedException(typeof(TypeResolvingFailedException))]
        public void Resolve_NameOfInstanceDoesNotExist_AnExpectionIsThrown()
        {
            container.Resolve("wrong");
        }

        [TestMethod]
        public void Resolve_NameOfInstanceIsEnteredThatDoesNotExist_AnExpectionIsThrown()
        {
            container.AddNamedInstance("test", "Hello");
            var resolve = container.Resolve("test");

            Assert.AreSame("Hello", resolve);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveGeneric_NullNameOfInstanceIsEntered_AnExpectionIsThrown()
        {
            container.Resolve<string>((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveGeneric_EmptyStringForNameOfInstanceIsEntered_AnExpectionIsThrown()
        {
            container.Resolve<string>("");
        }

        [TestMethod]
        [ExpectedException(typeof(TypeResolvingFailedException))]
        public void ResolveGeneric_NameOfInstanceDoesNotExist_AnExpectionIsThrown()
        {
            container.Resolve<string>("wrong");
        }

        [TestMethod]
        public void ResolveGeneric_NameOfInstanceIsEnteredThatDoesNotExist_AnExpectionIsThrown()
        {
            container.AddNamedInstance("test", "Hello");
            var resolve = container.Resolve<string>("test");

            Assert.AreSame("Hello", resolve);
        }
    }
}
