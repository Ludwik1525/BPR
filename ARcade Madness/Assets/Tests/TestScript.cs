using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestScript
    {
        //Tests if player spawns in correct position
        [Test]
        public void StartPositionTest()
        {
            GameObject go = new GameObject();
            var gm = go.AddComponent<GameMangerTest>();
            gm.SpawnPlayer();
            var player = gm.GetPlayer();
            
            Assert.AreEqual(new Vector3(0.2f,0.14f,0.12f), player.transform.position);
        }

        //Tests if player takes damage correctly
        [Test]
        public void DoDamageTest()
        {
            GameObject go = new GameObject();
            var gm = go.AddComponent<GameMangerTest>();
            gm.SpawnPlayer();
            var player = gm.GetPlayer();
            player.GetComponent<BattleTest>().DoDamage(300);

            //Player spawns with 3600 health
            Assert.AreEqual(3300, player.GetComponent<SpinTest>().health);
        }

        //Tests if player dies after taking lethal dose of damage
        [Test]
        public void DieTest()
        {
            GameObject go = new GameObject();
            var gm = go.AddComponent<GameMangerTest>();
            gm.SpawnPlayer();
            var player = gm.GetPlayer();
            player.GetComponent<BattleTest>().DoDamage(3600);

            Assert.AreEqual(true, player.GetComponent<BattleTest>().isDead);
        }



    }
}
