using UnityEngine;
using System.Collections;

/// <summary>
/// Describes the location of a Waypoint in screen and world coordinates
/// </summary>
public class WaypointDescription {
    private Vector2 screenCoords_;
    private Vector3 worldPos_;

    public Vector2 screenCoords
    {
        get { return screenCoords_; }
        set { screenCoords_ = value; }
    }
    public Vector3 worldPos
    {
        get { return worldPos_; }
        set { worldPos_ = value; }
    }

    public WaypointDescription(Vector2 coords)
    {
        screenCoords_ = coords;
    }

    public string ToString()
    {
        return "Screen: " + screenCoords_ + "World: " + worldPos_;
    }
}
