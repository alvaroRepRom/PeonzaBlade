using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteCreator : MonoBehaviour
{
    public Camera _camera;
    int height = 1024;
    int width = 1024;
    int depth = 24;

    private void Start()
    {
        SaveTexture( CaptureScreen() );
    }

    //method to render from camera
    public Texture2D CaptureScreen()
    {
        RenderTexture renderTexture = new RenderTexture(width, height, depth);
        Rect rect = new Rect(0,0,width,height);
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        _camera.targetTexture = renderTexture;
        _camera.Render();

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture.ReadPixels( rect , 0 , 0 );
        texture.Apply();

        _camera.targetTexture = null;
        RenderTexture.active = currentRenderTexture;
        Destroy( renderTexture );

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

        return texture;
    }

    private void SaveTexture( Texture2D texture )
    {
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/Art";
        if ( !System.IO.Directory.Exists( dirPath ) )
        {
            System.IO.Directory.CreateDirectory( dirPath );
        }

        System.IO.File.WriteAllBytes( dirPath + "/R_" + Random.Range( 0 , 100000 ) + ".png" , bytes );
        Debug.Log( bytes.Length / 1024 + "Kb was saved as: " + dirPath );
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
