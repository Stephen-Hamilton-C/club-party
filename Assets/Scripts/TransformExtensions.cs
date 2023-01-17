using UnityEngine;

public static class TransformExtensions {
    /// <summary>
    /// Gets all immediate children in this transform.
    /// If you are using this just to iterate through all children, do not use this.
    /// Instead, iterate through the Transform with foreach.
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
