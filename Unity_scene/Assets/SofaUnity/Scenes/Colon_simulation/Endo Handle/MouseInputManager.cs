using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


public class MouseInputManager : MonoBehaviour
{

    public static MouseInputManager instance;

    [DllImport("LibRawInput")]
    private static extern bool init();

    [DllImport("LibRawInput")]
    private static extern bool kill();

    [DllImport("LibRawInput")]
    private static extern IntPtr poll();

    public const byte RE_DEVICE_CONNECT = 0;
    public const byte RE_MOUSE = 2;
    public const byte RE_DEVICE_DISCONNECT = 1;
    public string getEventName(byte id)
    {
        switch (id)
        {
            case RE_DEVICE_CONNECT: return "RE_DEVICE_CONNECT";
            case RE_DEVICE_DISCONNECT: return "RE_DEVICE_DISCONNECT";
            case RE_MOUSE: return "RE_MOUSE";
        }
        return "UNKNOWN(" + id + ")";
    }

    public float mouse_dx;
    public float mouse_dy;

    public float mouse_x;
    public float mouse_y;

    List<int> deviceIds = new List<int>();
    public int defaultSelectedDeviceId = 0;
    [Dropdown("deviceIds")]
    public int selectedDevice = 0;

    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputEvent
    {
        public int devHandle;
        public int x, y, wheel;
        public byte press;
        public byte release;
        public byte type;
    }

    public class MousePointer
    {
        public GameObject obj;
        public Vector2 position;
        public int deviceID;
        public int playerID;
        public float sensitivity;
    }
    Dictionary<int, MousePointer> pointersByDeviceId = new Dictionary<int, MousePointer>();

    void Start()
    {
        instance = this;
        bool res = init();
        Debug.Log("Init() ==> " + res);
        Debug.Log(Marshal.SizeOf(typeof(RawInputEvent)));

        enterMultipleMode();
    }

    public void OnDestroy()
    {
        instance = null;
    }

    int addCursor(int deviceId)
    {
        if (!isInit)
        {
            Debug.LogError("Not initialized");
            return -1;
        }

        MousePointer mp = null;
        pointersByDeviceId.TryGetValue(deviceId, out mp);
        if (mp != null)
        {
            Debug.LogError("This device already has a cursor");
            return -1;
        }

        Debug.Log("Adding DeviceID " + deviceId);
        if (deviceIds.Count == defaultSelectedDeviceId)
            selectedDevice = deviceId;
        deviceIds.Add(deviceId);

        mp = new MousePointer();
        pointersByDeviceId[deviceId] = mp;
        return mp.playerID;
    }

    void deleteCursor(int deviceId)
    {
        var mp = pointersByDeviceId[deviceId];
        pointersByDeviceId.Remove(mp.deviceID);
        Destroy(mp.obj);
    }

    void enterMultipleMode()
    {
        clearCursorsAndDevices();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void clearCursorsAndDevices()
    {
        pointersByDeviceId.Clear();
    }


    // Update is called once per frame
    int lastEvents = 0;
    bool isInit = true;
    void Update()
    {
        mouse_dx = mouse_dy = 0;

        // Poll the events and properly update whatever we need
        IntPtr data = poll();
        int numEvents = Marshal.ReadInt32(data);
        if (numEvents > 0) lastEvents = numEvents;
        for (int i = 0; i < numEvents; ++i)
        {
            var ev = new RawInputEvent();
            long offset = data.ToInt64() + sizeof(int) + i * Marshal.SizeOf(ev);
            ev.devHandle = Marshal.ReadInt32(new IntPtr(offset + 0));
            ev.x = Marshal.ReadInt32(new IntPtr(offset + 4));
            ev.y = Marshal.ReadInt32(new IntPtr(offset + 8));
            ev.wheel = Marshal.ReadInt32(new IntPtr(offset + 12));
            ev.press = Marshal.ReadByte(new IntPtr(offset + 16));
            ev.release = Marshal.ReadByte(new IntPtr(offset + 17));
            ev.type = Marshal.ReadByte(new IntPtr(offset + 18));

            if (ev.type == RE_DEVICE_CONNECT) addCursor(ev.devHandle);
            else if (ev.type == RE_DEVICE_DISCONNECT) deleteCursor(ev.devHandle);
            else if (ev.type == RE_MOUSE)
            {
                if (selectedDevice != ev.devHandle)
                    continue;

                MousePointer pointer = null;
                if (pointersByDeviceId.TryGetValue(ev.devHandle, out pointer))
                {
                    mouse_dx = ev.x;
                    mouse_dy = ev.y;
                    mouse_x += ev.x;
                    mouse_y += ev.y;
                }
                else
                {
                    Debug.Log("Unknown device found");
                    addCursor(ev.devHandle);
                }
            }
        }
        Marshal.FreeCoTaskMem(data);
        
    }

    void OnApplicationQuit()
    {
        kill();
    }
}
