using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
    
    [Header("Item Info")]
    public string itemName;
    [TextArea(3, 5)]
    public string description;
}

public enum ItemType
{
    GlassPiece,
    LilyofValley,
    Flower,
    Honey,
    Bottle,
}

public enum ActionType
{
    Puzzle,
    ElixirIngredient, // final ingredients
    Exchange, // for the flower to the bees
    Bottle,
}