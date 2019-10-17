using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AssetDropdown))]
public class AssetDropdownDrawer : PropertyDrawer {

	private const int SELECT_BUTTON_WIDTH = 15;
	private const int GUI_ITEM_X_OFFSET = 3;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		position.width -= (SELECT_BUTTON_WIDTH + GUI_ITEM_X_OFFSET);
		float dropdownWidth = position.width;

		AssetDropdown assetDropdown = attribute as AssetDropdown;

        List<Object> objects;

        if (assetDropdown.ResourceType == null) {
            objects = new List<Object>(Resources.LoadAll(assetDropdown.ResourcePath));
        } else {
            objects = new List<Object>(Resources.LoadAll(assetDropdown.ResourcePath, assetDropdown.ResourceType));
        }

        objects.Insert(0, null);

        List<string> options = new List<string>();
        int index = 0;
        int selectedIndex = -1;

        string currentSelectedName = property.objectReferenceValue != null ? property.objectReferenceValue.name : string.Empty;

        foreach (Object obj in objects) {
            string name = obj == null ? "None" : obj.name;
            options.Add(name);
            
            if ((obj == null && string.IsNullOrEmpty(currentSelectedName)) ||
                (obj != null && obj.name == currentSelectedName)) {
                selectedIndex = index;
            }

            index++;
        }

        string propertyName = property.name.CapitalizeFirstChar().AddSpaceBetweenUpperChars();
        int newSelectedIndex;

        if (assetDropdown.ShowTitle) {
             newSelectedIndex = EditorGUI.Popup(position, propertyName, selectedIndex, options.ToArray());
        } else {
            newSelectedIndex = EditorGUI.Popup(position, selectedIndex, options.ToArray());
        }

        if (newSelectedIndex != selectedIndex) {
            Object newSelected = objects[newSelectedIndex];
            property.objectReferenceValue = newSelected;
        }

		bool objectSelected = objects[newSelectedIndex] != null;
		if (objectSelected) {
			position.x += dropdownWidth + GUI_ITEM_X_OFFSET;
			position.width = SELECT_BUTTON_WIDTH;
			Texture pingIcon = EditorGUIUtility.Load("Icons/pingObject.png") as Texture;
			if (GUI.Button(position, pingIcon, "label")) {
				EditorGUIUtility.PingObject(objects[newSelectedIndex]);
			}
		}
    }

}