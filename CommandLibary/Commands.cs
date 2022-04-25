namespace Example.CommandLibrary
{
    /// <summary>
    /// The SPI G4 laser commands.
    /// </summary>
    public enum Commands : byte
    {
        /// <summary>
        /// The set RS232 baud rate.
        /// </summary>
        SetRs232BaudRate = 0x10,

        /// <summary>
        /// Get set RS232 baud rate.
        /// </summary>
        GetRs232BaudRate = 0x11,
    }
}
