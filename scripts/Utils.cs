using Godot;
using System;

public static class Utils
{
    public static float ConvergeValue(float value, float target, float increment)
    {
        return value + -Mathf.Sign(value - target) * increment;
    }
}