using Godot;
using System;

public interface TrackSection
{
	float Length
	{
		get;
	}

	Path2D CurrentPath
	{
		get;
	}

	bool IsOffsetOnTrack(float offset);

	TrackJoint ClosestTrackJoint(float offset);
}
