using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DMXChangeSingleIntensity : MonoBehaviour
{
    public TextMeshProUGUI FanIntensityValue;
    public Slider FanIntensitySlider;
    public int Channel = 2;
    public DMXConnect dmxSingleFan;
    public Button button;

    public void Start()
    {
        FanIntensitySlider.onValueChanged.AddListener(delegate { FanSliderValue(); });
        button.onClick.AddListener(ChangeSingleIntensity);
    }
    void ChangeSingleIntensity()
    {
        dmxSingleFan.dmx.FanIntensity = (int)FanIntensitySlider.value;
        dmxSingleFan.dmx.ChannelSet = Channel;
        Debug.Log("Sending single Data to DMX");
        dmxSingleFan.dmx.ChangeSingleIntensity();
    }
    void FanSliderValue()
    {
        FanIntensityValue.text = $"Fan : {FanIntensitySlider.value}";
    }
}
