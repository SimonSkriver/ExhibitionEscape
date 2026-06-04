using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions {
    public void Bind(Story story) {
        story.BindExternalFunction("StartBoatCutscene", StartCutscene);
    }

    public void Unbind(Story story) {
        story.UnbindExternalFunction("StartBoatCutscene");
    }

    void StartCutscene() {
        Debug.Log("START CUTSCENE");
        EventManager.Instance.StartBoatEvent();
    }
}
