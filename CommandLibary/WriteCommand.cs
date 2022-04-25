using System;
using System.Collections.Generic;

namespace Example.CommandLibrary
{
    public class WriteCommand
    {
        public Commands LaserCommand { get; }

        public List<byte> Parameter { get; }

        public List<byte> Data { get; }

        public WriteCommand(Commands laserCommand, byte[] parameter = null)
        {
            Data = BuildSendData(laserCommand, parameter);
            Parameter = parameter != null ? new List<byte>(parameter) : new List<byte>();
            LaserCommand = laserCommand;
        }

        private List<byte> BuildSendData(Commands command, byte[] paramBytes)
        {
            var parameter = paramBytes ?? Array.Empty<byte>();
            int numberOfBytes = parameter.Length + Constants.ADD_TO_PARAMETER; // Defined by protocol

            List<byte> sendData = new List<byte>();

            sendData.Add(Constants.PACKET_START_BYTE);

            sendData.Add((byte)numberOfBytes);

            sendData.Add(Constants.COMMAND_START_BYTE);

            sendData.Add((byte)command);

            foreach (var param in parameter)
            {
                sendData.Add(param);
            }

            sendData.Add(Constants.PACKET_END_BYTE);

            byte checksum = new CheckSumCalculator().CalculateCheckSum(sendData);
            sendData.Add(checksum);

            return sendData;
        }
    }
}
