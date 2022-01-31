using Godot;
using System;

public class SwitchTrackSection : Node2D, TrackSection
{
	[Export] private NodePath sharedTrackJointPath;
	private TrackJoint sharedTrackJoint;
	[Export] private NodePath track1JointPath;
	private TrackJoint track1Joint;
	[Export] private NodePath track2JointPath;
	private TrackJoint track2Joint;

	[Export] private NodePath track1Path;
	private Track track1;
	[Export] private NodePath track2Path;
	private Track track2;

	[Export] private NodePath track1IndicatorPath;
	private Sprite track1Indicator;
	[Export] private NodePath track2IndicatorPath;
	private Sprite track2Indicator;

	private Track currentTrack;

	public override void _Ready()
	{
		sharedTrackJoint = GetNode(sharedTrackJointPath) as TrackJoint;
		track1Joint = GetNode(track1JointPath) as TrackJoint;
		track2Joint = GetNode(track2JointPath) as TrackJoint;

		track1 = GetNode(track1Path) as Track;
		track2 = GetNode(track2Path) as Track;

		track1Indicator = GetNode(track1IndicatorPath) as Sprite;
		track2Indicator = GetNode(track2IndicatorPath) as Sprite;

		CallDeferred("SetCurrentTrack", true);
	}

	public void SetCurrentTrack(bool isTrack1)
	{
		if (isTrack1)
		{
			currentTrack = track1;
			track1Indicator.Show();
			track2Indicator.Hide();
		}
		else
		{
			currentTrack = track2;
			track1Indicator.Hide();
			track2Indicator.Show();
		}
		sharedTrackJoint.GetParent().RemoveChild(sharedTrackJoint);
		currentTrack.AddChild(sharedTrackJoint);
	}

	public float Length
	{
		get
		{
			return currentTrack.Curve.GetBakedLength();
		}
	}

	public Path2D CurrentPath
	{
		get
		{
			return currentTrack;
		}
	}

	public bool IsOffsetOnTrack(float offset)
	{
		var bakedLength = currentTrack.Curve.GetBakedLength();
		return 0 < offset && offset < bakedLength;
	}

	public TrackJoint ClosestTrackJoint(float offset)
	{
		var endTrackJoint = currentTrack == track1 ? track1Joint : track2Joint;
		return offset < currentTrack.Curve.GetBakedLength() ? sharedTrackJoint : endTrackJoint;
	}

	public void _on_SwitchTrackSection_input_event(Node viewport, InputEvent inputEvent, int shapeIdx)
	{
		if (inputEvent.GetType() == typeof(InputEventMouseButton) && inputEvent.IsPressed())
		{
			if (currentTrack == track1) SetCurrentTrack(false);
			else SetCurrentTrack(true);
		}
	}
}
