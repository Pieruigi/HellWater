using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{

    public enum Language { English, Italian }
    public enum CutSceneState { Executed = 0, Ready = 1 }

    

    // Tags
    public class Tags
    {
        public static readonly string Player = "Player";
        public static readonly string Enemy = "Enemy";
        public static readonly string Npc = "Npc";
        public static readonly string StartingPoint = "StartingPoint";
    }


    public class Constants
    {
        // Interaction
        public static readonly float InteractionCooldownTime = 1.5f;
        public static readonly int DoorLockedState = 0;
        public static readonly int DoorClosedState = 1;
        public static readonly int DoorOpenState = 2;

        // Raycast
        public static readonly float RaycastVerticalOffset = 1f;

        // Resources
        public static readonly string ResourcesFolderEquipment = "Items/Equipment";
        public static readonly string ResourcesFolderMessageCollection = "MessageCollection";

        // Misc
        public static readonly float OrthographicAngle = 60f;
    }


}
