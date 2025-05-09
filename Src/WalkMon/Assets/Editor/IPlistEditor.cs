﻿/*==============================================================================
Copyright 2017 Maxst, Inc. All Rights Reserved.
==============================================================================*/

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
#if UNITY_IPHONE
using UnityEditor.iOS.Xcode;
#endif
using System.IO;


// Unity Xcode Project Document.
// https://docs.unity3d.com/ScriptReference/iOS.Xcode.PBXProject.html
public class IPlistEditor
{
    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if ( buildTarget == BuildTarget.iOS )
        {
#if UNITY_IPHONE
            // Plist File Setting.
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;

            var iTSAppUsesNonExemptEncryptionKey = "ITSAppUsesNonExemptEncryption";
            rootDict.SetString(iTSAppUsesNonExemptEncryptionKey, "false");

             // Xcode Project File Setting.
            string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string target;            

            var unityMainTargetGuidMethod = proj.GetType().GetMethod("GetUnityMainTargetGuid");
            var unityFrameworkTargetGuidMethod = proj.GetType().GetMethod("GetUnityFrameworkTargetGuid");

            if (unityMainTargetGuidMethod != null && unityFrameworkTargetGuidMethod != null) 
            {
                target = (string)unityFrameworkTargetGuidMethod.Invoke(proj, null);                
            }
            else
            {
                target = proj.TargetGuidByName("Unity-iPhone");                
            }            

            proj.AddFrameworkToProject(target, "Accelerate.framework", false);
            proj.AddFrameworkToProject(target, "ARKit.framework", false);

            File.WriteAllText(projPath, proj.WriteToString());            
#endif
        }
    }
}