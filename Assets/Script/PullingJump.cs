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
            // �N���b�N�������W�Ƙb�������W�̍������擾
            Vector3 dist = clickPosition - Input.mousePosition;
            // �N���b�N�ƃ����[�X���������W�Ȃ疳��
            if (dist.sqrMagnitude == 0) { return; }
            // ������W�������AjumpPower���|�����킹���l���ړ��ʂƂ���
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
        // �����s��Ȃ�
    }

    private bool IsGrounded(Collision collision) {
        // �Փ˂��Ă���_�̏��
        ContactPoint[] contacts = collision.contacts;
        foreach (ContactPoint contact in contacts) {
            // ������������x�N�g���B������1
            Vector3 upVector = Vector3.up;
            // ������Ɩ@���̓���
            float dotUN = Vector3.Dot(upVector, contact.normal);
            // ������Ɩ@���̊p�x���v�Z
            float dotDeg = Mathf.Acos(dotUN) * Mathf.Rad2Deg;
            // 45�x�ȓ��Ȃ�n�ʂƌ��Ȃ�
            if (dotDeg <= 45) {
                return true;
            }
        }
        return false;
    }
}
