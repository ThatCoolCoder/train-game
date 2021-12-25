using Godot;
using System;

public class LinearTrackSection : Track, TrackSection
{
    [Export] private NodePath End1Path;
    private TrackJoint End1;
    [Export] private NodePath End2Path;
    private TrackJoint End2;

    public override void _Ready()
    {
        End1 = GetNode(End1Path) as TrackJoint;
        End2 = GetNode(End2Path) as TrackJoint;
        base._Ready();
    }

    public float Length
    {
        get
        {
            return Curve.GetBakedLength();
        }
    }

    public Path2D CurrentPath
    {
        get
        {
            return this;
        }
    }

    public bool IsOffsetOnTrack(float offset)
    {
        return 0 < offset && offset < Length;
    }

    public TrackJoint ClosestTrackJoint(float offset)
    {
        return offset < Length / 2 ? End1 : End2;
    }
}