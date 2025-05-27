using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BDSHospital
{
    public class Door_Automatic1 : MonoBehaviour
    {
        public Animator doorAnim;
        private bool isPlayerInRange = false;
        private bool isOpen = false;

        // 잠금 해제 여부
        public bool isUnlocked = false;
        void Update()
        {
            //  잠금 해제된 경우에만 E 키로 문을 열 수 있음
            if (isUnlocked && isPlayerInRange && Input.GetKeyDown(KeyCode.E))
            {
                if (!isOpen)
                {
                    doorAnim.SetTrigger("Open");
                    isOpen = true;
                }
                else
                {
                    doorAnim.SetTrigger("Close");
                    isOpen = false;
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
            }
        }
    }
}
