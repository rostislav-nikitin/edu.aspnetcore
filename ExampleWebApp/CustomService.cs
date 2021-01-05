using System;

public class CustomService : ICustomService
{
    public string GetTime()
    {
        return DateTime.UtcNow.ToString();
    }
}