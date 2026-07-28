using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

//Gaurav Singh

// This is a custom class which has all of our player variables in it
// This is what gets turned into a JSON or load from  the JSON

public class PlayerData
{
    // Save File is the name of the file as it's saved through JSON
    // it is either, "Save1", "Save2", or "Save3"
    // Player Location is the name of the Scene the player is in
    public string SaveFile, PlayerLocation, HeldItem;

    // Fire is the tracker for how much the Burning Page System has progressed
    public float Fire;
    public int FireStage;

    // How much the characters trust the player
    public int BirdTrust;

    // Inventory is a list which contains every item the player has
    // If the player does not have an item then it isn't in the list
    public List<ItemSO> Inventory;

    // triggers are a list of triggers the player has met
    // example could be if the player has finished a certain puzzle or reached a certain dialogue point
    // If the player has not met a trigger then it isn't in the list
    public List<string> triggers;
}
