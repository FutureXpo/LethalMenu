﻿using GameNetcodeStuff;
using LethalMenu.Util;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace LethalMenu.Handler
{
    enum Behaviour
    {
        Idle = 0,
        Chase = 1,
        Aggravated = 2,
        Unknown = 3
    }
    public class EnemyHandler
    {
        private EnemyAI enemy;
        private PlayerControllerB target;

        public EnemyHandler(EnemyAI enemy)
        {
            this.enemy = enemy;
        }

        private void HandleLureCrawler()
        {
            CrawlerAI crawler = enemy as CrawlerAI;

            crawler.BeginChasingPlayerServerRpc((int)target.playerClientId);
        }

        private void HandleLureMouthDog()
        {
            MouthDogAI dog = enemy as MouthDogAI;

            dog.ReactToOtherDogHowl(target.transform.position);
        }

        private void HandleLureBaboonBird()
        {
            BaboonBirdAI baboon = enemy as BaboonBirdAI;

            Threat threat = new()
            {
                threatScript = target,
                lastSeenPosition = target.transform.position,
                threatLevel = int.MaxValue,
                type = ThreatType.Player,
                focusLevel = int.MaxValue,
                timeLastSeen = Time.time,
                distanceToThreat = 0.0f,
                distanceMovedTowardsBaboon = float.MaxValue,
                interestLevel = int.MaxValue,
                hasAttacked = true
            };

            baboon.SetAggressiveModeServerRpc(1);
            baboon.Reflect().Invoke("ReactToThreat", threat);
        }

        private void HandleLureForestGiant()
        {
            ForestGiantAI giant = enemy as ForestGiantAI;

            giant.SwitchToBehaviourServerRpc((int)Behaviour.Chase);
            giant.StopSearch(giant.roamPlanet, false);
            giant.chasingPlayer = target;
            giant.investigating = true;

            giant.SetDestinationToPosition(target.transform.position);
            giant.Reflect().SetValue("lostPlayerInChase", false);
        }

        private void HandleLureCentipede()
        {
            CentipedeAI centipede = enemy as CentipedeAI;
            centipede.SwitchToBehaviourServerRpc((int)Behaviour.Aggravated);
            //centipede.ClingToPlayerServerRpc(target.playerClientId);
            bool clingingToCeiling = (bool)centipede.Reflect().GetValue("clingingToCeiling");

            if(clingingToCeiling) centipede.TriggerCentipedeFallServerRpc(target.playerClientId);

           
        }

        private void HandleLureFlowerman()
        {
            FlowermanAI flowerman = enemy as FlowermanAI;
            flowerman.SwitchToBehaviourServerRpc((int)Behaviour.Aggravated);
            flowerman.EnterAngerModeServerRpc(20);
        }

        private void HandleLureSandSpider()
        {
            SandSpiderAI spider = enemy as SandSpiderAI;
            spider.SwitchToBehaviourServerRpc((int)Behaviour.Chase);
            //spider.meshContainer.position = target.transform.position;
            //spider.SyncMeshContainerPositionToClients();
           
            int web = spider.SpawnWeb(target.transform.position);

            spider.webTraps.ForEach(web => spider.PlayerTripWebServerRpc(web.trapID, (int) target.playerClientId));
           

            //spider.Reflect().SetValue("onWall", false).SetValue("watchFromDistance", false);
        }

        private void HandleLureRedLocustBees()
        {
            RedLocustBees bees = enemy as RedLocustBees;
            bees.SwitchToBehaviourServerRpc((int)Behaviour.Aggravated);
            bees.hive.isHeld = true;
        }

        private void HandleLureHoarderBug()
        {
            HoarderBugAI bug = enemy as HoarderBugAI;
            bug.SwitchToBehaviourServerRpc((int)Behaviour.Aggravated);
            bug.angryAtPlayer = target;
            bug.angryTimer = float.MaxValue;

            bug.Reflect().SetValue("lostPlayerInChase", false).Invoke("SyncNestPositionServerRpc", target.transform.position);
        }

        private void HandleLureNutcrackerEnemy()
        {
            NutcrackerEnemyAI nutcracker = enemy as NutcrackerEnemyAI;
            nutcracker.SwitchToBehaviourServerRpc((int)Behaviour.Aggravated);

            nutcracker.Reflect().SetValue("lastSeenPlayerPos", target.transform.position).Invoke("timeSinceSeeingTarget", 0);
        }

        private void HandleLureMaskedPlayerEnemy()
        {
            MaskedPlayerEnemy masked = enemy as MaskedPlayerEnemy;
            masked.SwitchToBehaviourServerRpc((int)Behaviour.Chase);
        }

        private void HandleLureSpringMan()
        {
            SpringManAI spring = enemy as SpringManAI;
            spring.SwitchToBehaviourServerRpc((int)Behaviour.Chase);
        }

        private void HandleLurePuffer()
        {
            PufferAI puffer = enemy as PufferAI;
            puffer.SwitchToBehaviourServerRpc((int)Behaviour.Aggravated);
        }

        private void HandleLureJester()
        {
            JesterAI jester = enemy as JesterAI;
            jester.SwitchToBehaviourServerRpc((int)Behaviour.Aggravated);
        }

        private void HandleLureSandWorm()
        {
            SandWormAI worm = enemy as SandWormAI;
            worm.SwitchToBehaviourServerRpc((int)Behaviour.Chase);
        }
        private void HandleLureEnemyByType()
        {

            switch (enemy)
            {
                case CrawlerAI:
                    HandleLureCrawler();
                    break;
                case MouthDogAI:
                    HandleLureMouthDog();
                    break;
                case BaboonBirdAI:
                    HandleLureBaboonBird();
                    break;
                case ForestGiantAI:
                    HandleLureForestGiant();
                    break;
                case CentipedeAI:
                    HandleLureCentipede();
                    break;
                case FlowermanAI:
                    HandleLureFlowerman();
                    break;
                case SandSpiderAI:
                    HandleLureSandSpider();
                    break;
                case RedLocustBees:
                    HandleLureRedLocustBees();
                    break;
                case HoarderBugAI:
                    HandleLureHoarderBug();
                    break;
                case NutcrackerEnemyAI:
                    HandleLureNutcrackerEnemy();
                    break;
                case MaskedPlayerEnemy:
                    HandleLureMaskedPlayerEnemy();
                    break;
                case SpringManAI:
                    HandleLureSpringMan();
                    break;
                case PufferAI:
                    HandleLurePuffer();
                    break;
                case JesterAI:
                    HandleLureJester();
                    break;
                case SandWormAI:
                    HandleLureSandWorm();
                    break;
                default:
                    enemy.SwitchToBehaviourServerRpc((int)Behaviour.Chase);
                    break;
            }
        }

        private void HandleSandWormKillPlayer()
        {
            SandWormAI worm = enemy as SandWormAI;
            Teleport(target);
            worm.StartEmergeAnimation();
        }

        private void HandleKillPlayerByType()
        {

            switch (enemy)
            {
                case MouthDogAI dog:
                    dog.KillPlayerServerRpc((int)target.playerClientId);
                    break;
                case ForestGiantAI giant:
                    giant.GrabPlayerServerRpc((int)target.playerClientId);
                    break;
                case FlowermanAI flowerman:
                    flowerman.KillPlayerAnimationServerRpc((int)target.playerClientId);
                    break;
                case RedLocustBees bees:
                    bees.BeeKillPlayerServerRpc((int)target.playerClientId);
                    break;
                case NutcrackerEnemyAI nutcracker:
                    nutcracker.LegKickPlayerServerRpc((int)target.playerClientId);
                    break;
                case MaskedPlayerEnemy masked:
                    masked.KillPlayerAnimationServerRpc((int)target.playerClientId);
                    break;
                case JesterAI jester:
                    jester.KillPlayerServerRpc((int)target.playerClientId);
                    break;
                case SandWormAI worm:
                    HandleSandWormKillPlayer();
                    break;
                case CentipedeAI centipede:
                    centipede.SwitchToBehaviourServerRpc((int)Behaviour.Aggravated);
                    centipede.ClingToPlayerServerRpc(target.playerClientId);
                    break;
                case BlobAI blob:
                    blob.SlimeKillPlayerEffectServerRpc((int) target.playerClientId);
                    break;
            }
        }
         
        public bool HasInstaKill()
        {
            List<System.Type> types = new()
            {
                typeof(MouthDogAI),
                typeof(ForestGiantAI),
                typeof(FlowermanAI),
                typeof(RedLocustBees),
                typeof(NutcrackerEnemyAI),
                typeof(MaskedPlayerEnemy),
                typeof(JesterAI),
                typeof(SandWormAI),
                typeof(BlobAI),
                typeof(CentipedeAI)
            };
                
            return types.Contains(enemy.GetType());
        }

        public void Control()
        {
            if (enemy.isEnemyDead) return;

            Cheats.EnemyControl.Control(enemy);
        }

        public void Kill(bool despawn = false)
        {
            bool forceDespawn = false;

            if (enemy.GetType() == typeof(ForestGiantAI)
                        || enemy.GetType() == typeof(SandWormAI)
                        || enemy.GetType() == typeof(BlobAI)
                        || enemy.GetType() == typeof(DressGirlAI)
                        || enemy.GetType() == typeof(PufferAI)
                        || enemy.GetType() == typeof(SpringManAI)
                        || enemy.GetType() == typeof(DocileLocustBeesAI)
                        || enemy.GetType() == typeof(DoublewingAI)
                        || enemy.GetType() == typeof(RedLocustBees)
                        || enemy.GetType() == typeof(LassoManAI)
                        || enemy.GetType() == typeof(JesterAI)
                        ) forceDespawn = true;

            enemy.KillEnemyServerRpc(forceDespawn ? forceDespawn : despawn);
        }

        public void Stun()
        {
            if (!enemy.enemyType.canBeStunned) return;
            enemy.SetEnemyStunned(true, 5);
        }

        public void Teleport(PlayerControllerB player)
        {
            if (enemy.GetType() != typeof(MaskedPlayerEnemy) && (enemy.isOutside && player.isInsideFactory || !enemy.isOutside && !player.isInsideFactory)) return;

            enemy.ChangeEnemyOwnerServerRpc(LethalMenu.localPlayer.actualClientId);
            enemy.transform.position = player.transform.position;
            enemy.SyncPositionToClients();
        }


        public void TargetPlayer(PlayerControllerB player)
        {
            target = player;
            enemy.targetPlayer = player;
            enemy.ChangeEnemyOwnerServerRpc(LethalMenu.localPlayer.actualClientId);
            enemy.SetMovingTowardsTargetPlayer(player);
            HandleLureEnemyByType();
        }

        public void KillPlayer(PlayerControllerB player)
        {
            target = player;
            enemy.targetPlayer = player;
            enemy.ChangeEnemyOwnerServerRpc(LethalMenu.localPlayer.actualClientId);
            HandleKillPlayerByType();
        }

        public static EnemyHandler GetHandler(EnemyAI enemy) => new(enemy);
    }

    public static class EnemyAIExtensions
    {
        public static EnemyHandler Handle(this EnemyAI enemy) => EnemyHandler.GetHandler(enemy);
    }

    public static class HoarderBugAIExtensions
    {
        public static void StealAllItems(this HoarderBugAI bug)
        {
            bug.ChangeEnemyOwnerServerRpc(LethalMenu.localPlayer.actualClientId);

            LethalMenu.Instance.StartCoroutine(StealItems(bug));
        }

        private static IEnumerator StealItems(HoarderBugAI bug)
        {
            List<NetworkObject> items = LethalMenu.items.FindAll(i => !i.isHeld && !i.isPocketed && !i.isInShipRoom && i.isInFactory).ConvertAll(i => i.NetworkObject);

            foreach (var obj in items)
            {
                yield return new WaitForSeconds(0.2f);
                bug.GrabItemServerRpc(obj);
                bug.DropItemServerRpc(obj, bug.nestPosition, true);
            }
        }
    }

    public static class SandSpiderAIExtensions
    {
        public static int SpawnWeb(this SandSpiderAI spider, Vector3 position)
        {
            spider.ChangeEnemyOwnerServerRpc(LethalMenu.localPlayer.actualClientId);

            Ray ray = new Ray(position, Vector3.Scale(Random.onUnitSphere, new Vector3(1f, Random.Range(0.6f, 1f), 1f)));

            if (Physics.Raycast(ray, out RaycastHit rayHit, 7f, StartOfRound.Instance.collidersAndRoomMask) && (double)rayHit.distance >= 1.5)
            {
                Vector3 point = rayHit.point;
                if (Physics.Raycast(position, Vector3.down, out rayHit, 10f, StartOfRound.Instance.collidersAndRoomMask))
                {
                    spider.SpawnWebTrapServerRpc(rayHit.point, point);


                    return spider.webTraps.Count - 1;
                }
            }

            return -1;
        }

        public static void BreakAllWebs(this SandSpiderAI spider)
        {
            spider.webTraps.ForEach(web => spider.BreakWebServerRpc(web.trapID, -1));
        }
    }
}
