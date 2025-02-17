﻿using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace LethalMenu.Cheats
{
    [HarmonyPatch]
    internal class UnlimitedStamina : Cheat
    {
        public override void Update()
        {
            if (!Hack.UnlimitedStamina.IsEnabled()) return;

            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player == null) return;
            player.sprintMeter = 1f;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), "LateUpdate")]
        public static void PlayerLateUpdate(PlayerControllerB __instance)
        {
            if (LethalMenu.localPlayer == null || LethalMenu.localPlayer.playerClientId != __instance.playerClientId || !Hack.UnlimitedStamina.IsEnabled()) return;

            __instance.sprintMeter = 1f;
            if (__instance.sprintMeterUI != null) __instance.sprintMeterUI.fillAmount = 1f;

        }
    }
}
