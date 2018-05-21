using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shifter.Tests
{
    [TestClass]
    public class ShifterContainerTests
    {
        [TestMethod]
        public void WhenCallingDefaultItShouldReturnADefaultShifterContainer()
        {
            // Act
            var defaultContainer = ShifterContainer.Default;

            // Assert    
            defaultContainer.Should().NotBeNull("because null is not allowed as a default container.");
        }

        [TestMethod]
        public void WhenCallingDefaultItShouldReturnTheSameDefaultShifterContainer()
        {
            // Act
            var defaultContainer = ShifterContainer.Default;
            var defaultContainer2 = ShifterContainer.Default;

            // Assert    
            defaultContainer.Should().BeSameAs(defaultContainer2, "because only one single instance is returned as the default container.");
        }

        [TestMethod]
        public void WhenCallingDefaultItShouldReturnAnInstanceOfIShifterContainer()
        {
            // Act
            var defaultContainer = ShifterContainer.Default;

            // Assert    
            defaultContainer.Should().BeAssignableTo<IShifterContainer>("because default is a IShifterContainer.");
        }

        [TestMethod]
        public void WhenUnregisteringATypeItShouldUnregisterAllInstancesOfThatType()
        {
            // Arrange
            var container = new ShifterContainer();
            container
                .AddInstance(typeof(int), 1234)
                .AddInstance(typeof(string), "Hello")
                .AddInstance(typeof(int), 2)
                .AddInstance(typeof(IShifterContainer), ShifterContainer.Default);

            // Act
            container.Unregister(typeof(int));
            var resolvedInts = container.ResolveAll<int>();
            var resolvedString = container.ResolveAll<string>();
            var resolvedContainer = container.ResolveAll<IShifterContainer>();

            // Assert   
            resolvedInts.Should().BeEmpty("because these are unregistered.");
            resolvedString.Count().Should().Be(1, "because one string was left.");
            resolvedContainer.Count().Should().Be(1, "because one IShifterContainer was left.");
        }

        [TestMethod]
        public void WhenUnregisteringATypeGenericallyItShouldUnregisterAllInstancesOfThatType()
        {
            // Arrange
            var container = new ShifterContainer();
            container
                .AddInstance(typeof(int), 1234)
                .AddInstance(typeof(string), "Hello")
                .AddInstance(typeof(int), 2)
                .AddInstance(typeof(IShifterContainer), ShifterContainer.Default);

            // Act
            container.Unregister<int>();
            var resolvedInts = container.ResolveAll<int>();
            var resolvedString = container.ResolveAll<string>();
            var resolvedContainer = container.ResolveAll<IShifterContainer>();

            // Assert   
            resolvedInts.Should().BeEmpty("because these are unregistered.");
            resolvedString.Count().Should().Be(1, "because one string was left.");
            resolvedContainer.Count().Should().Be(1, "because one IShifterContainer was left.");
        }

        [TestMethod]
        public void WhenResettingItShouldUnregisterAllInstancesOfThatType()
        {
            // Arrange
            var container = new ShifterContainer();
            container
                .AddInstance(typeof(int), 1234)
                .AddInstance(typeof(string), "Hello")
                .AddInstance(typeof(int), 2)
                .AddInstance(typeof(IShifterContainer), ShifterContainer.Default);

            // Act
            container.Reset();
            var resolvedInts = container.ResolveAll<int>();
            var resolvedString = container.ResolveAll<string>();
            var resolvedContainer = container.ResolveAll<IShifterContainer>();

            // Assert   
            resolvedInts.Should().BeEmpty("because these ints are unregistered.");
            resolvedString.Should().BeEmpty("because these strings are unregistered.");
            resolvedContainer.Should().BeEmpty("because these containers are unregistered.");
        }

        [TestMethod]
        public void WhenCallingResetItShouldBePossibleToRegisterInstancesAgain()
        {
            // Arrange
            var container = new ShifterContainer();
            
            // Act
            container.Reset();
            container.AddInstance("string");
            var resolvedValue = container.Resolve<string>();

            // Assert    
            resolvedValue.Should().Be("string", "because we added it.");
        }
    }
}