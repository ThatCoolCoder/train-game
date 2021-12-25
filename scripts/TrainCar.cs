using Godot;
using System;

public class TrainCar : PathFollow2D
{
    [Export] private NodePath currentTrackSectionPath;
    private TrackSection currentTrackSection;
    [Export] private float maxSpeed = 10;
    [Export] private float trackFriction = 1;
    [Export] private float acceleration = 1;
    [Export] private float crntSpeed = 0;

    private float accelerationMultiplier = 1;
    private bool controlledThisFrame = false;
    private float endOfTrackThreshold = 50;

    public override void _Ready()
    {
        currentTrackSection = GetNode(currentTrackSectionPath) as TrackSection;
    }

    private void Keybinds(float delta)
    {
        if (Input.IsActionPressed("accelerate"))
        {
            crntSpeed += acceleration * delta * accelerationMultiplier;
            controlledThisFrame = true;
        }
        if (Input.IsActionPressed("decelerate"))
        {
            crntSpeed -= acceleration * delta * accelerationMultiplier;
            controlledThisFrame = true;
        }
    }

    private void ConstrainSpeed()
    {
        crntSpeed = Mathf.Clamp(crntSpeed, -maxSpeed, maxSpeed);
    }

    private void SwitchTrackSection()
    {
        if (! currentTrackSection.IsOffsetOnTrack(Offset))
        {
            TrackJoint trackJoint = currentTrackSection.ClosestTrackJoint(Offset);
            if (trackJoint.ConnectedTo != null)
            {
                Offset = trackJoint.ConnectedTo.Offset;
                currentTrackSection = trackJoint.ConnectedTo.GetParent() as TrackSection;
                crntSpeed = Mathf.Abs(crntSpeed) * trackJoint.ConnectedTo.TravelDirection;
                accelerationMultiplier = trackJoint.ConnectedTo.TravelDirection;
            }
            else
            {
                Offset = trackJoint.Offset;
                crntSpeed = 0;
            }
        }

        // Parent self to the current track section
        if (! currentTrackSection.CurrentPath.GetChildren().Contains(this))
        {
            GetParent().RemoveChild(this);
            currentTrackSection.CurrentPath.AddChild(this);
        }
    }

    private void Move(float delta)
    {
        if (!controlledThisFrame)
        {
            crntSpeed = Utils.ConvergeValue(crntSpeed, 0.0f, trackFriction * delta);
        }
        Offset += crntSpeed * delta;
    }

    // Update is called once per frame
    public override void _Process(float delta)
    {
        controlledThisFrame = false;
        Keybinds(delta);
        ConstrainSpeed();
        Move(delta);
        SwitchTrackSection();
    }
}
