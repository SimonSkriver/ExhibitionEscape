using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions {
    public void Bind(Story story) {
        story.BindExternalFunction("StartBoatCutscene", StartCutscene);
        story.BindExternalFunction("GiveCapHat", GiveCapHat);
        Debug.Log("Story binded");
    }

    public void Unbind(Story story) {
        story.UnbindExternalFunction("StartBoatCutscene");
        story.UnbindExternalFunction("GiveCapHat");
        Debug.Log("Story UNBINDED");
    }

    void StartCutscene() => EventManager.Instance.StartBoatEvent();
    void GiveCapHat() => EventManager.Instance.GiveCapHat();
}
