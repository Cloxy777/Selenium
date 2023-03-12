using Selenium.Heroes.Common;
using Selenium.Heroes.Common.Loaders;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;
using System;

namespace Selenium.Heroes.TwoTowers.Tests;

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
        var deck = new Deck();
        deck = deck.Draw(cardDescriptors);

        var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors, deck);
        var turn = decisionMaker.CreateTurn();
        var move = turn.Moves.First();

        Console.WriteLine($"ActionType: {move.ActionType}.");
        Console.WriteLine($"CardDescriptor: {move.CardDescriptor.BaseCardEffect.Card.Header}.");

        Assert.AreEqual(ActionType.Play, move.ActionType);
        Assert.AreEqual("RABID SHEEP", move.CardDescriptor.BaseCardEffect.Card.Header);
    }

    [TestMethod]
    public void MakeDecision_Play_Screenshot_0002()
    {
        var player = new Player("Player")
        {
            Ore = 8,
            Mana = 11,
            Stacks = 15,
            Mines = 3,
            Monasteries = 2,
            Barracks = 2,
            Tower = 28,
            Wall = 5,
        };

        var enemy = new Player("Enemy")
        {
            Ore = 1,
            Mana = 20,
            Stacks = 9,
            Mines = 3,
            Monasteries = 4,
            Barracks = 2,
            Tower = 18,
            Wall = 11,
        };

        var headers = new[] { "BARRACKS", "DRAGON'S HEART", "GREATER WALL", "CRUSHER", "CRYSTAL SHIELD", "SANCTUARY" };
        var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors.Where(x => headers.Contains(x.BaseCardEffect.Card.Header)).ToList();
        var deck = new Deck();
        deck = deck.Draw(cardDescriptors);

        var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors, deck);
        var turn = decisionMaker.CreateTurn();
        var move = turn.Moves.First();


        Console.WriteLine($"ActionType: {move.ActionType}.");
        Console.WriteLine($"CardDescriptor: {move.CardDescriptor.BaseCardEffect.Card.Header}.");

        Assert.AreEqual(ActionType.Play, move.ActionType);
        Assert.AreEqual("CRUSHER", move.CardDescriptor.BaseCardEffect.Card.Header);
    }

    [TestMethod]
    public void MakeDecision_Play_Screenshot_0003()
    {
        var player = new Player("Player")
        {
            Ore = 27,
            Mana = 8,
            Stacks = 3,
            Mines = 4,
            Monasteries = 2,
            Barracks = 2,
            Tower = 24,
            Wall = 0,
        };

        var enemy = new Player("Enemy")
        {
            Ore = 14,
            Mana = 11,
            Stacks = 8,
            Mines = 4,
            Monasteries = 2,
            Barracks = 2,
            Tower = 13,
            Wall = 0,
        };

        var headers = new[] { "SINGING COAL", "STONE DEVOURERS", "LARGE WALL", "ORDINARY WALL", "VAMPIRE", "FOUNDATION" };
        var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors.Where(x => headers.Contains(x.BaseCardEffect.Card.Header)).ToList();
        var deck = new Deck();
        deck = deck.Draw(cardDescriptors);

        var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors, deck);
        var turn = decisionMaker.CreateTurn();
        var move = turn.Moves.First();

        Console.WriteLine($"ActionType: {move.ActionType}.");
        Console.WriteLine($"CardDescriptor: {move.CardDescriptor.BaseCardEffect.Card.Header}.");

        Assert.AreEqual(ActionType.Play, move.ActionType);
        Assert.AreEqual("FOUNDATION", move.CardDescriptor.BaseCardEffect.Card.Header);
    }

    [TestMethod]
    public void MakeDecision_Play_Screenshot_0004()
    {
        var player = new Player("Player")
        {
            Ore = 7,
            Mana = 4,
            Stacks = 16,
            Mines = 3,
            Monasteries = 2,
            Barracks = 2,
            Tower = 27,
            Wall = 9,
        };

        var enemy = new Player("Enemy")
        {
            Ore = 14,
            Mana = 1,
            Stacks = 4,
            Mines = 4,
            Monasteries = 2,
            Barracks = 3,
            Tower = 30,
            Wall = 1,
        };

        var headers = new[] { "BASTION", "BARRACKS", "WARRIOR", "TREMOR", "CAUSTIC CLOUD", "DIE MOULD" };
        var cardDescriptors = CardDescriptorsLoader.AllCardDescriptors.Where(x => headers.Contains(x.BaseCardEffect.Card.Header)).ToList();
        var deck = new Deck();
        deck = deck.Draw(cardDescriptors);

        var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors, deck);
        var turn = decisionMaker.CreateTurn();
        var move = turn.Moves.First();

        Console.WriteLine($"ActionType: {move.ActionType}.");
        Console.WriteLine($"CardDescriptor: {move.CardDescriptor.BaseCardEffect.Card.Header}.");

        Assert.AreEqual(ActionType.Play, move.ActionType);
        Assert.AreEqual("WARRIOR", move.CardDescriptor.BaseCardEffect.Card.Header);

        //Assert.AreEqual(ActionType.Discard, move.ActionType);
        //Assert.AreEqual("BASTION", move.CardDescriptor.BaseCardEffect.Card.Header);
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
        var deck = new Deck();
        deck = deck.Draw(cardDescriptors);

        var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors, deck);
        var turn = decisionMaker.CreateTurn();
        var move = turn.Moves.First();

        Console.WriteLine($"ActionType: {move.ActionType}.");
        Console.WriteLine($"CardDescriptor: {move.CardDescriptor.BaseCardEffect.Card.Header}.");

        Assert.AreEqual(ActionType.Discard, move.ActionType);
        Assert.AreEqual("DRAGON'S HEART", move.CardDescriptor.BaseCardEffect.Card.Header);
    }

    [TestMethod]
    // TODO: rename to Play
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
        var deck = new Deck();
        deck = deck.Draw(cardDescriptors);

        var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors, deck);
        var turn = decisionMaker.CreateTurn();
        var move = turn.Moves.First();

        Console.WriteLine($"ActionType: {move.ActionType}.");
        Console.WriteLine($"CardDescriptor: {move.CardDescriptor.BaseCardEffect.Card.Header}.");

        Assert.AreEqual(ActionType.Play, move.ActionType);
        Assert.AreEqual("BEETLE", move.CardDescriptor.BaseCardEffect.Card.Header);

        //Assert.AreEqual(ActionType.Discard, move.ActionType);
        //Assert.AreEqual("SHIFT", move.CardDescriptor.BaseCardEffect.Card.Header);
        //Assert.AreEqual("SUBSOIL WATERS", move.CardDescriptor.BaseCardEffect.Card.Header);
    }

    [TestMethod]
    public void MakeDecision_Discard_Screenshot_0003()
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
        var deck = new Deck();
        deck = deck.Draw(cardDescriptors);

        var decisionMaker = new DecisionMaker(player, enemy, cardDescriptors, deck);
        var turn = decisionMaker.CreateTurn();
        var move = turn.Moves.First();

        Console.WriteLine($"ActionType: {move.ActionType}.");
        Console.WriteLine($"CardDescriptor: {move.CardDescriptor.BaseCardEffect.Card.Header}.");

        Assert.AreEqual(ActionType.Play, move.ActionType);
        Assert.AreEqual("ABUNDANT SOIL", move.CardDescriptor.BaseCardEffect.Card.Header);
    }
}