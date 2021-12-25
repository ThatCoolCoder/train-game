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

    private float directionMultiplier = 1;
    private bool controlledThisFrame = false;
    private float endOfTrackThreshold = 50;

    public override void _Ready()
    {
        currentTrackSection = GetNode(currentTrackSectionPath) as TrackSection;
        // TrySetCurrentTrackSection(currentTrackSection);
    }

    private void Keybinds(float delta)
    {
        if (Input.IsActionPressed("accelerate"))
        {
            crntSpeed += acceleration * delta;
            controlledThisFrame = true;
        }
        if (Input.IsActionPressed("decelerate"))
        {
            crntSpeed -= acceleration * delta;
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
                TrySetCurrentTrackSection(trackJoint.ConnectedTo.GetParent() as TrackSection);
                Offset = trackJoint.ConnectedTo.Offset;
                if (trackJoint.ConnectedTo.TravelDirection == trackJoint.TravelDirection)
                {
                    directionMultiplier *= -1;
                }
            }
            else
            {
                Offset = trackJoint.Offset;
                crntSpeed = 0;
            }
        }

        TrySetCurrentTrackSection(currentTrackSection);
    }

    private void TrySetCurrentTrackSection(TrackSection trackSection)
    {

        currentTrackSection = trackSection;
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
        Offset += crntSpeed * delta * directionMultiplier;
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
