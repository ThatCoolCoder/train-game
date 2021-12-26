using Godot;
using System;

public class TrainCar : PathFollow2D
{
    // Track stuff
    [Export] private NodePath currentTrackSectionPath;
    private TrackSection currentTrackSection;
    private float directionMultiplier = 1;

    // Physics
    private static float gravitationalAcceleration = 9.8f;
    [Export] protected float mass = 1000;
    [Export] public float CurrentSpeed = 0;
    [Export] private float trackFrictionCoefficient = 0.01f; // cf of a bicycle
    private float acceleration = 0;


    public override void _Ready()
    {
        currentTrackSection = GetNode(currentTrackSectionPath) as TrackSection;
    }

    private void SwitchTrackSection()
    {
        // Move the carriage onto a different section of track if it has gone off the end of this one
        if (! currentTrackSection.IsOffsetOnTrack(Offset))
        {
            TrackJoint trackJoint = currentTrackSection.ClosestTrackJoint(Offset);
            if (trackJoint.ConnectedTo != null)
            {
                currentTrackSection = trackJoint.ConnectedTo.TrackSection;
                UpdateParentPath();
                Offset = trackJoint.ConnectedTo.Offset;
                if (trackJoint.ConnectedTo.TravelDirection == trackJoint.TravelDirection)
                {
                    directionMultiplier *= -1;
                }
            }
            else
            {
                Offset = trackJoint.Offset;
                CurrentSpeed = 0;
            }
        }
        UpdateParentPath();
    }

    private void UpdateParentPath()
    {
        if (! currentTrackSection.CurrentPath.GetChildren().Contains(this))
        {
            GetParent().RemoveChild(this);
            currentTrackSection.CurrentPath.AddChild(this);
        }
    }

    protected void Move(float delta)
    {
        ApplyForce(-Mathf.Sign(CurrentSpeed) * trackFrictionCoefficient * mass * 9.8f);
        CurrentSpeed += acceleration * delta;
        Offset += CurrentSpeed * delta * directionMultiplier;
        acceleration = 0;
    }

    protected void ApplyForce(float force)
    {
        // force is along the direction of the track
        acceleration += force / mass;
    }

    // Update is called once per frame
    public override void _Process(float delta)
    {
        Move(delta);
        SwitchTrackSection();
    }
}
