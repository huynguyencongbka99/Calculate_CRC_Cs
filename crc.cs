using System;
using System.IO.Ports;

class Program
{
    static void Main(string[] args)
    {
        // Instantiate a new SerialPort object
        SerialPort serialPort = new SerialPort();

        try
        {
            // Set the properties of the serial port for RS485 communication
            serialPort.PortName = "COM1"; // Change this to your COM port name
            serialPort.BaudRate = 9600; // Set your desired baud rate
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.RtsEnable = true; // Enable Request to Send (RTS) signal for RS485 control

            // Open the serial port
            serialPort.Open();

            // Data to be transmitted
            byte[] dataToSend = { 0x01, 0x02, 0x03, 0x04 }; // Example data

            // Calculate CRC for the data
            ushort crc = CalculateCRC(dataToSend);

            // Add CRC bytes to the data
            byte[] dataWithCRC = new byte[dataToSend.Length + 2];
            Array.Copy(dataToSend, dataWithCRC, dataToSend.Length);
            dataWithCRC[dataWithCRC.Length - 2] = (byte)(crc & 0xFF); // LSB first
            dataWithCRC[dataWithCRC.Length - 1] = (byte)((crc >> 8) & 0xFF); // MSB second

            // Transmit the data over RS485
            serialPort.Write(dataWithCRC, 0, dataWithCRC.Length);

            Console.WriteLine("Data transmitted successfully over RS485. Press any key to exit...");

            // Read user input to keep the program running
            Console.ReadKey();

            // Close the serial port
            serialPort.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    // Method to calculate CRC16 for data
    static ushort CalculateCRC(byte[] data)
    {
        ushort crc = 0xFFFF;

        foreach (byte b in data)
        {
            crc ^= b;

            for (int i = 0; i < 8; i++)
            {
                if ((crc & 0x0001) != 0)
                {
                    crc >>= 1;
                    crc ^= 0xA001;
                }
                else
                {
                    crc >>= 1;
                }
            }
        }

        return crc;
    }
}
