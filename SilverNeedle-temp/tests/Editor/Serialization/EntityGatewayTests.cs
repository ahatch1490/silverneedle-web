// Copyright (c) 2017 Trevor Redfern
//
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace Tests.Serialization 
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using SilverNeedle.Serialization;
    using System.Linq;
    using SilverNeedle.Characters;
    using System;

    [TestFixture]
    public class EntityGatewayTests {
        private EntityGateway<TestObject> subject;

        [SetUp]
        public void SetUp()
        {
            var data = new List<IObjectStore>();
            var entity1 = new MemoryStore();
            entity1.SetValue("name", "prop1");
            var entity2 = new MemoryStore();
            entity2.SetValue("name", "prop2");
            data.Add(entity1);
            data.Add(entity2);

            subject = new EntityGateway<TestObject>(data);
        }

        [Test]
        public void LoadsClassesFromObjectStores() {           
            Assert.AreEqual(1, subject.Where(x => x.Name == "prop1").Count());
            Assert.AreEqual(1, subject.Where(x => x.Name == "prop2").Count());
        }

        [Test]
        public void LoadsDirectlyFromDatafileLoaderByDefault()
        {
            var gateway = new EntityGateway<PersonalityType>();
            Assert.Greater(gateway.All().Count(), 0);
        }

        [Test]
        public void CanReturnASingleEntityThatMatches()
        {
            var prop1 = subject.Find("prop1");
            Assert.IsNotNull(prop1);
        }

        [Test]
        public void CanForceGatewayToASpecificList()
        {
            var list = new List<TestObject>();
            list.Add(new TestObject("hello1"));
            list.Add(new TestObject("hello2"));
            var gateway = new EntityGateway<TestObject>(list);
            Assert.AreEqual(2, gateway.Count());
            Assert.IsNotNull(gateway.Find("hello2"));
        }

        [Test]
        public void UsesCustomSerializerIfPossible()
        {
            var data = new MemoryStore();
            data.SetValue("name", "Foo");
            var gateway = new EntityGateway<TestSerialized>(new IObjectStore[] { data });    
            var first = gateway.All().First();
            Assert.That(first.Name, Is.EqualTo("Foo"));
        }

        [Test]
        public void ObjectsFlaggedAsAlwaysFreshShouldReloadEveryRequest()
        {
            var data = new MemoryStore();
            data.SetValue("name", "Foo");
            var data2 = new MemoryStore();
            data2.SetValue("name", "Bar");
            var gateway = new EntityGateway<AlwaysFresh>(new IObjectStore[] { data, data2 });

            var item = gateway.Find("Foo");
            item.SomeValue = false;

            var itemAgain = gateway.Find("Foo");
            Assert.That(itemAgain.SomeValue, Is.True);
            
        }

        [Test]
        public void ThrowsNotFoundExceptionIfObjectIsnotFound()
        {
            var gateway = new EntityGateway<TestObject>(new TestObject[] { new TestObject("Bar") });
            Assert.Throws(typeof(EntityNotFoundException), () => {gateway.Find("Foo"); });
        }

        [Test]
        public void CustomImplementationForObjectIsAllowed()
        {
            var data = new MemoryStore();
            data.SetValue("name", "Foo");
            data.SetValue("custom-implementation", "Tests.Serialization.EntityGatewayTests+TestObjectCustom");
            var data2 = new MemoryStore();
            data2.SetValue("name", "Bar");
            var gateway = new EntityGateway<TestObject>(new IObjectStore[] { data, data2});
            var one = gateway.Find("Foo");
            Assert.That(one, Is.InstanceOf(typeof(TestObjectCustom)));
            var two = gateway.Find("Bar");
            Assert.That(one, Is.InstanceOf(typeof(TestObject)));
        }

        [ObjectStoreSerializable]
        public class AlwaysFresh : IGatewayObject, IGatewayCopy<AlwaysFresh>
        {
            [ObjectStore("name")]
            public string Name { get; set; }

            public bool SomeValue = true;
            public bool Matches(string name)
            {
                return Name.Equals(name);
            }

            public AlwaysFresh() { }

            public AlwaysFresh(AlwaysFresh prev)
            {
                this.Name = prev.Name;
                this.SomeValue = prev.SomeValue;
            }

            public AlwaysFresh Copy()
            {
                return new AlwaysFresh(this);
            }
        }

        [ObjectStoreSerializable]
        public class TestSerialized : IGatewayObject
        {
            [ObjectStore("name")]
            public string Name { get; set; }

            public bool Matches(string name)
            {
                return true;
            }
        }

        public class TestObject : IGatewayObject {
            public string Name { get; private set; }
            public TestObject(IObjectStore data) {
                Name = data.GetString("name");
            }

            public TestObject(string name)
            {
                Name = name;
            }

            public bool Matches(string name)
            {
                return Name == name;
            }
        }

        public class TestObjectCustom : TestObject 
        { 
            public TestObjectCustom(IObjectStore data) : base(data)
            {

            }
        }
    }
}