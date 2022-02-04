# OpenDMX-Unity
Using Unity to control DMX

Download D2XX drivers to connect to the DMX usb
https://ftdichip.com/drivers/d2xx-drivers/

DMX Equipment used : Z-350 Fazer and Natural wind N6

Channel 1 for Z-350 Fazer
Channel 2 for Natural wind N6

DmxController.ConnectionOpen(); //To connect to the DMX

DmxController.ConnectionClose(); //To disconnect the DMX

DmxController.ChangeMultiIntensity(); //For Z-350 Fazer which has fog and fan

DmxController.ChangeSingleIntensity(); //For Natural wind N6 which has only fan
