using UnityEngine;
public static class Utils
{
    public static float LinearConversion(byte value, byte oldMin, byte oldMax, float newMin, float newMax)
    {
        int denom = (oldMax - oldMin);
        if (denom == 0)
        {
            throw new UnityException("Invalid linear conversion parameters", new System.ArgumentException("Invalid arguments: oldMin and oldMax cannot be equal", "oldMax"));
        }
        return (value - oldMin) * (newMax - newMin) / (oldMax - oldMin) + newMin;
    }
    public static float LinearConversion(int value, int oldMin, int oldMax, float newMin, float newMax)
    {
        int denom = (oldMax - oldMin);
        if (denom == 0)
        {
            throw new UnityException("Invalid linear conversion parameters", new System.ArgumentException("Invalid arguments: oldMin and oldMax cannot be equal", "oldMax"));
        }
        return (value - oldMin) * (newMax - newMin) / (oldMax - oldMin) + newMin;
    }
    public static int LinearConversion(int value, int oldMin, int oldMax, int newMin, int newMax)
    {
        int denom = (oldMax - oldMin);
        if (denom == 0)
        {
            throw new UnityException("Invalid linear conversion parameters", new System.ArgumentException("Invalid arguments: oldMin and oldMax cannot be equal", "oldMax"));
        }
        return (value - oldMin) * (newMax - newMin) / (oldMax - oldMin) + newMin;
    }
    public static double LinearConversion(double value, double oldMin, double oldMax, double newMin, double newMax)
    {
        double denom = (oldMax - oldMin);
        if (denom == 0)
        {
            throw new UnityException("Invalid linear conversion parameters", new System.ArgumentException("Invalid arguments: oldMin and oldMax cannot be equal", "oldMax"));
        }
        return (value - oldMin) * (newMax - newMin) / (oldMax - oldMin) + newMin;
    }
    public static float LinearConversion(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        float denom = (oldMax - oldMin);
        if (denom == 0f)
        {
            throw new UnityException("Invalid linear conversion parameters", new System.ArgumentException("Invalid arguments: oldMin and oldMax cannot be equal", "oldMax"));
        }
        return (value - oldMin) * (newMax - newMin) / (oldMax - oldMin) + newMin;
    }
    public static Vector3 LinearConversion(Vector3 value, Vector3 oldMin, Vector3 oldMax, Vector3 newMin, Vector3 newMax)
    {
        return new Vector3(LinearConversion(value.x, oldMin.x, oldMax.x, newMin.x, newMax.x), LinearConversion(value.y, oldMin.y, oldMax.y, newMin.y, newMax.y), LinearConversion(value.z, oldMin.z, oldMax.z, newMin.z, newMax.z));
    }
    public static Vector2 LinearConversion(Vector2 value, Vector2 oldMin, Vector2 oldMax, Vector2 newMin, Vector2 newMax)
    {
        return new Vector2(LinearConversion(value.x, oldMin.x, oldMax.x, newMin.x, newMax.x), LinearConversion(value.y, oldMin.y, oldMax.y, newMin.y, newMax.y));
    }
    public static Vector4 LinearConversion(Vector4 value, Vector4 oldMin, Vector4 oldMax, Vector4 newMin, Vector4 newMax)
    {
        return new Vector4(LinearConversion(value.x, oldMin.x, oldMax.x, newMin.x, newMax.x), LinearConversion(value.y, oldMin.y, oldMax.y, newMin.y, newMax.y), LinearConversion(value.z, oldMin.z, oldMax.z, newMin.z, newMax.z), LinearConversion(value.w, oldMin.w, oldMax.w, newMin.w, newMax.w));
    }
    public static bool ApproximatelyCheck(Vector3 First, Vector3 Second)
    {
        return First.Approximately(Second);
    }
    public static bool Approximately(this Vector3 v1, Vector3 Other)
    {
        return Mathf.Approximately(v1.sqrMagnitude, Other.sqrMagnitude) && Mathf.Approximately(v1.x, Other.x) && Mathf.Approximately(v1.y, Other.y) && Mathf.Approximately(v1.z, Other.z);
    }
    public static Vector3 MaxAbsoluteValue(this Vector3 v1, Vector3 MinimumValues)
    {
        Vector3 result;

        if (v1.x < 0f)
        {
            result.x = Mathf.Min(v1.x, -MinimumValues.x);
        }
        else
        {
            result.x = Mathf.Max(v1.x, MinimumValues.x);
        }

        if (v1.y < 0f)
        {
            result.y = Mathf.Min(v1.y, -MinimumValues.y);
        }
        else
        {
            result.y = Mathf.Max(v1.y, MinimumValues.y);
        }

        if (v1.z < 0f)
        {
            result.z = Mathf.Min(v1.z, -MinimumValues.z);
        }
        else
        {
            result.z = Mathf.Max(v1.z, MinimumValues.z);
        }

        return result;
    }
    public static Vector3 ClampAbsoluteValue(this Vector3 v1, Vector3 MinimumValues, Vector3 MaxValues)
    {
        Vector3 result;

        if (v1.x < 0f)
        {
            result.x = Mathf.Clamp(v1.x, -MaxValues.x, -MinimumValues.x);
        }
        else
        {
            result.x = Mathf.Clamp(v1.x, MinimumValues.x, MaxValues.x);
        }

        if (v1.y < 0f)
        {
            result.y = Mathf.Clamp(v1.y, -MaxValues.y, -MinimumValues.y);
        }
        else
        {
            result.y = Mathf.Clamp(v1.y, MinimumValues.y, MaxValues.y);
        }

        if (v1.z < 0f)
        {
            result.z = Mathf.Clamp(v1.z, -MaxValues.z, -MinimumValues.z);
        }
        else
        {
            result.z = Mathf.Clamp(v1.z, MinimumValues.z, MaxValues.z);
        }

        return result;
    }
    public static Vector3 MinAbsoluteValue(this Vector3 v1, Vector3 MaxValues)
    {
        Vector3 result;

        if (v1.x < 0f)
        {
            result.x = Mathf.Max(v1.x, -MaxValues.x);
        }
        else
        {
            result.x = Mathf.Min(v1.x, MaxValues.x);
        }

        if (v1.y < 0f)
        {
            result.y = Mathf.Max(v1.y, -MaxValues.y);
        }
        else
        {
            result.y = Mathf.Min(v1.y, MaxValues.y);
        }

        if (v1.z < 0f)
        {
            result.z = Mathf.Max(v1.z, -MaxValues.z);
        }
        else
        {
            result.z = Mathf.Min(v1.z, MaxValues.z);
        }

        return result;
    }
}