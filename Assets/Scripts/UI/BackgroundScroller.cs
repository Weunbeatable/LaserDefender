using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD.UI
{
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] float _background_Scroll_Speed = 0.5f;
        Material _myMaterial;
        Vector2 _offset;
        // Start is called before the first frame update
        void Start()
        {
            _myMaterial = GetComponent<Renderer>().material;
            _offset = new Vector2(0f, _background_Scroll_Speed);
        }

        // Update is called once per frame
        void Update()
        {
            _myMaterial.mainTextureOffset += _offset * Time.deltaTime; // maintexture offset allows us to access the offset variable, and we multiply offset by delta time to be framerate independent
        }
    }
}