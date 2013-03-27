#pragma strict
private var devices : WebCamDevice[];
public var deviceName : String;
private var wct : WebCamTexture;
private var resultString : String;


function Start() {
    yield Application.RequestUserAuthorization (UserAuthorization.WebCam | UserAuthorization.Microphone);

    if (Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone)) {
       devices = WebCamTexture.devices;
       deviceName = devices[0].name;
       wct = new WebCamTexture(deviceName, 640, 480, 30);
       renderer.material.mainTexture = wct;
       wct.Play();
       resultString = "no problems";
    } else {
       resultString = "no permission!";
    }
}

function OnGUI() {
}