using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static bool isMultiplayer = false;
    public static int coinsRequired = 100;  //It is temporary and will be changed after getting more screens
    public static string currentGameId = "";
    public static int scoreToWin = 500;

    public static int maxTricks = 13;

    //public static int tenFor200Bids = 10;
    //public static int tenFor200Points = 200;

    public static int minBostonTricks = 10;
    //public static int maxBostonTricks = 12;
    public static int bostonPoints = 200; //If made trick 10 - 12
    public static int nilPoints = 100;

    public static int turnMaxTime = 30;

    public static bool isSpadeActive = false;

    //    public static List<MutiplayerData> playerData;
}
