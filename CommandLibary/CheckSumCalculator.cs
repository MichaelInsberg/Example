using System;
using System.Collections.Generic;

namespace Example.CommandLibary
{
    public class CheckSumCalculator
    {
        public byte CalculateCheckSum(List<byte> dataList)
        {
            if (dataList == null)
            {
                throw new ArgumentNullException(nameof(dataList));
            }

            byte sum = 0;

            foreach (var data in dataList)
            {
                sum += data;
            }

            return sum;
        }
    }
}
