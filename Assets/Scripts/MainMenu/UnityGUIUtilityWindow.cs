using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using System.Collections;

public class UnityGUIUtility : EditorWindow
{
    private const string setAnchorsForSelectedLabel = "Set UI Anchor to Selected";
    private const string preserveAspectLabel = "Preserve Aspect";
    private const string unpreserveAspectLabel = "Unpreserve Aspect";
    private const string pleaseWaitLabel = "Please Wait";
    private const string setUIAnchorToAllLAbel = "Set UI Anchors To All";

    [MenuItem("Unity GUI Utility/Show Unity GUI Utility... %a")]
    private static void ShowEditor()
    {
        EditorWindow.GetWindow(typeof(UnityGUIUtility));
    }

    [MenuItem("Unity GUI Utility/Show Unity GUI Utility... %a", true)]
    private static bool ShowWindow()
    {
        return true;
    }

    void OnGUI()
    {
        if (GUILayout.Button(setAnchorsForSelectedLabel))
        {
            SetAnchorsToSelectedObjects();
        }
    }
    private void SetAnchorsToSelectedObjects()
    {
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        GameObject[] gameObjects = Selection.gameObjects;
        Undo.RecordObjects(gameObjects, setAnchorsForSelectedLabel);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            RectTransform selectedRectTransform = gameObjects[i].GetComponent<RectTransform>();
            if (selectedRectTransform != null)
            {
                SetAnchorToCorner(selectedRectTransform);
            }
        }
        Undo.FlushUndoRecordObjects();
    }

    private void SetAnchorToCorner(RectTransform t)
    {
        if (t.transform.parent == null)
        {
            return;
        }
        RectTransform pt = t.transform.parent.GetComponent<RectTransform>();
        if (pt == null)
            return;
        Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
            t.anchorMin.y + t.offsetMin.y / pt.rect.height);
        Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
            t.anchorMax.y + t.offsetMax.y / pt.rect.height);

        t.anchorMin = newAnchorsMin;
        t.anchorMax = newAnchorsMax;
        t.offsetMin = t.offsetMax = new Vector2(0, 0);
    }

}

