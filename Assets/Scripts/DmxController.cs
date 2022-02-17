using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DmxController
{
    public int FogIntensity { get; set; }
    public int FanIntensity { get; set; }
    public int ChannelSet { get; set; }
    public bool IsOpen { get => handle != IntPtr.Zero; }
    public int Delay { get; private set; } = 0;
    public volatile bool IsDisposed;
    public event EventHandler BeforeDataWrite;
    public event EventHandler AfterDataWrite;

    private byte[] buffer = new byte[513];
    private IntPtr handle = IntPtr.Zero;
    private Status status;

    /// <summary>
    /// Creates a new OpenDMX instance.
    /// </summary>
    public DmxController()
    {
    }

    /// <summary>
    /// Creates a new OpenDMX instance with a given delay between packet.
    /// </summary>
    /// <param name="delay">Delay between data packets in milliseconds (Fastest: 0)</param>
    public DmxController(int delay)
    {
        Delay = delay;
    }

    /// <summary>
    /// Initializes device of a given index.
    /// </summary>
    /// <exception cref="OpenDMXException"></exception>
    private void Open(uint deviceIndex)
    {
        status = FTD2XX.Open(deviceIndex, ref handle);
        status = FTD2XX.ResetDevice(handle);
        status = FTD2XX.SetDivisor(handle, (char)12);
        status = FTD2XX.SetDataCharacteristics(handle, DataBits.Bits8, StopBits.StopBits2, Parity.None);
        status = FTD2XX.SetFlowControl(handle, FlowControl.None, 0, 0);
        status = FTD2XX.ClrRts(handle);

        if (status != Status.Ok)
        {
            throw new OpenDMXException("Device could not be initialized.", status);
        }

        ClearBuffer();

        Task.Run(OnTimer);
    }

    /// <summary>
    /// Sets value of a single channel.
    /// </summary>
    private void SetChannel(int channel, byte value)
    {
        if (channel < 1 || channel > 512)
        {
            throw new ArgumentOutOfRangeException(nameof(channel), "Channel number must be between 1 and 512.");
        }

        buffer[channel] = value;
    }

    /// <summary>
    /// Sets values of a channel range.
    /// </summary>
    private void SetChannels(int startChannel, byte[] values)
    {
        if (startChannel < 1 || startChannel + values.Length > 512)
        {
            throw new ArgumentOutOfRangeException(nameof(startChannel), "Start channel number must be between 1 and 512.");
        }

        Buffer.BlockCopy(values, 0, buffer, startChannel, values.Length);
    }

    private Device[] GetDevices()
    {
        uint count = 0;

        status = FTD2XX.CreateDeviceInfoList(ref count);
        if (status != Status.Ok)
        {
            throw new OpenDMXException("Could not get devices count.", status);
        }

        var devices = new Device[count];
        var serial = new byte[16];
        var description = new byte[64];

        for (uint i = 0; i < count; i++)
        {
            devices[i] = new Device();

            status = FTD2XX.GetDeviceInfoDetail(i, ref devices[i].Flags, ref devices[i].Type, ref devices[i].ID, ref devices[i].LocId, serial, description, ref devices[i].ftHandle);
            if (status != Status.Ok)
            {
                throw new OpenDMXException("Could not get device info.", status);
            }

            devices[i].DeviceIndex = i;
            devices[i].SerialNumber = Encoding.ASCII.GetString(serial);
            devices[i].Description = Encoding.ASCII.GetString(description);

            var nullIndex = devices[i].SerialNumber.IndexOf('\0');
            if (nullIndex != -1)
                devices[i].SerialNumber = devices[i].SerialNumber.Substring(0, nullIndex);

            nullIndex = devices[i].Description.IndexOf('\0');
            if (nullIndex != -1)
                devices[i].Description = devices[i].Description.Substring(0, nullIndex);
        }

        if (status != Status.Ok)
        {
            throw new OpenDMXException("Could not get devices list.", status);
        }

        return devices;
    }

    private async void OnTimer()
    {
        while (!IsDisposed)
        {
            BeforeDataWrite?.Invoke(this, null);
            WriteBuffer();
            AfterDataWrite?.Invoke(this, null);
            await Task.Delay(Delay);
        }
    }

    private void WriteBuffer()
    {
        status = FTD2XX.Purge(handle, Purge.PurgeTx);
        status = FTD2XX.Purge(handle, Purge.PurgeRx);
        status = FTD2XX.SetBreakOn(handle);
        status = FTD2XX.SetBreakOff(handle);

        uint bytesWritten = 0;
        status = FTD2XX.Write(handle, buffer, (uint)buffer.Length, ref bytesWritten);

        if (status != Status.Ok)
        {
            throw new OpenDMXException("Data write error.", status);
        }
    }

    private void ClearBuffer()
    {
        Array.Clear(buffer, 0, buffer.Length);
        WriteBuffer();
    }

    //public void Dispose()
    //{
    //    IsDisposed = true;

    //    if (IsOpen)
    //    {
    //        ClearBuffer();
    //        FTD2XX.ResetDevice(handle);
    //        FTD2XX.Close(handle);
    //    }
    //}
    public void ConnectionOpen()
    {
        Debug.Log("Attempting to Connect to Machine");
        var devices = GetDevices();
        foreach (var d in devices)
        {
            Debug.Log($"> {d.Description} S/N: {d.SerialNumber}");
        }
        Open(devices.First().DeviceIndex);
    }
    public void ConnectionClose()
    {
        if (IsOpen)
        {
            ClearBuffer();
            FTD2XX.ResetDevice(handle);
            FTD2XX.Close(handle);
            Debug.Log("Connection Close")
        }
    }
    public void ChangeMultiIntensity()
    {
        SetChannels(ChannelSet, new byte[] { (byte)FogIntensity, (byte)FanIntensity });
    }
    public void ChangeSingleIntensity()
    {
        SetChannel(ChannelSet, (byte)FanIntensity);
    }
}

