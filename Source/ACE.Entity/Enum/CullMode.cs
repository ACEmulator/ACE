namespace ACE.Entity.Enum
{
    /// <summary>
    /// Polygon culling mode
    /// </summary>
    public enum CullMode
    {
        Landblock        = 0x0,     // not in dat, but it seems to be used for this?
        None             = 0x1,
        Clockwise        = 0x2,
        CounterClockwise = 0x3
    };
}
