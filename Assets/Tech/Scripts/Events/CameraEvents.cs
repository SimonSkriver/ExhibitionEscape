using System;
using System.Collections.Generic;
using Ink.Runtime;

public class CameraEvents
{
    public static event Action Customization;
    public static void OpenCustomizationMenu() => Customization?.Invoke();
}
