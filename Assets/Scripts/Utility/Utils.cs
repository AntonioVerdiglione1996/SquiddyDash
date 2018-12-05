using UnityEngine;
public static class Utils
{
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