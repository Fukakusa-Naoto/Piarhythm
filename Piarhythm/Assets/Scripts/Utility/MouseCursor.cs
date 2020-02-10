using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
	public Texture2D texture = null;
	public bool visibleFlag = true;
	// Start is called before the first frame update
	void Start()
    {
		Cursor.visible = visibleFlag;
		//ホットスポットを画像中央に設定(TextureはTexture2D)
		Vector2 hotspot = texture.texelSize * 0.5f;
		hotspot.y *= -1;

		//カーソルの画像をTexture、ホットスポットをhotspot、表示をForceSoftware(ソフトウェアカーソルを使用)に設定
		Cursor.SetCursor(texture, hotspot, CursorMode.ForceSoftware);
	}

    // Update is called once per frame
    void Update()
    {

    }
}
