using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IWG {

    [ExecuteInEditMode]
    public class IWGLightAnimator : MonoBehaviour {

        public Vector2 position;
        public float minRadius, maxRadius;
        public Color lightColor;
        
        public Material material;

        void Update() {
            if (material == null) return;
            material.SetFloat( "_MinRadius", minRadius );
            material.SetFloat( "_MaxRadius", maxRadius );
            material.SetVector( "_LightPosition", new Vector4( position.x, position.y, 0, 0 ) );
            material.SetColor( "_LightColor", lightColor );
        }
    }
}