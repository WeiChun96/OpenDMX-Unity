using System;
using System.Runtime.InteropServices;

public class FTD2XX
{
    private const string DLL_NAME = "FTD2XX.dll";

    [DllImport(DLL_NAME, EntryPoint = "FT_Open")]
    internal static extern Status Open(uint index, ref IntPtr ftHandle);

    [DllImport(DLL_NAME, EntryPoint = "FT_Close")]
    internal static extern Status Close(IntPtr ftHandle);

    [DllImport(DLL_NAME, EntryPoint = "FT_Write")]
    internal static extern Status Write(IntPtr ftHandle, byte[] lpBuffer, uint dwBytesToWrite, ref uint lpdwBytesWritten);

    [DllImport(DLL_NAME, EntryPoint = "FT_SetDataCharacteristics")]
    internal static extern Status SetDataCharacteristics(IntPtr ftHandle, DataBits uWordLength, StopBits uStopBits, Parity uParity);

    [DllImport(DLL_NAME, EntryPoint = "FT_SetFlowControl")]
    internal static extern Status SetFlowControl(IntPtr ftHandle, FlowControl usFlowControl, byte uXon, byte uXoff);

    [DllImport(DLL_NAME, EntryPoint = "FT_Purge")]
    internal static extern Status Purge(IntPtr ftHandle, Purge dwMask);

    [DllImport(DLL_NAME, EntryPoint = "FT_ClrRts")]
    internal static extern Status ClrRts(IntPtr ftHandle);

    [DllImport(DLL_NAME, EntryPoint = "FT_SetBreakOn")]
    internal static extern Status SetBreakOn(IntPtr ftHandle);

    [DllImport(DLL_NAME, EntryPoint = "FT_SetBreakOff")]
    internal static extern Status SetBreakOff(IntPtr ftHandle);

    [DllImport(DLL_NAME, EntryPoint = "FT_ResetDevice")]
    internal static extern Status ResetDevice(IntPtr ftHandle);

    [DllImport(DLL_NAME, EntryPoint = "FT_SetDivisor")]
    internal static extern Status SetDivisor(IntPtr ftHandle, char usDivisor);

    [DllImport(DLL_NAME, EntryPoint = "FT_CreateDeviceInfoList")]
    internal static extern Status CreateDeviceInfoList(ref uint numdevs);

    [DllImport(DLL_NAME, EntryPoint = "FT_GetDeviceInfoDetail")]
    internal static extern Status GetDeviceInfoDetail(uint index, ref uint flags, ref DeviceType chiptype, ref uint id, ref uint locid, byte[] serialnumber, byte[] description, ref IntPtr ftHandle);
}

public class Device
{
    public uint DeviceIndex;
    public uint Flags;
    public DeviceType Type;
    public uint ID;
    public uint LocId;
    public string SerialNumber;
    public string Description;
    public IntPtr ftHandle;
}

public enum DeviceType
{
    DeviceBM = 0,
    DeviceAM,
    Device100AX,
    DeviceUNKNOWN,
    Device2232,
    Device232R,
    Device2232H,
    Device4232H,
    Device232H,
    DeviceX_SERIES,
    Device4222H_0,
    Device4222H_1_2,
    Device4222H_3,
    Device4222_PROG,
    DeviceFT900,
    DeviceFT930,
    DeviceUMFTPD3A,
    Device2233HP,
    Device4233HP,
    Device2232HP,
    Device4232HP,
    Device233HP,
    Device232HP,
    Device2232HA,
    Device4232HA,
};

public enum Status
{
    Ok = 0,
    InvalidHandle,
    DeviceNotFound,
    DeviceNotOpen,
    IoError,
    InsufficientResources,
    InvalidParameter,
    InvalidBaudRate,
    deviceNotOpenedForErase,
    DeviceNotOpenedForWrite,
    FailedToWriteDevice,
    EepromReadFailed,
    EepromWriteFailed,
    EepromEraseFailed,
    EepromNotPresent,
    EepromNotProgrammed,
    InvalidArgs,
    OtherError,

};

public enum DataBits : byte
{
    Bits8 = 0x08,
    Bits7 = 0x07
}

public enum StopBits : byte
{
    StopBits1 = 0x00,
    StopBits2 = 0x02
}

public enum Parity : byte
{
    None = 0x00,
    Odd = 0x01,
    Even = 0x02,
    Mark = 0x03,
    Space = 0x04
}

public enum FlowControl : ushort
{
    None = 0x0000,
    RtsCts = 0x0100,
    DtrDsr = 0x0200,
    XonXoff = 0x0400
}

public enum Purge : byte
{
    PurgeRx = 0x01,
    PurgeTx = 0x02
}

