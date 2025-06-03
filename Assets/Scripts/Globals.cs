using UnityEngine;
using System.Collections.Generic;

public static class Globals
{
    public static Bounds WorldBounds;
    public static string LastScene = null;
    public static string SpawnPointID = null;
    public static int StoryStage = 0;
    public static HashSet<string> collectedItemIDs = new HashSet<string>();
}