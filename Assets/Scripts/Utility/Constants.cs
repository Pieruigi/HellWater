using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{

    public enum Language { English, Italian }
    public enum CutSceneState { Played = 0, Playing = 1, Ready = 2 }

    public enum DialogState { Completed, Ready }

    public enum PickableState { Picked, Ready }

    public enum GroundType { Concrete, Grass, Dirt }



    // Tags
    public class Tags
    {
        public static readonly string Player = "Player";
        public static readonly string Enemy = "Enemy";
        public static readonly string Npc = "Npc";
        public static readonly string StartingPoint = "StartingPoint";
        //public const string GroundConcrete = "Concrete";
        //public const string GroundGrass = "Grass";
        //public const string GroundDirt = "Dirt";
    }

    public class Layers
    {
        public static readonly string SightOccluder = "SightOccluder";
        public static readonly string Ground = "Ground";
    }

    public class Constants
    {
        // Interaction
        public static readonly float InteractionCooldownTime = 1.5f;
        //public static readonly int DoorLockedState = 0;
        //public static readonly int DoorClosedState = 1;
        //public static readonly int DoorOpenState = 2;

        // Raycast
        public static readonly float RaycastVerticalOffset = 1f;

        // Resources
        public static readonly string ResourcesFolderEquipment = "Items/Equipment";
        public static readonly string ResourcesFolderMessageCollection = "MessageCollection";
        
        //public static readonly string ResourcesFolderMessageCollection = "MessageCollection";

        // Misc
        public static readonly float OrthographicAngle = 60f;

        // Layers
        //public static readonly string LayerSightOccluder = "SightOccluder";
        //public static readonly string LayerGround = "Ground";

        // Cache codes
        public static readonly string CacheCodeSceneIndex = "lvl";
        public static readonly string CacheCodePlayer = "ply";
        public static readonly string CacheCodeEquipment = "eqp";
        public static readonly string CacheCodeInventory = "inv";
    }


}
