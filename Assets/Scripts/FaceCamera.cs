using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // メインカメラを一度だけ取得する
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // カメラが取得できていなければ何もしない
        if (mainCamera == null)
        {
            return;
        }

        // このオブジェクトの向きを、メインカメラの向きと完全に同じにする
        // これにより、UIは常にカメラに対してまっすぐ表示される
        transform.rotation = mainCamera.transform.rotation;
    }
}