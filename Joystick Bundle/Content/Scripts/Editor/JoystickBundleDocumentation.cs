using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tutorial.Wizard.JoystickBundle;
using System;

public class JoystickBundleDocumentation : TutorialWizard
{
    //required//////////////////////////////////////////////////////
    public string FolderPath = "documentation/editor/";
    public NetworkImages[] ServerImages = new NetworkImages[]
    {
        new NetworkImages{Name = "img-0.jpg", Image = null},
        new NetworkImages{Name = "img-1.jpg", Image = null},
        new NetworkImages{Name = "img-2.jpg", Image = null},
    };
    public Steps[] AllSteps = new Steps[] {
     new Steps { Name = "Get Started", StepsLenght = 0 },
    new Steps { Name = "Usage", StepsLenght = 0 },
    new Steps { Name = "Prefabs", StepsLenght = 0 },
    new Steps { Name = "bl_Joystick.cs", StepsLenght = 0 },
    };

    public override void OnEnable()
    {
        base.OnEnable();
        base.Initizalized(ServerImages, AllSteps, FolderPath);
    }

    public override void WindowArea(int window)
    {
        if (window == 0) { DrawGetStarted(); }
        else if (window == 1) { DrawUsage(); }
        else if (window == 2) { DrawPrefabs(); }
        else if (window == 3) { DrawJoystick(); }
    }

    //final required////////////////////////////////////////////////

    void DrawGetStarted()
    {
        if(subStep == 0)
        {
            DrawText("<b>Requires:</b>\n\nUnity 2018.4++\n\n- All Joystick prefabs are located in:  <color=#E0E0E089><i>Assets->Joystick Bundle->Content->Prefabs->Joysticks</i></color>\n  to use simple drag one of the prefabs in your scene <b>Canvas</b> and that's <i>(after integrate the code)</i>\n");
        }       
    }

    void DrawUsage()
    {
        DrawText("In order to use the Joystick system (not just the design) you only have to modify a few line of code.\n\nFor example if you wanna use for the player movement, with the default Unity Input System you will use something like:\n");
        DrawCodeText("Input.GetAxis(\"Vertical\");\n//and or\nInput.GetAxis(\"Horizontal\");\n");
        DrawText("Well instead of that two lines you have to use:\n");
        DrawCodeText("public bl_Joystick joystick;\n...\n\njoystick.Vertical;\n//and or\njoystick.Horizontal;\n");
        DrawText("Don't worry to replace the Unity Input code, you still can use Unity Input in Editor by simple check on the \"<b>UseFallbackOnEditor</b>\" toggle of <b>bl_Joystick.cs</b> in the inspector.\n\nYou also can set which Axis will use as fallback in <b>FallbackVerticalAxis</b> and <b>FallbackHorizontalAxis</b>.\n");
    }

    private void DrawPrefabs()
    {
        DrawText("You can select between 50+ joystick prefabs with different designs, all the prefabs are located in: <i>Assets->Joystick Bundle->Content->Prefabs->Joysticks -> *</i>, you can preview each prefabs by opening the scene JoysticksPreview which is located at: <i>Assets->Joystick Bundle->Example->Scene-> *</i>.\n\nAll prefabs are designed in Unity using separate Image components for each part of the joystick <i>(Base, Axis, Stick and any other decoration)</i>, that give you the total freedom to modify them easily in Unity, Scale, Rotate, Change Color, Change Sprites, etc... all in Unity.\n\nYou can use one of the prefabs as reference and modify to your needs using the collection of sprites include in the package.\n\nSimply drag the joystick prefab inside a Canvas in your scene hierarchy, Unpack the prefab if's necessary and then apply all modifications that you want.\n");
    }

    private void DrawJoystick()
    {
        DrawText("I already how use the joystick system to get the axis values, here I'll explain the variables in the Inspector of the script (<b>bl_Joystick.cs</b>) so you can understand what are they for and how to use.\n");
        DrawPropertieInfo("Smooth Return", "bool", "After stop using the Joystick (InputUp) the stick dot will smoothly return to the center or teleport.");
        DrawPropertieInfo("Use Fallback On Editor", "bool", "When is in editor bl_Joystick.Vertical and bl_Joystick.Horizontal will return the Unity Input system Axis instead of the joystick value?");
        DrawPropertieInfo("Stick Area", "float", "The boundary movement area of the center stick");
        DrawPropertieInfo("Fallback Vertical Axis", "string", "The Unity Input System Vertical Axis Name that will be use in Editor instead of the Joystick Axis");
        DrawPropertieInfo("Fallback Horizontal Axis", "string", "The Unity Input System Horizontal Axis Name that will be use in Editor instead of the Joystick Axis");
        DrawPropertieInfo("Stick Rect", "RectTransform", "The center dot that will move along with the touch position");
        DrawPropertieInfo("Joystick Base", "RectTransform", "The interactable UI which will detect the touch to use the joystick, any input inside that RectTransform area will be use for the Joystick");
    }

    [MenuItem("Window/JoystickBundle/Documentation")]
    static void Open()
    {
        GetWindow<JoystickBundleDocumentation>();
    }
}