using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerCharacter {
    public string Name { get; private set; }
    public string ResourcePath { get; private set; }

    public static PlayerCharacter A = new PlayerCharacter("Variant A", "Controllers/Variant A");
    public static PlayerCharacter B = new PlayerCharacter("Variant B", "Controllers/Variant B");
    public static PlayerCharacter C = new PlayerCharacter("Variant C", "Controllers/Variant C");
    public static PlayerCharacter D = new PlayerCharacter("Variant D", "Controllers/Variant D");

    public static PlayerCharacter[] All = new PlayerCharacter[] { A, B, C, D };

    public PlayerCharacter(string name, string resourcePath) {
        this.Name = name;
        this.ResourcePath = resourcePath;
    }

    public static PlayerCharacter GetPlayerCharacter(string name) {
        foreach (PlayerCharacter character in All) {
            if (character.Name == name) {
                return character;
            }
        }
        return null;
    }

    public static PlayerCharacter GetNextCharacter(PlayerCharacter character) {
        int index = Array.IndexOf(All, character);
        index++;
        if (index >= All.Length) {
            index = 0;
        }
        return All[index];
    }

    public static PlayerCharacter GetPreviousCharacter(PlayerCharacter character) {
        int index = Array.IndexOf(All, character);
        index--;
        if (index < 0) {
            index = All.Length - 1;
        }
        return All[index];
    }
}

//public enum PlayerCharacter {
//    [ResourcePath("Controllers/Variant A")]
//    A,
//    [ResourcePath("Controllers/Variant B")]
//    B,
//    [ResourcePath("Controllers/Variant C")]
//    C,
//    [ResourcePath("Controllers/Variant D")]
//    D
//}


//public class ResourcePathAttribute : Attribute {
//    public string Path { get; private set; }

//    public ResourcePathAttribute(string path) {
//        this.Path = path;
//    }

//    public static string GetResourcePath(Enum Value) {
//        Type Type = Value.GetType();

//        FieldInfo FieldInfo = Type.GetField(Value.ToString());

//        ResourcePathAttribute Attribute = FieldInfo.GetCustomAttribute(
//            typeof(ResourcePathAttribute)
//        ) as ResourcePathAttribute;

//        return Attribute.Path;
//    }
//}