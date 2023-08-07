using Assets;
using UnityEditor;
using UnityEngine;

public class BulletPatternLoader: MonoBehaviour
{
    [SerializeField] private BulletSpawner bulletSpawner;
    [SerializeField] string folderPath = "Assets/Assets/BulletPatternAssets";
        
    public void LoadBulletPatterns()
    {
        // Clear the list to avoid duplicates when reloading in the Editor
        bulletSpawner.bulletPatterns.Clear();

        // Load all GameObject assets in the specified folder
        string[] guids = AssetDatabase.FindAssets("t:BulletPattern", new[] { folderPath });
    
        // Add loaded GameObject assets to the bulletPatterns list
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            BulletPattern pattern = AssetDatabase.LoadAssetAtPath<BulletPattern>(assetPath);
            if (pattern != null)
                bulletSpawner.bulletPatterns.Add(pattern);
        }
    }
}