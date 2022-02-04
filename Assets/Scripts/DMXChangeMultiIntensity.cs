using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DMXChangeMultiIntensity : MonoBehaviour
{
    public TextMeshProUGUI FogIntensityValue, FanIntensityValue;
    public Button button;
    public Slider FogIntensitySlider, FanIntensitySlider;
    public DMXConnect dmxConnection;
    public int Channel = 1;

    void Start()
    {
        
        button.onClick.AddListener(ChangeIntensity);
        FogIntensitySlider.onValueChanged.AddListener(delegate { FogSliderValue(); });
        FanIntensitySlider.onValueChanged.AddListener(delegate { FanSliderValue(); });
    }

    void ChangeIntensity()
    {
        dmxConnection.dmx.ChannelSet = Channel;
        dmxConnection.dmx.FogIntensity = (int)FogIntensitySlider.value;
        dmxConnection.dmx.FanIntensity = (int)FanIntensitySlider.value;
        Debug.Log("Sending Multiple Data to DMX");
        dmxConnection.dmx.ChangeMultiIntensity();
    }
    void FogSliderValue()
    {
        FogIntensityValue.text = $"Fog : {FogIntensitySlider.value}";
    }

    void FanSliderValue()
    {
        FanIntensityValue.text = $"Fan : {FanIntensitySlider.value}";
    }
}

