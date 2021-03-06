using Godot;
using System;

public class TrackJoint : PathFollow2D
{
    public static float MaxConnectDist = 10.0f;
    public TrackJoint ConnectedTo = null;

    // What is the direction that trains entering this place should go? (-1 or 1)
    [Export] public int TravelDirection = 1;
    [Export] private NodePath TrackSectionPath;
    public TrackSection TrackSection;

    public override void _Ready()
    {
        Hide(); // make this only visible in the editor

        TrackSection = GetNode(TrackSectionPath) as TrackSection;

        // Connect to other joints
        var trackSectionEnds = GetTree().GetNodesInGroup("TrackJoint");
        TrackJoint closest = null;
        float distanceToClosest = float.PositiveInfinity;
        var globalPosition = GlobalPosition;
        foreach (TrackJoint trackJoint in trackSectionEnds)
        {
            if (trackJoint == this) continue;
            float distance = globalPosition.DistanceTo(trackJoint.GlobalPosition);
            if (distance < distanceToClosest && distance < MaxConnectDist)
            {
                distanceToClosest = distance;
                closest = trackJoint;
            }
        }
        ConnectedTo = closest;
    }
}