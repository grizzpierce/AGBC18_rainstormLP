using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IWG {

    public class IWGLoadNextScene : MonoBehaviour {

        public Animator animator;
        public string nextSceneName;

        AsyncOperation asyncOp;
        
        void Start() {
            asyncOp = SceneManager.LoadSceneAsync( nextSceneName, LoadSceneMode.Single );
            asyncOp.allowSceneActivation = false;
        }

        void Update() {
            if (animator.GetCurrentAnimatorStateInfo( 0 ).normalizedTime >= 1f) {
                asyncOp.allowSceneActivation = true;
            }
        }
    }
}