using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SnHGameConstants", menuName = "ScriptableObjects/SnHGameConstants", order = 1)]
public class SnHGameConstants : ScriptableObject
{
    // vending machine price
    public int VMPrice = 1;

    // Number of objects to collect
    public int CollectTP;
    public int OtherIndex;
    public int CollectOther;

    public int collectTotal = 12;

    // colours to indicate in basket if completed
    public Color incompleteBackgroundColor = new Color(91, 91, 91);
    public Color completeBackgroundColor = new Color(67, 173, 92);

    // Active player background colour
    public Color activeColour = new Color(91, 91, 91, 255);
    public Color inactiveColour = new Color(255, 255, 255, 255);

    // Number of NPCs to spawn
    public int NPCs;

    // all spawn zones
    public SPZC[] spawnZones = new SPZC[] {new SPZC(-5.15f, -4.3f, -3.1f, 3.65f),
                                            new SPZC(-3.7f, -3.15f, 2.8f, 3f),
                                            new SPZC(-3.7f, -3.15f, 0.3f, 0.8f),
                                            new SPZC(-3.7f,-3.15f, -1.9f, -1.6f),
                                            new SPZC(-3.7f, -3.15f, -4.3f, -3.8f),
                                            new SPZC(-2.8f, -1.7f, -4.2f, 3.15f),
                                            new SPZC(-1.1f, 2.6f, 0.3f, 0.8f),
                                            new SPZC(-1.1f, 2.6f, -1.7f, -1.4f),
                                            new SPZC(-1.1f, 2.6f, -4.3f, -3.5f),
                                            new SPZC(3.1f, 4.3f, -4.3f, 2f),
                                            new SPZC(4.7f, 5.1f, -1f, -0.65f),
                                            new SPZC(4.7f, 5.1f, 4.3f, -3.7f),
                                            new SPZC(6.6f, 8.4f, 2.8f, 3.4f),
                                            new SPZC(6.6f, 8.4f, -4.3f, -3.3f)};

    /*    public NPCZC[] npcZones = new NPCZC[] {new NPCZC(-2.9f, 2.4f, "left"),
                                                new NPCZC(-2.9f, 1.65f, "left"),
                                                new NPCZC(-4.05f, 2.1f, "right"),
                                                new NPCZC(-4.05f, 0.0f, "right"),
                                                new NPCZC(-4.05f, -0.9f, "right"),
                                                new NPCZC(-2.9f, -0.5f, "left"),
                                                new NPCZC(-2.9f, -2.5f, "left"),
                                                new NPCZC(-2.9f, -3.25f, "left"),
                                                new NPCZC(-4.05f, -2.9f, "right"),
                                                new NPCZC(-0.3f, 1.3f, "up"),
                                                new NPCZC(1.75f, 1.3f, "up"),
                                                new NPCZC(2.35f, -0.8f, "up"),
                                                new NPCZC(-0.1f, -0.8f, "up"),
                                                new NPCZC(-0.5f, -2.9f, "up"),
                                                new NPCZC(1.7f, -2.9f, "up"),
                                                new NPCZC(4.4f, -2.9f, "right"),
                                                new NPCZC(4.4f, 0.4f, "right"),
                                                new NPCZC(4.4f, 1.7f, "right"),
                                                new NPCZC(5.5f, -2.15f, "left"),
                                                new NPCZC(5.5f, 1.2f, "left")};*/

    public NPCZC[] npcZones = new NPCZC[] {new NPCZC(-2.9f, 2.4f, "left"),
                                            new NPCZC(-2.9f, 1.65f, "left"),
                                            new NPCZC(-4.05f, 2.1f, "right"),
                                            new NPCZC(-4.05f, 0.0f, "right"),
                                            new NPCZC(-4.05f, -0.9f, "right"),
                                            new NPCZC(-2.9f, -0.5f, "left"),
                                            new NPCZC(-2.9f, -2.5f, "left"),
                                            new NPCZC(-2.9f, -3.25f, "left"),
                                            new NPCZC(-4.05f, -2.9f, "right"),
                                            new NPCZC(4.4f, -2.9f, "right"),
                                            new NPCZC(4.4f, 0.4f, "right"),
                                            new NPCZC(4.4f, 1.7f, "right"),
                                            new NPCZC(5.5f, -2.15f, "left"),
                                            new NPCZC(5.5f, 1.2f, "left")};

    // spawn constraints
    public int spawnFrequency = 5;
    public int spawnPerZone = 1;
}

// add zones to this later
public class SPZC
{
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    public SPZC(float xmin, float xmax, float ymin, float ymax)
    {
        xMin = xmin;
        xMax = xmax;
        yMin = ymin;
        yMax = ymax;
    }
}

public class NPCZC
{
    public float x;
    public float y;
    public string direction;

    public NPCZC(float xval, float yval, string d)
    {
        x = xval;
        y = yval;
        direction = d;
    }
}
