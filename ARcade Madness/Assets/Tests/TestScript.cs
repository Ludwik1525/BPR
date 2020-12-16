using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestScriptSimplePasses()
        {
            GameObject go = new GameObject();
            var gm = go.AddComponent<GameMangerTest>();

            gm.SpawnPlayer();

            var player = gm.GetPlayer();
            
            Assert.AreEqual(new Vector3(0.2f,0.14f,0.12f), player.transform.position);
        }

        [Test]
        public void DoDamageTest()
        {
            GameObject go = new GameObject();
            var gm = go.AddComponent<GameMangerTest>();

            gm.SpawnPlayer();

            var player = gm.GetPlayer();

            player.GetComponent<BattleTest>().DoDamage(300);

            //Originaly 3600
            Assert.AreEqual(3300, player.GetComponent<SpinTest>().health);
        }

        [Test]
        public void DieTest()
        {
            GameObject go = new GameObject();
            var gm = go.AddComponent<GameMangerTest>();

            gm.SpawnPlayer();

            var player = gm.GetPlayer();

            player.GetComponent<BattleTest>().DoDamage(3600);

            Assert.AreEqual(true, player.GetComponent<BattleTest>().isDead );
        }



    }
}
