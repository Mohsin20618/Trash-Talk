using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDB", menuName = "CardDB")]
public class CardDeckDB : ScriptableObject
{
    public List<CardData> list = new List<CardData>();
}
