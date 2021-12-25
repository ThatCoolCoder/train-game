using Godot;
using System;

public static class Utils
{
    public static float ConvergeValue(float value, float target, float increment)
    {
        return value + -Sign(value - target) * increment;
    }

    public static int Sign(float value)
    {
        if (value > 0) return 1;
        else if (value == 0) return 0;
        else return -1;
    }

    public static float Constrain(float value, float min, float max)
    {
        return Mathf.Min(Mathf.Max(value, min), max);
    }
}