﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class RectangleMenuItemController : MenuItemController {

	public GameObject textObject;
	public TextController textController;

	public GameObject subIconObject;
	public IconController subIconController;

	public string rectangleIconBasePath = "SAO/Icons/listObj";

	// Item
	public Vector3 menuItemScale = new Vector3(.06f, .06f, .06f);
	public Vector3 hoverBoxCenter = new Vector3(0.09f, 0, -.1f);
	public Vector3 hoverBoxSize = new Vector3(1.6f, .46f, .35f);
	public Vector3 pressBoxCenter = new Vector3(0.09f, 0, 0);
	public Vector3 pressBoxSize = new Vector3(1.6f, .45f, .1f);
	public Vector3 boxScale = new Vector3(1f, 1f, 1f);
	public int sortingOrder = -1;

	// Sub Icon
	public Vector3 subIconPosition = new Vector3(-.477f, 0, 0);
	public Vector3 subIconScale = new Vector3(.015f, .015f, .015f);
	public int     subIconSortingOrder = 105;

	// Text
	public Vector3 textPosition = new Vector3(-.2433072f, 0.0769f, 0);
	public Vector3 textScale = new Vector3(.01f, .01f, .01f);
	public int     textSize = 150;
	public Color   textColorNormal = Color.gray;
	public Color   textColorRaised = Color.white;
	public Color   textColorFaded  = new Color(Color.gray.r, Color.gray.g, Color.gray.b, .588f);
	public int     textSortingOrder = 102;
	public Font    textFont = Resources.Load<Font>("SAO/Fonts/TTF/SAOUITTF-Regular");

	// Misc
	public static float yOffset = 0.0261f;
	public bool  childrenAreList;
    public static float xOffset = 0.1187f;

    public void giveData(JSONNode jsonNode, GameObject menuItem) {
		this.jsonNode = jsonNode;
		this.menuItem = menuItem;
		betterMethodName(jsonNode, menuItem);

		subIconObject = transform.FindChild("icon").gameObject;
		textObject = transform.FindChild("text").gameObject;

	}

	public void handleCreation() {
		handleHover();
		handlePress();
		handleIcon();
		handleSubIcon();
		handleText();
        StartCoroutine(waitForTransform());// handleTransform();
		handleMisc();
	}

	public void handleBoxColliderSimilarities (CollisionController colController) {
		colController.initBoxCollider();
		colController.ColliderScale = boxScale;
		colController.ColliderIsTrigger = true;
	}
	
	public void handleHover() {
		handleBoxColliderSimilarities(hoverController);
		hoverController.IsHover = true;
		hoverController.ColliderCenter = hoverBoxCenter;
		hoverController.ColliderSize = hoverBoxSize;
	}

	public void handlePress() {
		handleBoxColliderSimilarities(pressController);
		pressController.IsHover = false;
		pressController.ColliderCenter = pressBoxCenter;
		pressController.ColliderSize = pressBoxSize;
	}

	public void handleIcon() {
		iconController.Image = Resources.Load<Sprite>(rectangleIconBasePath);
	}

	public void handleSubIcon() {
		subIconController = subIconObject.GetComponent<IconController>();
		subIconController.Image = Resources.Load<Sprite>(jsonNode["_baseIconPath"]);
		subIconController.SpritePosition = subIconPosition;
		subIconController.SpriteScale = subIconScale;
	}

	public void handleText() {
		textController = textObject.GetComponent<TextController>();
		textController.Text = jsonNode["_text"];
		textController.TextFont = textFont;
		textController.TextColor = textColorNormal;
		textController.TextSize = textSize;
		textController.TextScale = textScale;
		textController.TextPosition = textPosition;
	}

	public void handleTransform() {
        if (getParentTransform(jsonNode["_parent"]).childCount % 2 == 0) { // fix
            Debug.Log(jsonNode["_text"] + " + " + yOffset * getParentTransform(jsonNode["_parent"]).childCount / 2);
            transform.localPosition = new Vector3(xOffset, (-yOffset / 2) + yOffset * getParentTransform(jsonNode["_parent"]).childCount / 2 - yOffset * getMyGroupIndex(jsonNode["_parent"], jsonNode["_text"]) , 0f);
        }
        else {
            float calc1 = getMyGroupIndex(jsonNode["_parent"], jsonNode["_text"]) * 2 + 1;
            float calc2 = getParentTransform(jsonNode["_parent"]).childCount;
           
            /*
            if (calc1 == calc2)
                Debug.Log(jsonNode["_text"] + " --> Middle");
            else if (calc1 < calc2)
               Debug.Log(jsonNode["_text"] + " --> Above");
            else if (calc1 > calc2)
                Debug.Log(jsonNode["_text"] + " --> Below");
            */

            transform.localPosition = new Vector3(xOffset, yOffset * (getParentTransform(jsonNode["_parent"]).childCount / 2) - (yOffset * getMyGroupIndex(jsonNode["_parent"], jsonNode["_text"])), 0f);
        }
        
		//transform.localRotation = rotation; // will handle this when I get to curvature
	}

	public void handleMisc() {
		iconController.SortingOrder = sortingOrder;
        setItemParent();
        transform.localScale = menuItemScale;
        if (jsonNode["_childrenAreList"] != null)
			childrenAreList = jsonNode["_childrenAreList"].AsBool;
	}

	void Start() {

	}
    
    IEnumerator waitForTransform()
    {
        yield return new WaitForSeconds(5);
        handleTransform();
    }
    
    	
}
