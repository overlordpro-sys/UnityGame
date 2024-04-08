using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public enum PlayerCharacter {
    [ResourcePath("Controllers/Variant A")]
    A,
    [ResourcePath("Controllers/Variant B")]
    B,
    [ResourcePath("Controllers/Variant C")]
    C,
    [ResourcePath("Controllers/Variant D")]
    D
}


public class ResourcePathAttribute : Attribute {
    public string Path { get; private set; }

    public ResourcePathAttribute(string path) {
        this.Path = path;
    }

    public static string GetResourcePath(Enum Value) {
        Type Type = Value.GetType();

        FieldInfo FieldInfo = Type.GetField(Value.ToString());

        ResourcePathAttribute Attribute = FieldInfo.GetCustomAttribute(
            typeof(ResourcePathAttribute)
        ) as ResourcePathAttribute;

        return Attribute.Path;
    }
}

// NGO
public class PlayerDataKeys {
    public const string PlayerName = "PlayerName";
    public const string PlayerCharacter = "PlayerCharacter";
    public const string PlayerLobby = "GameMode";
}