using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkVariables
{
    Dictionary<string, Ink.Runtime.Object> variables;

    public InkVariables(Story story) {
        // Initialize the dictionary using the global variables in the story
        variables = new Dictionary<string, Ink.Runtime.Object>();
        
        foreach(string name in story.variablesState) {
            Ink.Runtime.Object value = story.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Name: " + name + "\nValue: " + value);
        }
    }


    public void SyncVariableAndStartListening(Story story) {
        // Syncronize Variables
        SyncVariablesToStory(story);

        // Start Listening for changes in variables in Ink Story
        story.variablesState.variableChangedEvent += UpdateVariableState;
    }


    // OnDisable
    public void StopListening(Story story) => story.variablesState.variableChangedEvent -= UpdateVariableState;


    public void UpdateVariableState(string name, Ink.Runtime.Object value) {
        // Deffensive check
        if (!variables.ContainsKey(name)) { return; }

        // Change variable value
        variables[name] = value;
        Debug.Log("Updated dialogue value: " + name + " = " + value);
    }


    // Set all the C# variables to the story (Ink file)
    void SyncVariablesToStory(Story story) {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables) {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
