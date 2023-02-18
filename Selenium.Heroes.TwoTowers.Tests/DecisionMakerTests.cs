using Selenium.Heroes.Common.Loaders;
using Selenium.Heroes.Common.Models;
using System;

namespace Selenium.Heroes.TwoTowers.Tests
{
    [TestClass]
    public class DecisionMakerTests
    {
        [TestMethod]
        public void MakeDecision_Play_Screenshot_0001()
        {
            var player = new Player("Player")
            {
                Ore = 7,
                Mana = 7,
                Stacks = 7,
                Mines = 2,
                Monasteries = 2,
                Barracks = 2,
                Tower = 20,
                Wall = 5,
            };

            var enemy = new Player("Enemy")
            {
                Ore = 4,
                Mana = 7,
                Stacks = 7,
                Mines = 2,
                Monasteries = 2,
                Barracks = 2,
                Tower = 20,
                Wall = 9,
            };

            var headers = new[] { "RABID SHEEP", "ORC", "DRAGON", "SMOKY QUARTZ", "STONE DEVOURERS", "FORTIFIED WALL" };
            var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors.Where(x => headers.Contains(x.BaseCardEffect.Card.Header)).ToList();

            var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors);
            var decision = decisionMaker.MakeDecision();

            Console.WriteLine($"ActionType: {decision.ActionType}.");
            Console.WriteLine($"CardDescriptor: {decision.CardDescriptor.BaseCardEffect.Card.Header}.");

            Assert.AreEqual(ActionType.Play, decision.ActionType);
            Assert.AreEqual("SMOKY QUARTZ", decision.CardDescriptor.BaseCardEffect.Card.Header);
        }

        [TestMethod]
        public void MakeDecision_Play_Screenshot_0002()
        {
            var player = new Player("Player")
            {
                Ore = 7,
                Mana = 7,
                Stacks = 7,
                Mines = 2,
                Monasteries = 2,
                Barracks = 2,
                Tower = 20,
                Wall = 5,
            };

            var enemy = new Player("Enemy")
            {
                Ore = 7,
                Mana = 7,
                Stacks = 7,
                Mines = 2,
                Monasteries = 2,
                Barracks = 3,
                Tower = 20,
                Wall = 5,
            };

            var headers = new[] { "BARRACKS", "ROCKCASTER", "FAMILIAR", "ABUNDANT SOIL", "TINY SNAKES", "FISSION" };
            var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors.Where(x => headers.Contains(x.BaseCardEffect.Card.Header)).ToList();

            var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors);
            var decision = decisionMaker.MakeDecision();

            Console.WriteLine($"ActionType: {decision.ActionType}.");
            Console.WriteLine($"CardDescriptor: {decision.CardDescriptor.BaseCardEffect.Card.Header}.");

            Assert.AreEqual(ActionType.Play, decision.ActionType);
            Assert.AreEqual("ABUNDANT SOIL", decision.CardDescriptor.BaseCardEffect.Card.Header);
        }

        [TestMethod]
        public void MakeDecision_Discard_Screenshot_0001()
        {
            var player = new Player("Player")
            {
                Ore = 9,
                Mana = 3,
                Stacks = 6,
                Mines = 2,
                Monasteries = 2,
                Barracks = 2,
                Tower = 21,
                Wall = 0,
            };

            var enemy = new Player("Enemy")
            {
                Ore = 9,
                Mana = 6,
                Stacks = 2,
                Mines = 2,
                Monasteries = 2,
                Barracks = 2,
                Tower = 20,
                Wall = 5,
            };

            var headers = new[] { "DRAGON'S HEART", "SINGING COAL", "SUBSOIL WATERS", "SAPPHIRE", "THIEF", "PEGASUS RIDER" };
            var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors.Where(x => headers.Contains(x.BaseCardEffect.Card.Header)).ToList();

            var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors);
            var decision = decisionMaker.MakeDecision();

            Console.WriteLine($"ActionType: {decision.ActionType}.");
            Console.WriteLine($"CardDescriptor: {decision.CardDescriptor.BaseCardEffect.Card.Header}.");

            Assert.AreEqual(ActionType.Discard, decision.ActionType);
            Assert.AreEqual("DRAGON'S HEART", decision.CardDescriptor.BaseCardEffect.Card.Header);
        }

        [TestMethod]
        public void MakeDecision_Discard_Screenshot_0002()
        {
            var player = new Player("Player")
            {
                Ore = 6,
                Mana = 18,
                Stacks = 10,
                Mines = 2,
                Monasteries = 3,
                Barracks = 3,
                Tower = 30,
                Wall = 1,
            };

            var enemy = new Player("Enemy")
            {
                Ore = 8,
                Mana = 10,
                Stacks = 9,
                Mines = 3,
                Monasteries = 3,
                Barracks = 3,
                Tower = 20,
                Wall = 12,
            };

            var headers = new[] { "SHIFT", "STONE GIANT", "SUBSOIL WATERS", "BEETLE", "THIEF", "DRAGON'S EYE" };
            var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors.Where(x => headers.Contains(x.BaseCardEffect.Card.Header)).ToList();

            var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors);
            var decision = decisionMaker.MakeDecision();

            Console.WriteLine($"ActionType: {decision.ActionType}.");
            Console.WriteLine($"CardDescriptor: {decision.CardDescriptor.BaseCardEffect.Card.Header}.");

            Assert.AreEqual(ActionType.Discard, decision.ActionType);
            Assert.AreEqual("SUBSOIL WATERS", decision.CardDescriptor.BaseCardEffect.Card.Header);
        }
    }
}