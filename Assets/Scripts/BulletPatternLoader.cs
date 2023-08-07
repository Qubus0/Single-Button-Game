using Assets;
using UnityEditor;
using UnityEngine;

public class BulletPatternLoader: MonoBehaviour
{
    [SerializeField] private BulletSpawner bulletSpawner;
        
    public void LoadBulletPatterns()
    {
        // Clear the list to avoid duplicates when reloading in the Editor
        bulletSpawner.bulletPatterns.Clear();
    
        string folderPath = "Assets/Assets/BulletPatternAssets";
    
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