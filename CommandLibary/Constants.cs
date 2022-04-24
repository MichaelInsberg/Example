namespace Example.CommandLibary
{
    /// <summary>
    /// The SPI G4 write constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The packet start byte.
        /// </summary>
        public const byte PACKET_START_BYTE = 0x1B;

        /// <summary>
        /// The command start byte.
        /// </summary>
        public const int COMMAND_START_BYTE = 0x09;

        /// <summary>
        /// The packet end byte.
        /// </summary>
        public const int PACKET_END_BYTE = 0x0D;

        /// <summary>
        /// The add to parameter.
        /// </summary>
        public const int ADD_TO_PARAMETER = 2;
    }
}
