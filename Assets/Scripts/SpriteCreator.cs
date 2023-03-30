using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCreator : MonoBehaviour
{
    public Camera _camera;
    int height = 1024;
    int width = 1024;
    int depth = 24;

    //method to render from camera
    public Sprite CaptureScreen()
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

        return sprite;
    }
}
