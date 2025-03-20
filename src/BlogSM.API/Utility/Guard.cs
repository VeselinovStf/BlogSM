using System;

namespace BlogSM.API.Utility;

public static class Guard
{
    public static bool IsValidStringLength(int length, int lessThen, int bigerThen)
    {
        if (length < lessThen || length > bigerThen)
        {
            return false;
        }

        return true;
    }
}
