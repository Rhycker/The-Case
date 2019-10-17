using System;
using UnityEngine;

public class AssetDropdown : PropertyAttribute {

    public string ResourcePath { get; private set; }
    public Type ResourceType { get; private set; }
    public bool ShowTitle { get; private set; }

    public AssetDropdown(string resourcePath, Type resourceType = null, bool showTitle = true) {
        ResourcePath = resourcePath;
        ResourceType = resourceType;
        ShowTitle = showTitle;
    }

}