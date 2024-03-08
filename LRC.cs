using System;

class Program
{
    static void Main(string[] args)
    {
        // Test data
        byte[] testData = { 0x01, 0x02, 0x03, 0x04 };

        // Calculate LRC for the test data
        byte lrc = CalculateLRC(testData);

        // Print the LRC value
        Console.WriteLine("LRC: 0x" + lrc.ToString("X2"));
    }

    // Method to calculate LRC for data
    static byte CalculateLRC(byte[] data)
    {
        byte lrc = 0;

        foreach (byte b in data)
        {
            lrc ^= b;
        }

        lrc = (byte)((lrc ^ 0xFF) + 1); // Take two's complement

        return lrc;
    }
}
