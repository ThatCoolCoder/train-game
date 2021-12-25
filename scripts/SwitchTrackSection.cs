using Godot;
using System;

public class SwitchTrackSection : Node2D, TrackSection
{
    [Export] private NodePath TrackSection1Path;
    private LinearTrackSection TrackSection1;
    [Export] private NodePath TrackSection2Path;
    private LinearTrackSection TrackSection2;
    private LinearTrackSection currentTrackSection;

    public override void _Ready()
    {
        TrackSection1 = GetNode(TrackSection1Path) as LinearTrackSection;
        TrackSection2 = GetNode(TrackSection2Path) as LinearTrackSection;
        currentTrackSection = TrackSection1;
    }

    public float Length
    {
        get
        {
            return currentTrackSection.Length;
        }
    }

    public Path2D CurrentPath
    {
        get
        {
            return currentTrackSection.CurrentPath;
        }
    }

    public bool IsOffsetOnTrack(float offset)
    {
        return currentTrackSection.IsOffsetOnTrack(offset);
    }

    public TrackJoint ClosestTrackJoint(float offset)
    {
        return currentTrackSection.ClosestTrackJoint(offset);
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            if (currentTrackSection == TrackSection1) currentTrackSection = TrackSection2;
            else currentTrackSection = TrackSection1;
        }
        GD.Print(currentTrackSection.Name);
    }
}
