using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shifter.Tests
{
    [TestClass]
    public class ServiceLocatorTests
    {
        [TestMethod]
        public void WhenCallingGetServiceItShouldReturnTheValueRegisteredForTheService()
        {
            // Arrange
            var container = new ShifterContainer();
            container.AddInstance("The string that should be returned.");

            // Act
            var service = container.GetService(typeof(string));

            // Assert  
            service.Should().Be("The string that should be returned.", "because this string should be returned.");
        }

        [TestMethod]
        public void WhenCallingGetInstanceItShouldReturnTheInstanceRegisteredForTheCall()
        {
            // Arrange
            var container = new ShifterContainer();
            container.AddInstance("The string that should be returned.");

            // Act
            var instance = container.GetInstance(typeof(string));

            // Assert  
            instance.Should().Be("The string that should be returned.", "because this string should be returned.");  
        }

        [TestMethod]
        public void WhenCallingGenericGetInstanceItShouldReturnTheInstanceRegisteredForTheCall()
        {
            // Arrange
            var container = new ShifterContainer();
            container.AddInstance("The string that should be returned.");

            // Act
            var instance = container.GetInstance<string>();

            // Assert  
            instance.Should().Be("The string that should be returned.", "because this string should be returned.");
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
            var resolveAll = container.GetAllInstances(typeof(string));

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
            var resolveAll = container.GetAllInstances<String>();

            // Assert 
            resolveAll.Count().Should().Be(4, "because there are 4 instances registered.");
            resolveAll.Should().Contain(expectedItemsList);
        }
    }
}