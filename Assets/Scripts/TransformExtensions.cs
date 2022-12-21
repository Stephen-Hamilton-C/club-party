using UnityEngine;

public static class TransformExtensions {
    /// <summary>
    /// Gets all immediate children in this transform
    /// </summary>
    /// <returns>All immediate child transforms in this transform</returns>
    public static Transform[] GetChildren(this Transform transform) {
        var children = new Transform[transform.childCount];
        var i = 0;
        foreach (Transform child in transform) {
            children[i] = child;
            i++;
        }

        return children;
    }
}
