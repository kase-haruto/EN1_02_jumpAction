using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullingJump : MonoBehaviour {
    private Rigidbody rb;
    private Vector3 clickPosition;
    [SerializeField]
    private float jumpPower = 10;
    private bool isCanJump;
    private int jumpCount = 0;
    private const int kMaxJumpVal = 2;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            clickPosition = Input.mousePosition;
        }
        if (isCanJump && Input.GetMouseButtonUp(0)) {
            // クリックした座標と話した座標の差分を取得
            Vector3 dist = clickPosition - Input.mousePosition;
            // クリックとリリースが同じ座標なら無視
            if (dist.sqrMagnitude == 0) { return; }
            // 差分を標準化し、jumpPowerを掛け合わせた値を移動量とする
            rb.velocity = dist.normalized * jumpPower;
            jumpCount++;
            if (jumpCount >= kMaxJumpVal) {
                isCanJump = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (IsGrounded(collision)) {
            jumpCount = 0;
            isCanJump = true;
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (IsGrounded(collision)) { isCanJump = true; }
    }

    private void OnCollisionExit(Collision collision) {
        // 何も行わない
    }

    private bool IsGrounded(Collision collision) {
        // 衝突している点の情報
        ContactPoint[] contacts = collision.contacts;
        foreach (ContactPoint contact in contacts) {
            // 上方向を示すベクトル。長さは1
            Vector3 upVector = Vector3.up;
            // 上方向と法線の内積
            float dotUN = Vector3.Dot(upVector, contact.normal);
            // 上方向と法線の角度を計算
            float dotDeg = Mathf.Acos(dotUN) * Mathf.Rad2Deg;
            // 45度以内なら地面と見なす
            if (dotDeg <= 45) {
                return true;
            }
        }
        return false;
    }
}
