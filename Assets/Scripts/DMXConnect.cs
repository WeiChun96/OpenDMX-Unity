using UnityEngine;
using UnityEngine.UI;

public class DMXConnect : MonoBehaviour
{
    public Button OpenConnection, CloseConnection;
    public DmxController dmx { get; set; }

    void Awake()
    {
        dmx = new DmxController();
    }
    void Start()
    {
        OpenConnection.onClick.AddListener(TurnOnMachine);
        CloseConnection.onClick.AddListener(TurnOffMachine);
    }

    void TurnOnMachine()
    {
        dmx.ConnectionOpen();
    }    
    void TurnOffMachine()
    {
        dmx.ConnectionClose();
    }
}