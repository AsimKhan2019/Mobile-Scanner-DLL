using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System.Net.Sockets;
using System.IO;

namespace ComToPhone
{
    public class BluetoothImageCom
    {
        public static DeviceInfo[] getConnectedDevices()
        {
            BluetoothClient bc = new BluetoothClient();
            BluetoothDeviceInfo[] devices = bc.DiscoverDevices();
            DeviceInfo[] toReturn = new DeviceInfo[devices.Length];
            for (int i = 0; i < toReturn.Length; i++)
            {
                toReturn[i] = new DeviceInfo(devices[i].DeviceName, devices[i].DeviceAddress.ToInt64());
            }
            return toReturn;
        }
        public static System.Drawing.Image takePhoto(long address)
        {
            BluetoothAddress Address = new BluetoothAddress(address);
            BluetoothClient bc = new BluetoothClient();

            Guid service = new Guid("a637c370-421e-11e1-b86c-0800200c9a66");
            try
            {
                bc.Connect(Address, service);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Could not connect to bluetooth device, check that its Bluetooth radio is on.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                //button1.IsEnabled = true;
                return null;
            };

            NetworkStream ns = bc.GetStream();

            byte[] toWrite = System.Text.Encoding.UTF8.GetBytes("Take a Pic\r\n");
            ns.Write(toWrite, 0, toWrite.Length);

            StreamReader sr = new StreamReader(ns);

            String temp = sr.ReadLine();
            if (temp == null)
                temp = "It was null?!?";

            temp = sr.ReadLine();

            int size = int.Parse(temp);

            if (size < 0)
            {
                switch (size)
                {
                    case -1:
                        //MessageBox.Show("Make sure application is started on phone.  If error persists, close app and restart.");
                        break;
                    default:
                        break;
                }
                //button1.IsEnabled = true;
                return null;
            }
            byte[] data = new byte[size];
            int offset = 0;
            int remaining = data.Length;
            //progressBar1.Maximum = remaining;
            //progressBar1.Value = 0;
            while (remaining > 0)
            {
                int read = ns.Read(data, offset, remaining);
                if (read <= 0)
                    throw new EndOfStreamException
                        (String.Format("End of stream reached with {0} bytes left to read", remaining));
                remaining -= read;
                offset += read;
                //progressBar1.Value = offset;
                //progressBar1.InvalidateVisual();
            }
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            ms.Position = 0;

            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            return image;
            //image1.Source = ConvertDrawingImageToWPFImage(image,500,600).Source;
            
        }
    }
}
