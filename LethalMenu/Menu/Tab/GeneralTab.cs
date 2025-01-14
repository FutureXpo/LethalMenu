﻿using LethalMenu.Menu.Core;
using UnityEngine;

namespace LethalMenu.Menu.Tab
{
    internal class GeneralTab : MenuTab
    {
        Vector2 scrollPos;
        private Texture2D avatar;

        public GeneralTab() : base("General")
        {
            avatar = GetImage("https://icyrelic.com/img/Avatar2.jpg");
        }

        public override void Draw()
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(GUILayout.Width(HackMenu.Instance.contentWidth - HackMenu.Instance.spaceFromLeft));
            MenuContent();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            string intoText = "Thank you for using Lethal Menu.\n\nIf you have any suggestions please leave a comment on the forum post.\nAny bugs you find please provide some steps to recreate the issue and leave a comment.";

            //draw the avatar with the intoText on the right
            GUILayout.BeginHorizontal();
            GUILayout.Label(avatar, GUILayout.Width(100), GUILayout.Height(100));
            GUILayout.Label(intoText);
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            foreach (string line in Settings.Changelog.changes)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);

                if (line.StartsWith("v")) style.fontStyle = FontStyle.Bold;
                GUILayout.Label(line.StartsWith("v") ? "Changelog " + line : line, style);
            }

            GUILayout.EndScrollView();
        }
    }
}
