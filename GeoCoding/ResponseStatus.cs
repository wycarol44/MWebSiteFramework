using System;

namespace MapQuest2
{
    public enum ResponseStatus : int
    {
        Ok = 0,
        OkBatch = 100,
        ErrorInput = 400,
        ErrorAccountKey = 403,
        ErrorUnknown = 500,
    }
}